using SS;

namespace GUI;

public partial class MainPage : ContentPage
{

	private Spreadsheet spreadsheet;
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

	public void FileMenuNew(object sender, EventArgs e) {

	}

    public void FileMenuOpenAsync(object sender, EventArgs e)
    {

    }

    public void CellChangedValue(object sender, EventArgs e)
    {
        Entry entry = (Entry)sender;
		spreadsheet.SetContentsOfCell(entry.StyleId, entry.Text);

    }

    public void CellFocused(object sender, EventArgs e)
    {
		Entry entry = (Entry)sender;
		selectedCell.Text = entry.StyleId;
		selectedCellEntry.Text = spreadsheet.GetCellValue(entry.StyleId).ToString();
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
					Text = $"{label}{row + 1}",
					WidthRequest = 75,
					StyleId = $"{label}{row + 1}"
				};

				_cells.Add(entry.StyleId, entry);

				entry.Completed += CellChangedValue;
				entry.Focused += CellFocused;

				horiz.Add(entry);
			}

			foreach (KeyValuePair<string, Entry> entry in _cells)
			{
				spreadsheet.SetContentsOfCell(entry.Key, entry.Value.Text);
			}

			Grid.Children.Add(horiz);
		}
    }
}


