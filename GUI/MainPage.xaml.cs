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
			string action = await DisplayActionSheet("Want to save your changes?", "Cancel", null, "Save", "Create new", "");
			if (action == "Save")
			{
				FileMenuSave(sender, e);
			}
			else if (action == "Create new")
			{
				Navigation.PushAsync(new MainPage());
			}
			else if (action == "Cancle")
			{
				// do nothing
			}
		} else
		{
            Navigation.PushAsync(new MainPage());
        }
    }

    /// <summary>
    /// Saves the current spreadsheet to a file.
	/// User can save to the current (default) directory or to a custom directory.
    /// </summary>
    private async void FileMenuSave(object sender, EventArgs e)
	{
        string currDirectory = "C:\\Users\\Jiwon Park\\source\\repos\\CS3500\\spreadsheet-JiwonPark-97\\GUI\\bin\\Debug\\";
        
		// get if the user wants to save to curr path or not
		bool toCurrDirectory = await DisplayAlert("Save to", "Would you like to save to the current path?: " 
			+ System.Environment.NewLine  + currDirectory, "Yes", "New path");
		
		// save to curr directory
		if (toCurrDirectory)
		{
			// get the file name
            string filename = await DisplayPromptAsync("Save as", "File name:");

			// if cancel clicked, filename is null
			if (filename is not null)
			{
                try
                {
                    spreadsheet.Save(currDirectory + filename + ".sprd");
                }
                catch (Exception)
                {
                    await DisplayAlert("Alert", "Invalid filename", "OK");
                }
            }

		// save to a new path
        } else
		{
            // ask for a new directory
            string newDirectory = await DisplayPromptAsync("Save to", "File directory:");
			if (newDirectory is not null)
			{
                string filename = await DisplayPromptAsync("Save as", "File name:");
				try
				{
                    spreadsheet.Save(newDirectory + filename + ".sprd");
                }
				catch (Exception) 
				{
                    await DisplayAlert("Alert", "Invalid file path", "OK");
                }
            }
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
			// safety warning
            string action = await DisplayActionSheet("Want to save your changes?", "Cancel", null, "Save", "Open", "");

            if (action == "Save")
            {
                FileMenuSave(sender, e);
            }

			// ignore current changes and open a file
            else if (action == "Open")
            {
                string currDirectory = "C:\\Users\\Jiwon Park\\source\\repos\\CS3500\\spreadsheet-JiwonPark-97\\GUI\\bin\\Debug\\";

				// ask if user wants to open in the current directory
                bool fromCurrDirectory = await DisplayAlert("Open in", "Would you like to open a file in the current path?: "
																+ System.Environment.NewLine + currDirectory, "Yes", "New path");
                if (fromCurrDirectory)
				{
					try
					{
                        string filename = await DisplayPromptAsync("Open", "File name:");

						// read .sprd file
                        Spreadsheet newSpreadsheet = new Spreadsheet(currDirectory + filename + ".sprd", IsValid, s => s.ToUpper(), "six");

						// clear current spreadsheet GUI
						Clear();

						// update a spreadsheet model and entries on GUI
                        spreadsheet = newSpreadsheet;
						foreach(string cellName in spreadsheet.GetNamesOfAllNonemptyCells())
						{
							_cells[cellName].Text = spreadsheet.GetCellValue(cellName).ToString();
						}
						// default focus
						_cells["A1"].Focus();

					} catch (Exception)
					{
                        await DisplayAlert("Alert", "File does not exist", "OK");
                    }
                } else
				{
					try
					{
                        string filepath = await DisplayPromptAsync("Open", "File path:");

                        // read .sprd file
                        Spreadsheet newSpreadsheet = new Spreadsheet(filepath, IsValid, s => s.ToUpper(), "six");

                        // clear current spreadsheet GUI
                        Navigation.PushAsync(new MainPage());

                        // update a spreadsheet model and entries on GUI
                        spreadsheet = newSpreadsheet;
                        foreach (string cellName in spreadsheet.GetNamesOfAllNonemptyCells())
                        {
                            _cells[cellName].Text = spreadsheet.GetCellValue(cellName).ToString();
                        }

                        _cells["A1"].Focus();
                    }
					catch (Exception)
					{
                        await DisplayAlert("Alert", "File path invalid", "OK");
                    }

                }
            }
            else if (action == "Cancle")
            {
                // do nothing
            }
        }
        else
        {
            string currDirectory = "C:\\Users\\Jiwon Park\\source\\repos\\CS3500\\spreadsheet-JiwonPark-97\\GUI\\bin\\Debug\\";

            // ask if user wants to open in the current directory
            bool fromCurrDirectory = await DisplayAlert("Open in", "Would you like to open a file in the current path?: "
                                                            + System.Environment.NewLine + currDirectory, "Yes", "New path");
            if (fromCurrDirectory)
            {
                try
                {
                    string filename = await DisplayPromptAsync("Open", "File name:");

                    // read .sprd file
                    Spreadsheet newSpreadsheet = new Spreadsheet(currDirectory + filename, IsValid, s => s.ToUpper(), "six");

                    // clear current spreadsheet GUI
                    Navigation.PushAsync(new MainPage());

                    // update a spreadsheet model and entries on GUI
                    spreadsheet = newSpreadsheet;
                    foreach (string cellName in spreadsheet.GetNamesOfAllNonemptyCells())
                    {
                        _cells[cellName].Text = spreadsheet.GetCellValue(cellName).ToString();
                    }
                    // default focus
                    _cells["A1"].Focus();

                }
                catch (Exception)
                {
                    await DisplayAlert("Alert", "File does not exist", "OK");
                }
            }
            else
            {
                try
                {
                    string filepath = await DisplayPromptAsync("Open", "File path:");

                    // read .sprd file
                    Spreadsheet newSpreadsheet = new Spreadsheet(filepath, IsValid, s => s.ToUpper(), "six");

                    // clear current spreadsheet GUI
                    Navigation.PushAsync(new MainPage());

                    // update a spreadsheet model and entries on GUI
                    spreadsheet = newSpreadsheet;
                    foreach (string cellName in spreadsheet.GetNamesOfAllNonemptyCells())
                    {
                        _cells[cellName].Text = spreadsheet.GetCellValue(cellName).ToString();
                    }

                    _cells["A1"].Focus();
                }
                catch (Exception)
                {
                    await DisplayAlert("Alert", "File path invalid", "OK");
                }
            }
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
            } else
			{
                entry.Text = spreadsheet.GetCellValue(entry.StyleId).ToString();
            }
        }
        catch (Exception)
		{
            await DisplayAlert("Alert", "Invalid contents", "OK");
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
        Entry entry = (Entry)sender;
        try
        {
            // update the cell and its dependent cells
            string cellId = selectedCellName.Text;
            List<string> cellsToRecalculate = spreadsheet.SetContentsOfCell(cellId, entry.Text).ToList();

            foreach (string cellToRecalculate in cellsToRecalculate)
            {
                _cells[cellToRecalculate].Text = spreadsheet.GetCellValue(cellToRecalculate).ToString();
            }
            _cells[cellId].Focus();

            // make FormulaError look simpler
            if (spreadsheet.GetCellValue(cellId) is FormulaError)
            {
                entry.Text = "FormulaError";
				_cells[cellId].Text = "FormulaError";
            }
        }
        catch (Exception)
        {
            await DisplayAlert("Alert", "Invalid contents", "OK");
        }
    }

	/// <summary>
	/// Focus on the first cell (A1) by default
	/// </summary>
	private void FocusOnDefaultCell(object sender, EventArgs e)
	{
		_cells["A1"].Focus();
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
				StrokeThickness = 1,
				Content = new Label
				{
					Text = $" - ",
					WidthRequest = 25
				}
			}
			);

		// Top Row Label, for upper left corner
		foreach(var label in ROWHEADERS)
		{
			TopLabels.Add(
				new Border
				{
					Stroke = Color.FromRgb(0, 0, 0),
					StrokeThickness = 1,
					HeightRequest = 20,
					WidthRequest = 75,
					HorizontalOptions = LayoutOptions.Center,
					Content = new Label
					{
						Text = $"{label}",
                        BackgroundColor = Color.FromRgba("#7395AE"),
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
					StrokeThickness = 1,
					HeightRequest = 30,
					WidthRequest = 25,
					Content = new Label
					{
						Text = $"{row + 1}",
						VerticalTextAlignment = TextAlignment.Center,
						BackgroundColor = Color.FromRgba("#7395AE")
					}
				}) ;
			foreach(var label in ROWHEADERS)
			{
				var entry = new Entry
				{
					Text = "",
					WidthRequest = 75,
					StyleId = $"{label}{row + 1}"
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


