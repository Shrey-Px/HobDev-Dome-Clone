namespace Admin.CustomControls.Entries;

public class SingleImageBorderedEntry : Border
{
    public static readonly BindableProperty EntryTextProperty = BindableProperty.Create(
        nameof(EntryText),
        typeof(string),
        typeof(SingleImageBorderedEntry),
        string.Empty,
        BindingMode.TwoWay
    );

    public string EntryText
    {
        get => (string)GetValue(EntryTextProperty);
        set => SetValue(EntryTextProperty, value);
    }

    public static readonly BindableProperty ImgSourceProperty = BindableProperty.Create(
        nameof(ImgSource),
        typeof(string),
        typeof(SingleImageBorderedEntry),
        string.Empty,
        BindingMode.TwoWay
    );

    public string ImgSource
    {
        get => (string)GetValue(ImgSourceProperty);
        set => SetValue(ImgSourceProperty, value);
    }

    protected override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        base.OnPropertyChanged(propertyName);
        if (entry != null)
        {
            if (propertyName == EntryTextProperty.PropertyName)
            {
                entry.Text = EntryText;
            }
        }
        if (image != null)
            if (propertyName == ImgSourceProperty.PropertyName)
            {
                image.Source = ImgSource;
            }
    }

    Entry entry;
    Image image;

    public SingleImageBorderedEntry()
    {
        StrokeThickness = .5;
        StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(30) };
        Padding = new Thickness(0);
        Background = Colors.Transparent;
        this.AppThemeBinding(Border.StrokeProperty, Brush.Black, Brush.White);
        entry = new Entry
        {
            FontFamily = "InterRegular",
            FontSize = 25,
            Background = Colors.Transparent,
            HeightRequest = 70,
            VerticalOptions = LayoutOptions.Fill,
            HorizontalOptions = LayoutOptions.Fill,
        }
            .Bind(Entry.TextProperty, nameof(EntryText), BindingMode.TwoWay, source: this)
            .AppThemeBinding(Entry.TextColorProperty, Colors.Black, Colors.White)
            .AppThemeBinding(Entry.PlaceholderColorProperty, Colors.Black, Colors.White);

        this.AppThemeBinding(Entry.BackgroundProperty, Colors.White, Colors.Black);

        image = new Image { Margin = new Thickness(5, 5, 0, 5) }.Bind(
            Image.SourceProperty,
            nameof(ImgSource),
            BindingMode.TwoWay,
            source: this
        );

        Content = new Grid
        {
            RowDefinitions = Rows.Define((BodyRow.first, Auto)),
            ColumnDefinitions = Columns.Define((BodyColumn.first, Auto), (BodyColumn.second, Star)),

            Children = { image, entry.Column(BodyColumn.second) },
        };
    }
}
