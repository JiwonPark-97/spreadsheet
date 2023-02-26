/// <summary>
/// Author:    Jiwon Park
/// Partner:   None
/// Date:      20-Feb-2023
/// Course:    CS 3500, University of Utah, School of Computing
/// Copyright: CS 3500 and Jiwon Park - This work may not 
///            be copied for use in Academic Coursework.
///
/// I, Jiwon Park, certify that I wrote this code from scratch and
/// did not copy it in part or whole from another source.  All 
/// references used in the completion of the assignments are cited 
/// in my README file.
///
/// This File contains a single class Spreadsheet that inherits from AbstractSpreadsheet. 
/// </summary>

using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using SpreadsheetUtilities;
using static System.Net.Mime.MediaTypeNames;

namespace SS
{

    /// <inheritDoc\>
    public class Spreadsheet : AbstractSpreadsheet
    {
        // all nonempty cells
        private Dictionary<string, Cell> cells;

        private DependencyGraph dg;
        private bool changed;

        /// <summary>
        /// Creates an empty spreadsheet that imposes no extra validity conditions, normalizes every cell name to itself, and use the name "default" as the version.
        /// </summary>
        public Spreadsheet() : base(s => true, s => s, "default")
        {
            cells = new Dictionary<string, Cell>();
            dg = new DependencyGraph();
            changed = false;
        }

        /// <summary>
        /// Create an empty spreadsheet. Allows the user to provide
        /// a validity delegate, a normalization delegate, and a version.
        /// </summary>
        /// 
        /// <param name="isValid"> defines what valid variables look like for the application </param>
        /// <param name="normalize"> defines a normalization procedure to be applied to all valid variable strings </param>
        /// <param name="version"> defines the version of the spreadsheet (should it be saved) </param>
        public Spreadsheet(Func<string, bool> isValid, Func<string, string> normalize, string version) : base(isValid, normalize, version)
        {
            cells = new Dictionary<string, Cell>();
            dg = new DependencyGraph();
            changed = false;
        }

        /// <summary>
        /// Reads a saved spreadsheet from the file. Allow the user to provide
        /// a string representing a path to a file, a validity delegate, a normalization delegate, and a version.
        /// </summary>
        /// <param name="pathToFile"> a path to the file to read from </param>
        /// <param name="isValid"> defines what valid variables look like for the application </param>
        /// <param name="normalize"> defines a normalization procedure to be applied to all valid variable strings </param>
        /// <param name="version"> defines the version of the spreadsheet (should it be saved) </param>
        /// <exception cref="SpreadsheetReadWriteException">
        /// Thrown if anything goes wrong when reading the file.
        /// </exception>
        public Spreadsheet(string pathToFile, Func<string, bool> isValid, Func<string, string> normalize, string version) : base(isValid, normalize, version)
        {
            cells = new Dictionary<string, Cell>();
            dg = new DependencyGraph();
            changed = false;

            if (version != GetSavedVersion(pathToFile))
            {
                throw new SpreadsheetReadWriteException("The version of the saved spreadsheet does not match the version parameter provided to the constructor.");
            }
            try
            {
                string name = "";
                string contents = "";
                using (XmlReader reader = XmlReader.Create(pathToFile))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name)
                            {
                                case "spreadsheet":
                                    break;
                                case "cell":
                                    break;
                                case "name":
                                    reader.Read();
                                    name = reader.Value;
                                    break;
                                case "contents":
                                    reader.Read();
                                    contents = reader.Value;
                                    SetContentsOfCell(name, contents);
                                    break;
                                // throw
                            }
                        }
                    }
                }
            }

            // If any of the names contained in the saved spreadsheet are invalid
            catch (InvalidNameException)
            {
                throw new SpreadsheetReadWriteException("Invalid name in the file.");
            }

            // If any circular dependencies are encountered
            catch (CircularException)
            {
                throw new SpreadsheetReadWriteException("Circular dependencies in the file");
            }

            // If any invalid formulas are encountered
            catch (FormulaFormatException)
            {
                throw new SpreadsheetReadWriteException("Invalid formula format");
            }

            // Anything else goes wrong, including opening/reading/closing file.
            catch (Exception e)
            {
                throw new SpreadsheetReadWriteException(e.Message);
            }
        }

        /// <inheritDoc\>
        public override bool Changed { get => changed; protected set => changed = value; }

        /// <inheritDoc\>
        public override object GetCellContents(string name)
        {
            name = Normalize(name);

            if (!IsValidName(name) || !IsValid(name))
            {
                throw new InvalidNameException();
            }

            Cell? cell;
            if (!cells.TryGetValue(name, out cell))
            {
                // if the cell is empty, contents is an empty string
                return "";
            }
            return cell.GetContents();
        }

        /// <inheritDoc\>
        public override object GetCellValue(string name)
        {
            Normalize(name);

            if (!IsValidName(name) || !IsValid(name))
            {
                throw new InvalidNameException();
            }

            Cell? cell;
            if (!cells.TryGetValue(name, out cell))
            {
                // if the cell is empty, the contents and value are an empty strings
                return "";
            }
            return cell.GetValue();
        }

        /// <inheritDoc\>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            List<string> keys = new List<string>(cells.Keys);
            return keys;
        }

        /// <inheritDoc\>
        public override string GetSavedVersion(string filename)
        {
            if (filename is null || filename == "")
            {
                throw new SpreadsheetReadWriteException("Invalid filename");
            }

            try
            {
                string? version = "";
                using (XmlReader reader = XmlReader.Create(filename))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name)
                            {
                                case "spreadsheet":
                                    version = reader["version"];
                                    break;
                            }
                        }
                    }
                }
                if (version is not null)
                {
                    return version;
                } else
                {
                    throw new SpreadsheetReadWriteException("Invalid version");
                }
            }

            catch (Exception)
            {
                throw new SpreadsheetReadWriteException("Something went wrong");
            }
        }

        /// <inheritDoc\>
        public override void Save(string filename)
        {

            if (filename.Contains(@"\"))
            {
                int index = filename.Count();
                for (int i = filename.Count() - 1; i >= 0; i--)
                {
                    if (filename[i] == '\\')
                    {
                        index = i;
                        break;
                    }
                }
                string filepath = filename.Remove(index + 1);

                if (!Directory.Exists(filepath))
                {
                    throw new SpreadsheetReadWriteException("Path does not exist");
                }
            }

            try
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.IndentChars = "  ";

                // Create an XmlWriter inside this block, and automatically Dispose() it at the end.
                using (XmlWriter writer = XmlWriter.Create(filename, settings))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("spreadsheet"); // Starts the spreadsheet block

                    // Adds a version attribute to the spreadsheet element
                    writer.WriteAttributeString("version", Version);

                    // write cells
                    foreach (KeyValuePair<string, Cell> cell in cells)
                    {
                        writer.WriteStartElement("cell"); // Starts the cell block

                        writer.WriteElementString("name", cell.Key);

                        // formula needs "=" in front
                        if (cell.Value.GetContents() is Formula)
                        {
                            writer.WriteElementString("contents", "=" + cell.Value.GetContents().ToString());
                        }
                        else
                        {
                            writer.WriteElementString("contents", cell.Value.GetContents().ToString());

                        }

                        writer.WriteEndElement(); // Ends the cell block

                    }
                    writer.WriteEndElement(); // Ends the spreadsheet block
                    writer.WriteEndDocument();
                }
            }
            catch(Exception)
            {
                throw new SpreadsheetReadWriteException("Something went wrong saving a file");
            }
            changed = false;
        }

        /// <inheritDoc\>
        public override IList<string> SetContentsOfCell(string name, string content)
        {
            name = Normalize(name);

            if (!IsValidName(name) || !IsValid(name))
            {
                throw new InvalidNameException();
            }

            // content is number
            double number;
            if (double.TryParse(content, out number))
            {
                IList<string> cellsToRecalculate = SetCellContents(name, number);
                for (int i = 1; i < cellsToRecalculate.Count(); i++)
                {
                    cells[cellsToRecalculate[i]].Recalculate(Lookup);
                }
                return cellsToRecalculate;

            // content is formula
            } else if (content.Count() > 0 && content[0] == '=')
            {
                string formulaStr = content.Remove(0, 1);
                Formula formula = new Formula(formulaStr, Normalize, IsValid);
                IList<string> cellsToRecalculate = SetCellContents(name, formula);
                for (int i = 1; i < cellsToRecalculate.Count(); i++)
                {
                    cells[cellsToRecalculate[i]].Recalculate(Lookup);
                }
                return cellsToRecalculate;

            // content is string
            } else
            {
                IList<string> cellsToRecalculate = SetCellContents(name, content);
                for (int i = 1; i < cellsToRecalculate.Count(); i++)
                {
                    cells[cellsToRecalculate[i]].Recalculate(Lookup);
                }
                return cellsToRecalculate;
            }
        }

        /// <inheritDoc\>
        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            name = Normalize(name);

            if (!IsValidName(name) || !IsValid(name))
            {
                throw new InvalidNameException();
            }

            return dg.GetDependents(name);
        }

        /// <inheritDoc\>
        protected override IList<string> SetCellContents(string name, double number)
        {
            // error checking handled in SetContentsOfCell

            cells[name] = new Cell(name, number);

            dg.ReplaceDependees(name, new HashSet<string>());
            changed = true;

            // return a set consisting of name plus the names of all other cells whose value depends, directly or indirectly, on the named cell.
            return GetCellsToRecalculate(name).ToList();
        }

        /// <inheritDoc\>
        protected override IList<string> SetCellContents(string name, string text)
        {
            // error checking handled in SetContentsOfCell

            // If the contents is an empty string, the cell is empty - remove from cells
            if (text == "")
            {
                cells.Remove(name);
            }
            else
            {
                cells[name] = new Cell(name, text);
            }
            dg.ReplaceDependees(name, new HashSet<string>());
            changed = true;

            // return a set consisting of name plus the names of all other cells whose value depends, directly or indirectly, on the named cell.
            return GetCellsToRecalculate(name).ToList();
        }

        /// <inheritDoc\>
        protected override IList<string> SetCellContents(string name, Formula formula)
        {
            // error checking handled in SetContentsOfCell

            // save original dependencies in case of CircularException
            HashSet<string> origianlDependees = dg.GetDependees(name).ToHashSet();

            dg.ReplaceDependees(name, formula.GetVariables());

            try
            {
                // handle CircularException
                GetCellsToRecalculate(name);

                HashSet<string> variables = formula.GetVariables().ToHashSet();
                foreach (string variable in variables)
                {
                    dg.AddDependency(variable, name);
                }
                cells[name] = new Cell(name, formula, Lookup);

                // return a set consisting of name plus the names of all other cells whose value depends, directly or indirectly, on the named cell.
                return GetCellsToRecalculate(name).ToList();
            }

            catch (CircularException e)
            {
                dg.ReplaceDependees(name, origianlDependees);
                throw e;
            }
        }

        /// <summary>
        /// A helper method for checking validity of a cell (variable) name.
        /// Variables for a Spreadsheet are only valid if they are one or more letters followed by one or more digits (numbers).
        /// </summary>
        /// <param name="name"> a cell name </param>
        /// <returns> true if valid, false otherwise </returns>
        private bool IsValidName(string name)
        {
            string pattern = string.Format(@"^[a-zA-Z]+[0-9]+$");
            return (Regex.IsMatch(name, pattern));
        }

        /// <summary>
        /// Represents a cell. 
        /// </summary>
        private class Cell
        {
            // cell name
            public string name;

            // cell contents
            public object contents;

            // cell value
            public object value;

            /// <summary>
            /// constructor for number contents
            /// </summary>
            /// <param name="name"> cell name </param>
            /// <param name="number"> cell content </param>
            public Cell(string name, double number)
            {
                this.name = name;
                contents = number;
                value = number;
            }

            /// <summary>
            /// constructor for string contents
            /// </summary>
            /// <param name="name"> cell name </param>
            /// <param name="text"> cell content </param>
            public Cell(string name, string text)
            {
                this.name = name;
                contents = text;
                value = text;
            }

            /// <summary>
            /// constructor for formula contents
            /// </summary>
            /// <param name="name"> cell name </param>
            /// <param name="formula"> cell content </param>
            /// <param name="lookup"> lookup for formula </param>
            public Cell(string name, Formula formula, Func<string, double> lookup)
            {
                this.name = name;
                contents = formula;
                value = formula.Evaluate(lookup);
            }

            /// <summary>
            /// Getter for contents
            /// </summary>
            public object GetContents()
            {
                return contents;
            }

            /// <summary>
            /// Getter for value
            /// </summary>
            public object GetValue()
            {
                return value;
            }

            public void Recalculate(Func<string, double> lookup)
            {
                value = ((Formula)contents).Evaluate(lookup);
            }
        }

        /// <summary>
        /// A lookup for a cell name (variable). 
        /// </summary>
        /// <param name="name"> cell name </param>
        /// <returns> a value of the named cell </returns>
        /// <exception cref="ArgumentException">
        /// Thrown if the named cell does not contain double value
        /// </exception>
        private double Lookup(string name)
        {
            object value = GetCellValue(name);
            if (value is double)
            {
                return (double)value;
            } else
            {
                throw new ArgumentException("The named cell does not contain a double value.");
            }
        }

    }

}

