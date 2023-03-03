using SpreadsheetUtilities;
using SS;
using System.Text.RegularExpressions;

namespace GUI;

public partial class MainPage : ContentPage
{

	private AbstractSpreadsheet spreadsheet;
	private Dictionary<string, Entry> _cells;

	private readonly char[] ROWHEADERS = "ABCDEFGHIJ".ToArray();
	private readonly int ROWS = 50;

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

    private async void FileMenuNew(object sender, EventArgs e) 
	{
		if (spreadsheet.Changed)
		{
            bool answer = await DisplayAlert("File overwritten alert", "Would you like to open a new file without saving your changes?", "Yes", "No");
			if (answer)
			{
                _ = Navigation.PushAsync(new MainPage());
            }
            else
			{
				// do nothing
			}
		}
        else
		{
            _ = Navigation.PushAsync(new MainPage());
        }
	}

	private async void FileMenuSave(object sender, EventArgs e)
	{
        string result = await DisplayPromptAsync("Save as", "File name:");
		if (result is not null)
		{
			try
			{
                // TODO: fill filepath
                spreadsheet.Save("some/filepath/" + result + ".sprd");
            }
			catch (Exception)
			{
                await DisplayAlert("Alert", "Invalid filename", "OK");
            }
        } else
		{
			// cancle clicked - do nothing
        }
	}

    private async void FileMenuOpenAsync(object sender, EventArgs e)
    {
        if (spreadsheet.Changed)
        {
            bool answer = await DisplayAlert("File overwritten alert", "Would you like to open a file without saving your changes?", "Yes", "No");
            if (answer)
            {
                string result = await DisplayPromptAsync("Open", "File name:");
				if (result is not null)
				{
					try
					{
                        _ = Navigation.PushAsync(new MainPage());
						spreadsheet = new Spreadsheet(result, IsValid, s => s.ToUpper(), "six");

                    } catch (Exception)
					{
                        await DisplayAlert("Alert", "Invalid filename", "OK");
                    }

                } else
				{
                    // cancle clicked - do nothing
                }
            }
            else
            {
               // No clicked - do nothing
            }
        }
        else
        {
            string result = await DisplayPromptAsync("Open", "File name:");
            if (result is not null)
            {
				try
				{
                    _ = Navigation.PushAsync(new MainPage());
                    spreadsheet = new Spreadsheet(result, IsValid, s => s.ToUpper(), "six");
                } catch (Exception)
				{
                    await DisplayAlert("Alert", "Invalid filename", "OK");
                }
            }
            else
            {
                // cancle clicked - do nothing
            }
        }
    }

    private void CellChangedValue(object sender, EventArgs e)
    {
        Entry entry = (Entry)sender;
		try
		{
            spreadsheet.SetContentsOfCell(entry.StyleId, entry.Text);
            entry.Text = spreadsheet.GetCellValue(entry.StyleId).ToString();
        }
        catch (Exception)
		{
			// error message pop up
		}
    }

	private void FocusNextEntry(object sender, EventArgs e)
	{
		Unfocus();
        string originalCellId = ((Entry)sender).StyleId;
		string originalRow = originalCellId.Remove(0, 1);
		int nextRow = int.Parse(originalRow) + 1;
		string nextCellId = originalCellId[0] + nextRow.ToString();
		_cells[nextCellId].Focus();
    }


    private void CellFocused(object sender, EventArgs e)
    {
		Entry entry = (Entry)sender;
		selectedCellName.Text = entry.StyleId;
		selectedCellValue.Text = spreadsheet.GetCellValue(entry.StyleId).ToString();

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

    private void WidgetEntryChanged(object sender, EventArgs e)
	{
        Entry entry = (Entry)sender;
        try
        {
			string cellId = selectedCellName.Text;
            spreadsheet.SetContentsOfCell(cellId, entry.Text);
			_cells[cellId].Text = entry.Text;
        }
        catch (Exception)
        {
            // error message pop up
        }
    }

	private void FocusOnDefaultCell(object sender, EventArgs e)
	{
		_cells["A1"].Focus();
	}


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
						BackgroundColor = Color.FromRgb(168, 168 , 168),
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
						Text= $"{row + 1}",
						VerticalTextAlignment = TextAlignment.Center,
                        BackgroundColor = Color.FromRgb(168, 168, 168)
                    }
                });
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


