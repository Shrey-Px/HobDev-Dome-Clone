namespace Player.CustomControls.Entries;

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

    public static readonly BindableProperty PlaceholderTextProperty = BindableProperty.Create(
        nameof(PlaceholderText),
        typeof(string),
        typeof(SingleImageBorderedEntry),
        string.Empty,
        BindingMode.OneTime
    );

    public string PlaceholderText
    {
        get => (string)GetValue(PlaceholderTextProperty);
        set => SetValue(PlaceholderTextProperty, value);
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
            if (propertyName == PlaceholderTextProperty.PropertyName)
            {
                entry.Placeholder = PlaceholderText;
            }
        }
        if (image != null)
            if (propertyName == ImgSourceProperty.PropertyName)
            {
                image.Source = ImgSource;
            }
    }

    readonly CustomEntry? entry;
    readonly Image? image;

    public SingleImageBorderedEntry()
    {
        try
        {
            StrokeThickness = .5;
            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(7) };
#if IOS
            Padding = new Thickness(10);
#elif ANDROID
            Padding = new Thickness(10, 5);
#endif
            Stroke = Brush.Black;

            this.AppThemeBinding(
                Border.BackgroundProperty,
                Colors.White,
                Color.FromArgb("#23262A")
            );

            entry = new CustomEntry { }.Bind(
                CustomEntry.TextProperty,
                nameof(EntryText),
                BindingMode.TwoWay,
                source: this
            );

            image = new Image { }.Bind(
                Image.SourceProperty,
                nameof(ImgSource),
                BindingMode.TwoWay,
                source: this
            );

            Content = new Grid
            {
                RowDefinitions = Rows.Define((BodyRow.first, Auto)),
                ColumnDefinitions = Columns.Define(
                    (BodyColumn.first, Auto),
                    (BodyColumn.second, Star)
                ),
                ColumnSpacing = 10,
                Children = { image, entry.Column(BodyColumn.second) },
            };
        }
        catch (Exception ex)
        {
            Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
    }
}
