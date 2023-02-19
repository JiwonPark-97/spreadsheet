/// <summary>
/// Author:    Jiwon Park
/// Partner:   None
/// Date:      17-Feb-2023
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
using System.Text.RegularExpressions;
using System.Xml;
using SpreadsheetUtilities;
using static System.Net.Mime.MediaTypeNames;

// Fixing old code
namespace SS
{
    public class Spreadsheet : AbstractSpreadsheet
    {
        private Dictionary<string, Cell> cells;
        private DependencyGraph dg;
        private bool changed;

        public Spreadsheet() : base(s => true, s => s, "default")
        {
            cells = new Dictionary<string, Cell>();
            dg = new DependencyGraph();
            changed = false;
        }

        public Spreadsheet(Func<string, bool> isValid, Func<string, string> normalize, string version) : base(isValid, normalize, version)
        {
            cells = new Dictionary<string, Cell>();
            dg = new DependencyGraph();
            changed = false;
        }

        public Spreadsheet(string pathToFile, Func<string, bool> isValid, Func<string, string> normalize, string version) : base(isValid, normalize, version)
        {
            cells = new Dictionary<string, Cell>();
            dg = new DependencyGraph();
            changed = false;

            if (version != GetSavedVersion(pathToFile))
            {
                throw new SpreadsheetReadWriteException("The versions do not match.");
            }
            try
            {
                using (XmlReader reader = XmlReader.Create(pathToFile))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            string name = "";
                            string contents = "";
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
                                    break;
                            }
                            SetCellContents(name, contents);
                        }
                    }
                }
            }
            catch (InvalidNameException)
            {
                throw new SpreadsheetReadWriteException("Invalid name");
            }
            catch (CircularException)
            {
                throw new SpreadsheetReadWriteException("Circular dependencies");
            }
            catch (Exception)
            {
                throw new SpreadsheetReadWriteException("Something went wrong");
            }
        }


        public override bool Changed { get => changed; protected set => changed = value; }

        public override object GetCellContents(string name)
        {
            if (!IsValidName(name) || !IsValid(name))
            {
                throw new InvalidNameException();
            }

            name = Normalize(name);

            Cell? cell;
            if (!cells.TryGetValue(name, out cell))
            {
                // if the cell is empty, contents is an empty string
                return "";
            }
            return cell.GetContents();
        }

        public override object GetCellValue(string name)
        {
            if (!IsValidName(name) || !IsValid(name))
            {
                throw new InvalidNameException();
            }

            Normalize(name);

            Cell? cell;
            if (!cells.TryGetValue(name, out cell))
            {
                // if the cell is empty, the contents and value are an empty strings
                return "";
            }
            return cell.GetValue();
        }

        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            List<string> keys = new List<string>(cells.Keys);
            return keys;
        }

        public override string GetSavedVersion(string filename)
        {
            try
            {
                string? version = "";
                using (XmlReader reader = XmlReader.Create(filename))
                {
                    while (reader.Read())
                    {
                        if (reader.Name == "spreadsheet")
                        {
                            version = reader["version"];
                        }
                    }
                }

                // check for invalid version
                if (version is null || version == "")
                {
                    throw new SpreadsheetReadWriteException("The file version is invalid.");
                }

                return version;
            }
            catch (Exception)
            {
                throw new SpreadsheetReadWriteException("Something went wrong reading a file");
            }
        }

        public override void Save(string filename)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "  ";

            // Create an XmlWriter inside this block, and automatically Dispose() it at the end.
            using (XmlWriter writer = XmlWriter.Create(filename, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("spreadsheet");

                // This adds a version attribute to the spreadsheet element
                writer.WriteAttributeString("version", Version);

                // write cells
                foreach (KeyValuePair<string, Cell> cell in cells)
                {
                    writer.WriteStartElement("cell");
                    writer.WriteStartElement("name");
                    (cell.Key).WriteXml(writer);
                    writer.WriteStartElement("contents");

                    writer.WriteEndElement();
                    writer.WriteEndElement();
                }

                writer.WriteEndElement(); // Ends the cell block
                writer.WriteEndElement(); // Ends the spreadsheet block
                writer.WriteEndDocument();

            }

            changed = false;
        }

        public override IList<string> SetContentsOfCell(string name, string content)
        {
            if (!IsValidName(name) || !IsValid(name))
            {
                throw new InvalidNameException();
            }

            // when a valid cell name 'token' comes in as a parameter, should replace it with Normalize(token)
            Normalize(name);

            double number;
            if (double.TryParse(content, out number))
            {
                return SetCellContents(name, number);
            } else if (content[0] == '=')
            {
                string formulaStr = content.Remove(0, 1);
                Formula formula = new Formula(formulaStr, Normalize, IsValid);
                return SetCellContents(name, formula);
            } else
            {
                return SetCellContents(name, content);
            }
        }

        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            if (IsValidName(name) || !IsValid(name))
            {
                throw new InvalidNameException();
            }

            Normalize(name);

            return dg.GetDependents(name);
        }

        protected override IList<string> SetCellContents(string name, double number)
        {
            // error checking handled in SetContentsOfCell
            Normalize(name);

            cells[name] = new Cell(name, number);

            dg.ReplaceDependees(name, new HashSet<string>());
            changed = true;


            // return a set consisting of name plus the names of all other cells whose value depends, directly or indirectly, on the named cell.
            return GetCellsToRecalculate(name).ToList();
        }

        protected override IList<string> SetCellContents(string name, string text)
        {
            // error checking handled in SetContentsOfCell

            Normalize(name);

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

        protected override IList<string> SetCellContents(string name, Formula formula)
        {
            // error checking handled in SetContentsOfCell

            Normalize(name);

            // handle CircularException
            GetCellsToRecalculate(name);

            cells[name] = new Cell(name, formula, Lookup);

            HashSet<string> variables = formula.GetVariables().ToHashSet();
            foreach (string variable in variables)
            {
                dg.AddDependency(variable, name);
            }
            changed = true;

            // return a set consisting of name plus the names of all other cells whose value depends, directly or indirectly, on the named cell.
            return GetCellsToRecalculate(name).ToList();
        }

        private bool IsValidName(string name)
        {
            string pattern = string.Format(@"^[a-zA-Z]+[0-9]+$");
            return (Regex.IsMatch(name, pattern));
        }

        private class Cell
        {
            public string name;
            public object contents;
            public object value;

            public Cell(string name, double number)
            {
                this.name = name;
                contents = number;
                value = number;
            }

            public Cell(string name, string text)
            {
                this.name = name;
                contents = text;
                value = text;
            }

            public Cell(string name, Formula formula, Func<string, double> lookup)
            {
                this.name = name;
                contents = formula;
                value = formula.Evaluate(lookup);
            }

            public object GetContents()
            {
                return contents;
            }

            public object GetValue()
            {
                return value;
            }

        }

        private double Lookup(string name)
        {
            object contents = GetCellContents(name);
            if (contents is double)
            {
                return (double)contents;
            } else
            {
                throw new ArgumentException();
            }
            
        }

    }

}

