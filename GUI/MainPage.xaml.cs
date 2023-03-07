/// <summary>
/// Author:    Jiwon Park
/// Partner:   None
/// Date:      6-Mar-2023
/// Course:    CS 3500, University of Utah, School of Computing
/// Copyright: CS 3500 and Jiwon Park - This work may not 
///            be copied for use in Academic Coursework.
///
/// I, Jiwon Park, certify that I wrote this code from scratch and
/// did not copy it in part or whole from another source.  All 
/// references used in the completion of the assignments are cited 
/// in my README file.
///
/// This File contains a partial class for MainPage.xaml. The MainPage class provides methods that specifies the spreadsheet GUI's behavior.
/// </summary>

using SpreadsheetUtilities;
using SS;
using System;
using System.Text.RegularExpressions;

namespace GUI;

/// <summary>
/// A main page connected to MainPage.xamlm file's ContentPage. 
/// </summary>
public partial class MainPage : ContentPage
{
    // spreadsheet model
	private AbstractSpreadsheet spreadsheet;

    // keep track of cells on spreadsheet
	private Dictionary<string, Entry> _cells;

    // spreadsheet size (ROWHEADERS * rows)
    private readonly char[] ROWHEADERS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToArray();
	private readonly int ROWS = 99;

	/// <summary>
	/// Open a window of spreadsheet GUI and initialize features.
	/// </summary>
	public MainPage()
	{
		spreadsheet = new Spreadsheet(IsValid, s => s.ToUpper(), "six");

		_cells = new Dictionary<string, Entry>();

		InitializeComponent();

		InitializeGrid();

    }

    /// <summary>
    /// Validator for the spreadsheet. Any cell that is not in the grid should be treated as invalid. (i.e. only valid for A1-Z99)
    /// </summary>
    /// <param name="s"> a string to be checked if valid </param>
    /// <returns> true if valid, false otherwise </returns>
    private bool IsValid(string s)
	{
        string pattern = string.Format(@"^[A-Z][0-9][0-9]?$");
        return (Regex.IsMatch(s, pattern));
    }

	/// <summary>
	/// Clears cells on the current spreadsheet and reset sum section
	/// </summary>
	private void Clear()
	{
		foreach (Entry entry in _cells.Values)
		{
			entry.Text = "";
		}

        rowOrColLable.Text= "enter row/col label";
        sumResult.Text = ") = 0";
    }

    /// <summary>
    /// Creates a New empty spreadsheet in the window.
    /// If current changes not saved, ask for saving.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void FileMenuNew(object sender, EventArgs e)
	{
		if (spreadsheet.Changed)
		{
            // ask to save
			string action = await DisplayActionSheet("Want to save your changes?", "Cancel", null, "Save", "Don't save");

			if (action == "Save")
			{
                
                // ask for directory
                string directory = await DisplayPromptAsync("Save to", "File directory:");
                if (directory is not null)
                {

                    // ask for filename
                    string filename = await DisplayPromptAsync("Save as", "File name:");
                    if (filename is not null)
                    {
                        try
                        {
                            spreadsheet.Save(directory + "\\" + filename + ".sprd");
                            Clear();
                            spreadsheet = new Spreadsheet(IsValid, s => s.ToUpper(), "six");
                            _cells["A1"].Focus();
                        }
                        catch(Exception)
                        {
                            await DisplayAlert("Alert", "Invalid directory", "OK");
                        }
                    }
                    else
                    {
                        // do nothing - cancel clicked (when asked for a file name)
                    }
                }
                else
                {
                    // do nothing - cancel clicked (when asked for a directory)
                }
            }

            // ignore the current changes and open empty spreadsheet
			else if (action == "Don't save")
			{
                Clear();
                spreadsheet = new Spreadsheet(IsValid, s => s.ToUpper(), "six");
                _cells["A1"].Focus();
            }
            else
			{
				// do nothing - cancel clicked
			}

        // spreadsheet has not been changed
		} else
		{
            Clear();
            spreadsheet = new Spreadsheet(IsValid, s => s.ToUpper(), "six");
            _cells["A1"].Focus();
        }
    }

    /// <summary>
    /// Saves the current spreadsheet to a file.
	/// Ask user for a directory to save to, and a file name to save as.
    /// </summary>
    private async void FileMenuSave(object sender, EventArgs e)
	{

        // ask for directory
		string directory = await DisplayPromptAsync("Save to", "File directory:");
        if (directory is not null)
        {

            // ask for filename
            string filename = await DisplayPromptAsync("Save as", "File name:");
            if (filename is not null)
            {
                try
                {
                    spreadsheet.Save(directory + "\\" + filename + ".sprd");
                }
                catch
                {
                    await DisplayAlert("Alert", "Invalid directory", "OK");
                }
            } else
            {
                // do nothing - cancel clicked (when asked for a file name)
            }
        } else
        {
            // do nothing - cancel clicked (when asked for a directory)
        }
    }

    /// <summary>
    /// Opens a spreadsheet file from given directory.
    /// Ask user for a directory to open in and a filename to open. 
    /// Called from FileMenuOpen.
    /// </summary>
    private async void Open(object sender, EventArgs e)
    {
        // ask for directory
        string fileDirectory = await DisplayPromptAsync("Open", "File directory:");
		if (fileDirectory is not null)
		{

            // ask for file name
            string filename = await DisplayPromptAsync("Open", "File name:");
			if (filename is not null)
			{
                try
                {
                    Spreadsheet newSpreadsheet = new Spreadsheet(fileDirectory + "\\" + filename + ".sprd", IsValid, s => s.ToUpper(), "six"); spreadsheet.Save(fileDirectory + filename + ".sprd");
                    Clear();
                    _cells["A1"].Focus();

                    // update a spreadsheet model and entries on GUI
                    spreadsheet = newSpreadsheet;
                    foreach (string cellName in spreadsheet.GetNamesOfAllNonemptyCells())
                    {
                        _cells[cellName].Text = spreadsheet.GetCellValue(cellName).ToString();
                    }
                }
                catch (Exception)
                {
                    await DisplayAlert("Alert", "Invalid file path", "OK");

                }
            } else
			{
				// do nothing - cancel clicked (when asked for a file name)
			}

        } else
		{
			// do nothing - cancel clicked (when asked for a file path)
		}
    }

    /// <summary>
    /// Opens a spreadsheet file from given directory
    /// If current changes not saved, ask for saving.
    /// </summary>
    /// <param name="sender"> </param>
    /// <param name="e"></param>
    private async void FileMenuOpen(object sender, EventArgs e)
    {
		if (spreadsheet.Changed)
		{
            // ask to save
			string action = await DisplayActionSheet("Want to save your changes?", "Cancel", null, "Save", "Don't save");
			if (action == "Save")
			{

                // ask for directory
                string directory = await DisplayPromptAsync("Save to", "File directory:");
                if (directory is not null)
                {

                    // ask for file name
                    string filename = await DisplayPromptAsync("Save as", "File name:");
                    if (filename is not null)
                    {
                        try
                        {
                            spreadsheet.Save(directory + "\\" + filename + ".sprd");
                            Open(sender, e);
                        }
                        catch
                        {
                            await DisplayAlert("Alert", "Invalid file path", "OK");
                        }
                    }
                    else
                    {
                        // do nothing - cancel clicked (when asked for a file name)
                    }
                }
                else
                {
                    // do nothing - cancel clicked (when asked for a directory)
                }
			} 
            else if(action == "Don't save")
			{
				Open(sender, e);
			} 
            else
			{
				// do nothing - cancel clicked (when asked to save)
			}

        // spreadsheet has not been changed
		} else
		{
			Open(sender, e);
		}
    }

    /// <summary>
    /// Displays a help pop up that contains a description of how to use the spreadsheet.
    /// </summary>
    private async void Help(object sender, EventArgs e)
	{
        string help = "" 
            + Environment.NewLine 
            + ""
            + Environment.NewLine
            + ""
            + Environment.NewLine
            + "" 
            + Environment.NewLine
            + "" 
            + Environment.NewLine
            + "" 
            + Environment.NewLine
            + "" 
            + Environment.NewLine
            + "";
        await DisplayAlert("How to use", help, "OK");
    }

    /// <summary>
    /// Displays a pop up for "what's this error?" menu. Explains possible errors.
    /// </summary>
    private async void ErrorDescription(object sender, EventArgs e)
    {
        string errorDescription =
            "Circular Dependencies:"
            + Environment.NewLine
            + "     Circular dependencies are encountered."
            + Environment.NewLine
            + "     e.g. A1 = B2, B2 = C3, C3 = A1"
            + Environment.NewLine + Environment.NewLine
            + "Invalid Directory:"
            + Environment.NewLine
            + "     The given directory or the file doesn't exist."
            + Environment.NewLine
            + "      Make sure not to include \\ at the end of directory."
            + Environment.NewLine
            + "      Example directory:"
            + Environment.NewLine
            + "     C:\\Users\\UserName\\source\\repos\\CS3500\\spreadsheet\\GUI\\bin\\Debug"
            + Environment.NewLine + Environment.NewLine
            + "Invalid Formula:"
            + Environment.NewLine
            + "     The input formula is incorrect or contains invalid variables."
            + Environment.NewLine
            + "     Check if your formula refers to any cell that's not on the current sheet"
            + Environment.NewLine + Environment.NewLine
            + "Formula error:"
            + Environment.NewLine
            + "     The input formula refers to a non-numeric value cell or contains division by 0."
            + Environment.NewLine + Environment.NewLine
            + "Invalid label:" 
            + Environment.NewLine
            + "     The input row/column label is not valid."
            + Environment.NewLine
            + "         Valid row labels: 1-99"
            + Environment.NewLine
            + "         Valid column labels: A-Z"
            ;

        await DisplayAlert("Possible errors", errorDescription, "OK");
    }

    /// <summary>
    /// Updates a cell and its dependent cells for changed contents. 
    /// Called when a cell gets unfocused.
    /// </summary>
    private async void CellChangedValue(object sender, EventArgs e)
    {
        Entry entry = (Entry)sender;

        // change cell color back to default color (unfocused)
        entry.BackgroundColor = Color.FromRgba("#FFFFEFD5");

        try
		{
			// update cells that depend on this cell
            List<string> cellsToRecalculate = spreadsheet.SetContentsOfCell(entry.StyleId, entry.Text).ToList();

            // first item in cellsToRecalculate is changed cell itself - update only its dependents
			cellsToRecalculate.Remove(entry.StyleId);
			foreach (string cellToRecalculate in cellsToRecalculate)
			{
				_cells[cellToRecalculate].Text = spreadsheet.GetCellValue(cellToRecalculate).ToString();
			}

			// display value
            if (spreadsheet.GetCellValue(entry.StyleId) is FormulaError) 
			{
				entry.Text = "FormulaError";
            }
            else
			{
                entry.Text = spreadsheet.GetCellValue(entry.StyleId).ToString();
            }
        }
        catch (CircularException)
        {
            await DisplayAlert("Alert", "Circular dependency is detected", "OK");
            entry.Focus();
        }
        catch (FormulaFormatException)
        {
            await DisplayAlert("Alert", "Invalid formula", "OK");
            entry.Focus();
        }
        catch (InvalidNameException)
        {
            await DisplayAlert("Alert", "Invalid formula", "OK");
            entry.Focus();
        }

        // for safety. if anything else goes wrong, display the system error message.
        catch (Exception exception)
        {
            await DisplayAlert("Alert", exception.Message, "OK");
            entry.Focus();
        }
    }

	/// <summary>
	/// Focus on the next cell (right below the previously selected cell).
    /// Called when cell is complete (enter hit).
	/// </summary>
	private void FocusNextEntry(object sender, EventArgs e)
	{
		Unfocus();

        // calculate the next cell
        string originalCellId = ((Entry)sender).StyleId;
		string originalRow = originalCellId.Remove(0, 1);
		int nextRow = int.Parse(originalRow) + 1;
		string nextCellId = originalCellId[0] + nextRow.ToString();

		_cells[nextCellId].Focus();
    }

	/// <summary>
	/// Updates the widgets and displays contents in the selected cell.
    /// Called when cell is focused. 
	/// </summary>
    private void CellFocused(object sender, EventArgs e)
    {
		Entry entry = (Entry)sender;
        entry.BackgroundColor = Color.FromRgba("#8FBC8F");

        // update cell name widget
		selectedCellName.Text = entry.StyleId;

        // update cell value widget
		object value = spreadsheet.GetCellValue(entry.StyleId);
		if (value is FormulaError)
		{
			selectedCellValue.Text = "FormulaError";
		}
		else
		{
            selectedCellValue.Text = spreadsheet.GetCellValue(entry.StyleId).ToString();
        }

        // update cell contents widget and the selected cell
        object contents = spreadsheet.GetCellContents(entry.StyleId);

        if (contents is Formula)
        {
            entry.Text = "=" + contents.ToString();
			selectedCellEntry.Text = "=" + contents.ToString();
        } 
        else
        {
            entry.Text = contents.ToString();
			selectedCellEntry.Text = contents.ToString();
        }    
	}

	/// <summary>
	/// Updates a cell and its dependent cells for contents changed from the widget.
    /// Called when widget entry gets unfocused.
	/// </summary>
    private async void WidgetEntryChanged (object sender, EventArgs e)
	{
        // prevent error message - somehow opening empty spreadsheet causes widget entry to get focused.
        if (selectedCellName.Text != "cell name")
        {

            Entry entry = (Entry)sender;

            // update the cell and its dependent cells
            string cellId = selectedCellName.Text;
            try
            {
                List<string> cellsToRecalculate = spreadsheet.SetContentsOfCell(cellId, entry.Text).ToList();

                foreach (string cellToRecalculate in cellsToRecalculate)
                {
                    _cells[cellToRecalculate].Text = spreadsheet.GetCellValue(cellToRecalculate).ToString();
                }

                // make FormulaError look simpler
                if (spreadsheet.GetCellValue(cellId) is FormulaError)
                {
                    entry.Text = "FormulaError";
                    _cells[cellId].Text = "FormulaError";
                }
            }
            catch (CircularException)
            {
                await DisplayAlert("Alert", "Circular dependency is detected", "OK");
            }
            catch (FormulaFormatException)
            {
                await DisplayAlert("Alert", "Invalid formula", "OK");
            }
            catch (InvalidNameException)
            {
                await DisplayAlert("Alert", "Invalid formula", "OK");
            }

            // for safety. if anything else goes wrong, display the system error message.
            catch (Exception exception)
            {
                await DisplayAlert("Alert", exception.Message, "OK");
            }
        }

    }

    /// <summary>
    /// Move focus from widget entry to its corresponding cell.
    /// Called when widget entry gets completed.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void FocusOnCellEntry(object sender, EventArgs e)
    {
        string cellId = selectedCellName.Text;
        _cells[cellId].Focus();
    }

    /// <summary>
    /// Focus on the first cell (A1) by default
    /// </summary>
    private void FocusOnDefaultCell(object sender, EventArgs e)
	{
         if (selectedCellName.Text != "A1")
        {
            _cells["A1"].Focus();

        }
    }

    /// <summary>
    /// Adds up an entire row or column and displays the result.
    /// Called when "calculate" button clicked.
    /// </summary>
    private async void Sum(object sender, EventArgs e)
    {
        // first make input into uppercase
        string labelName = rowOrColLable.Text.ToUpper();
        rowOrColLable.Text = labelName;

        double sum = 0;

        // check validity of the input label
        string ColFormat = string.Format("^[A-Z]$");
        string RowFormat = string.Format("^[0-9][0-9]?$");
        bool validCol = Regex.IsMatch(labelName, ColFormat);
        bool validRow = Regex.IsMatch(labelName, RowFormat);

        if (!validCol && !validRow)
        {
            await DisplayAlert("Alert", "Invalid label", "OK");
            rowOrColLable.Text = "";
        }

        // input label is valid
        else
        {
            // sum entire row
            int labelNum;
            if (int.TryParse(labelName, out labelNum))
            {
                string pattern = string.Format("^[A-Z]" + labelNum + "\\b");
                foreach (string cellName in _cells.Keys)
                {
                    if (Regex.IsMatch(cellName, pattern))
                    {
                        object cellValue = spreadsheet.GetCellValue(cellName);
                        if (cellValue is double)
                        {
                            sum += (double)cellValue;
                        }
                    }

                }
            }

            // sum entire column
            else
            {
                string pattern = string.Format(labelName + "[0-9][0-9]?$");
                foreach (string cellName in _cells.Keys)
                {
                    if (Regex.IsMatch(cellName, pattern))
                    {
                        object cellValue = spreadsheet.GetCellValue(cellName);
                        if (cellValue is double)
                        {
                            sum += (double)cellValue;
                        }
                    }
                }
            }
        }

        sumResult.Text = ") = " + sum.ToString();
    }

    /// <summary>
    /// Initializes the spreadsheet grid
    /// </summary>
    private void InitializeGrid()
    {
        // Upper left corner
        TopLabels.Add(
            new Border
            {
                Stroke = Color.FromRgb(0, 0, 0),
                StrokeThickness = 0,
                Content = new Label
                {
                    Text = $"   - ",
                    WidthRequest = 34
                }
            }
            );

        // Top Row Label, for upper left corner
        foreach (var label in ROWHEADERS)
        {
            TopLabels.Add(
                new Border
                {
                    Stroke = Color.FromRgb(255, 255, 255),
                    StrokeThickness = 1,
                    HeightRequest = 20,
                    WidthRequest = 75,
                    HorizontalOptions = LayoutOptions.Center,
                    Content = new Label
                    {
                        Text = $"{label}",
                        BackgroundColor = Color.FromRgba("#8FBC8F"),
                        HorizontalTextAlignment = TextAlignment.Center
                    }
                }
                );
        }

        // All Spreadsheet Grid Cells
        for (int row = 0; row < ROWS; row++)
        {
            var horiz = new HorizontalStackLayout();
            // Left Column Labels
            horiz.Add(
                new Border
                {
                    Stroke = Color.FromRgba("#FFFFEFD5"),
                    StrokeThickness = 1,
                    HeightRequest = 30,
                    WidthRequest = 35,
                    Content = new Label
                    {
                        Text = $"  {row + 1}",
                        VerticalTextAlignment = TextAlignment.Center,
                        BackgroundColor = Color.FromRgba("#8FBC8F")
                    }
                });
            foreach (var label in ROWHEADERS)
            {
                var entry = new Entry
                {
                    //Text = "",
                    WidthRequest = 75,
                    StyleId = $"{label}{row + 1}",
                    BackgroundColor = Color.FromRgba("#FFFFEFD5")
                };

                _cells.Add(entry.StyleId, entry);

                entry.Completed += FocusNextEntry;
                entry.Focused += CellFocused;
                entry.Unfocused += CellChangedValue;

                horiz.Add(entry);
            }

            Grid.Children.Add(horiz);
        }
    }
}


