namespace Player.CustomControls.Pickers;

public class CustomPicker : CustomBorder
{
    public static readonly BindableProperty CustomPickerSelectedItemProperty =
        BindableProperty.Create(
            nameof(CustomPickerSelectedItem),
            typeof(object),
            typeof(CustomPicker),
            null,
            BindingMode.TwoWay
        );

    public object CustomPickerSelectedItem
    {
        get => (object)GetValue(CustomPickerSelectedItemProperty);
        set => SetValue(CustomPickerSelectedItemProperty, value);
    }

    // bindable property for picker item source
    public static readonly BindableProperty PickerItemSourceProperty = BindableProperty.Create(
        nameof(PickerItemSource),
        typeof(IList),
        typeof(CustomPicker),
        null,
        BindingMode.TwoWay
    );

    public IList PickerItemSource
    {
        get => (IList)GetValue(PickerItemSourceProperty);
        set => SetValue(PickerItemSourceProperty, value);
    }

    protected override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        base.OnPropertyChanged(propertyName);

        if (propertyName == nameof(CustomPickerSelectedItem))
        {
            if (picker != null)
                picker.SelectedItem = CustomPickerSelectedItem;
        }
        else if (propertyName == nameof(PickerItemSource))
        {
            if (picker != null)
                picker.ItemsSource = PickerItemSource;
        }
    }

    public Picker? picker;

    public CustomPicker()
    {
        try
        {
            StrokeThickness = .5;
            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(7) };
            Padding = new Thickness(10);
            Stroke = Brush.Black;
            this.AppThemeBinding(
                Border.BackgroundProperty,
                Colors.White,
                Color.FromArgb("#23262A")
            );

            picker = new Picker { FontFamily = "InterMedium", FontSize = 16 }
                .Bind(
                    Picker.SelectedItemProperty,
                    nameof(CustomPickerSelectedItem),
                    BindingMode.TwoWay,
                    source: this
                )
                .Bind(
                    Picker.ItemsSourceProperty,
                    nameof(PickerItemSourceProperty),
                    BindingMode.TwoWay,
                    source: this
                )
                .AppThemeBinding(Picker.TextColorProperty, Colors.Black, Colors.White)
                .AppThemeBinding(Picker.TitleColorProperty, Colors.Black, Colors.White);

            Content = picker;
        }
        catch (Exception ex)
        {
            Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
    }
}
