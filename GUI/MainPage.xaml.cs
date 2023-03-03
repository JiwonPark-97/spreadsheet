using SS;

namespace GUI;

public partial class MainPage : ContentPage
{

	private AbstractSpreadsheet spreadsheet;
	private Dictionary<string, Entry> _cells;

	private readonly char[] ROWHEADERS = "ABCDEFGHIJ".ToArray();
	private readonly int ROWS = 50;

	public MainPage()
	{
		spreadsheet = new Spreadsheet();

		_cells = new Dictionary<string, Entry>();

		InitializeComponent();

		InitializeGrid();

    }

    private void FileMenuNew(object sender, EventArgs e) {

	}

    private void FileMenuOpenAsync(object sender, EventArgs e)
    {

    }

    private void CellChangedValue(object sender, EventArgs e)
    {
        Unfocus();
        Entry entry = (Entry)sender;
		spreadsheet.SetContentsOfCell(entry.StyleId, entry.Text);
    }

	private void FocusNextEntry(object sender, EventArgs e)
	{
        string originalCell = ((Entry)sender).StyleId;
		string originalRow = originalCell.Remove(0, 1);
		int nextRow = int.Parse(originalRow) + 1;
		string nextCell = originalCell[0] + nextRow.ToString();
		_cells[nextCell].Focus();
    }


    private void CellFocused(object sender, EventArgs e)
    {
		Entry entry = (Entry)sender;
		selectedCellName.Text = entry.StyleId;
		selectedCellValue.Text = spreadsheet.GetCellValue(entry.StyleId).ToString();
		selectedCellEntry.Text = spreadsheet.GetCellContents(entry.StyleId).ToString();
    }

	//private void CellFocus(object sender, EventArgs e)
 //   {
 //       Focus();
 //   }


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
					Text = $"{label}{row + 1}",
					WidthRequest = 75,
					StyleId = $"{label}{row + 1}"
				};

				_cells.Add(entry.StyleId, entry);

				entry.Completed += CellChangedValue;
				entry.Completed += FocusNextEntry;
				entry.Focused += CellFocused;

				horiz.Add(entry);
			}

			foreach (KeyValuePair<string, Entry> entry in _cells)
			{
				spreadsheet.SetContentsOfCell(entry.Key, entry.Value.Text);
				if (entry.Key == "A1")
				{
					entry.Value.Focus();
				}
			}

			Grid.Children.Add(horiz);

		}


    }
}


