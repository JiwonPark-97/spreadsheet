using SpreadsheetUtilities;
using SS;
using System.Text.RegularExpressions;

namespace GUI;

public partial class MainPage : ContentPage
{

	private AbstractSpreadsheet spreadsheet;
	private Dictionary<string, Entry> _cells;

    private readonly char[] ROWHEADERS = "ABCDEFGHIJK".ToArray();
    //private readonly char[] ROWHEADERS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToArray();
	private readonly int ROWS = 50;

	/// <summary>
	/// Open a window of spreadsheet GUI
	/// </summary>
	public MainPage()
	{
		spreadsheet = new Spreadsheet(IsValid, s => s.ToUpper(), "six");

		_cells = new Dictionary<string, Entry>();

		InitializeComponent();

		InitializeGrid();

    }

    /// <summary>
    /// Validator for the spreadsheet. Any cell that is not in the grid should be treated as invalid.
	/// i.e. only valid for A1-Z99
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    private bool IsValid(string s)
	{
        string pattern = string.Format(@"^[A-Z][0-9][0-9]?$");
        return (Regex.IsMatch(s, pattern));
    }

	/// <summary>
	/// Clears the current spreadsheet
	/// </summary>
	private void Clear()
	{
		foreach (Entry entry in _cells.Values)
		{
			entry.Text = "";
		}
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
			string action = await DisplayActionSheet("Want to save your changes?", "Cancel", null, "Save", "Don't save", "");
			if (action == "Save")
			{
				FileMenuSave(sender, e);
                Clear();
                spreadsheet = new Spreadsheet(IsValid, s => s.ToUpper(), "six");
                _cells["A1"].Focus();
            }
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
		} else
		{
            Clear();
            spreadsheet = new Spreadsheet(IsValid, s => s.ToUpper(), "six");
            _cells["A1"].Focus();
        }
    }

    /// <summary>
    /// Saves the current spreadsheet to a file.
	/// User can save to the current (default) directory or to a custom directory.
    /// </summary>
    private async void FileMenuSave(object sender, EventArgs e)
	{
		string directory = await DisplayPromptAsync("Save to", "File directory:");
        if (directory is not null)
        {
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

    private async void Open(object sender, EventArgs e)
    {
        string fileDirectory = await DisplayPromptAsync("Open", "File path:");
		if (fileDirectory is not null)
		{
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
                    await DisplayAlert("Alert", "File path invalid or file doesn't exist", "OK");

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
	/// Opens a spreadsheet file from filepath given.
	/// If current changes not saved, ask for saving.
	/// </summary>
	/// <param name="sender"> </param>
	/// <param name="e"></param>
    private async void FileMenuOpen(object sender, EventArgs e)
    {
		if (spreadsheet.Changed)
		{
			string action = await DisplayActionSheet("Want to save your changes?", "Cancel", null, "Save", "Don't save", "");
			if (action == "Save")
			{
				FileMenuSave(sender, e);
				Open(sender, e);
			} else if(action == "Don't save")
			{
				Open(sender, e);
			} else
			{
				// do nothing - cancel clicked
			}
		} else
		{
			Open(sender, e);
		}
    }

    /// <summary>
    /// Displays a help popup that describes how to use the spreadsheet
    /// </summary>
    private async void Help(object sender, EventArgs e)
	{
        await DisplayAlert("How to use", "blah", "OK");
    }

	/// <summary>
	/// Updates a cell and its dependent cells for changed contents
	/// </summary>
    private async void CellChangedValue(object sender, EventArgs e)
    {
        Entry entry = (Entry)sender;
		try
		{
			// update cells that depend on this cell
            List<string> cellsToRecalculate = spreadsheet.SetContentsOfCell(entry.StyleId, entry.Text).ToList();
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
            await DisplayAlert("Alert", "Invalid variable in formula: 1", "OK");
            entry.Focus();
        }
        catch (InvalidNameException)
        {
            await DisplayAlert("Alert", "Invalid variable in formula: 2", "OK");
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
	/// Focus on the next cell (right below the previously selected cell)
	/// </summary>
	private void FocusNextEntry(object sender, EventArgs e)
	{
		Unfocus();
        string originalCellId = ((Entry)sender).StyleId;
		string originalRow = originalCellId.Remove(0, 1);
		int nextRow = int.Parse(originalRow) + 1;
		string nextCellId = originalCellId[0] + nextRow.ToString();
		_cells[nextCellId].Focus();
    }

	/// <summary>
	/// Updates the widgets and displays contents in the selected cell
	/// </summary>
    private void CellFocused(object sender, EventArgs e)
    {
		Entry entry = (Entry)sender;
		selectedCellName.Text = entry.StyleId;

		object value = spreadsheet.GetCellValue(entry.StyleId);
		if (value is FormulaError)
		{
			selectedCellValue.Text = "FormulaError";
		}
		else
		{
            selectedCellValue.Text = spreadsheet.GetCellValue(entry.StyleId).ToString();
        }

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
	/// Updates a cell and its dependent cells for contents changed from the widget
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
                await DisplayAlert("Alert", "Invalid variable in formula: 3", "OK");
            }
            catch (InvalidNameException)
            {
                await DisplayAlert("Alert", "Invalid variable in formula: 4", "OK");
            }

            // for safety. if anything else goes wrong, display the system error message.
            catch (Exception exception)
            {
                await DisplayAlert("Alert", exception.Message, "OK");
                //_cells[cellId].Focus();
            }
        }

    }

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
		foreach(var label in ROWHEADERS)
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
		for(int row = 0; row< ROWS; row++)
		{
			var horiz = new HorizontalStackLayout();
			// Left Column Lables
			horiz.Add(
				new Border
				{
					Stroke = Color.FromRgb(0, 0, 0),
					StrokeThickness = 0,
					HeightRequest = 30,
					WidthRequest = 35,
					Content = new Label
					{
						Text = $"  {row + 1}",
						VerticalTextAlignment = TextAlignment.Center,
						BackgroundColor = Color.FromRgba("#8FBC8F")
					}
				}) ;
			foreach(var label in ROWHEADERS)
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


