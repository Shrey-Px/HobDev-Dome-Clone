namespace Player.CustomControls.Entries;

public class BorderedEntry : Border
{
    public static readonly BindableProperty EntryTextProperty = BindableProperty.Create(
        nameof(EntryText),
        typeof(string),
        typeof(BorderedEntry),
        string.Empty,
        BindingMode.TwoWay,
        propertyChanged: EntryTextPropetyChanged
    );

    private static void EntryTextPropetyChanged(
        BindableObject bindable,
        object oldValue,
        object newValue
    )
    {
        BorderedEntry control = (BorderedEntry)bindable;
        if (control != null)
        {
            control.entry.Text = newValue?.ToString();
        }
    }

    public string EntryText
    {
        get => (string)GetValue(EntryTextProperty);
        set => SetValue(EntryTextProperty, value);
    }

    public static readonly BindableProperty PlaceholderTextProperty = BindableProperty.Create(
        nameof(PlaceholderText),
        typeof(string),
        typeof(BorderedEntry),
        string.Empty,
        BindingMode.OneTime,
        propertyChanged: PlaceholderTextPropetyChanged
    );

    private static void PlaceholderTextPropetyChanged(
        BindableObject bindable,
        object oldValue,
        object newValue
    )
    {
        BorderedEntry control = (BorderedEntry)bindable;
        if (control != null)
        {
            control.entry.Placeholder = newValue?.ToString();
        }
    }

    public string PlaceholderText
    {
        get => (string)GetValue(PlaceholderTextProperty);
        set => SetValue(PlaceholderTextProperty, value);
    }

    public CustomEntry entry;

    public BorderedEntry()
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

            Content = entry;
        }
        catch (Exception ex)
        {
            Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
    }
}
