namespace GUI;

public partial class MainPage : ContentPage
{
	private readonly char[] ROWHEADERS = "ABCDEFGHIJ".ToArray();
	private readonly int ROWS = 50;

	public MainPage()
	{
		InitializeComponent();

		InitializeGrid();
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
						BackgroundColor = Color.FromRgb(200, 200 , 250),
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
                        BackgroundColor = Color.FromRgb(200, 200, 250)
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

				entry.Completed += CellCahngedValue;
				entry.Focused += CellFocused;

				horiz.Add(entry);
			}

			Grid.Children.Add(horiz);
		}
    }
}


