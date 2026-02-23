namespace Admin.CustomControls.Entries
{
    public class BorderedEntry : Border
    {
        public static readonly BindableProperty EntryTextProperty = BindableProperty.Create(
            nameof(EntryText),
            typeof(string),
            typeof(BorderedEntry),
            string.Empty,
            BindingMode.TwoWay,
            propertyChanged: EntryTextPropertyChanged
        );

        private static void EntryTextPropertyChanged(
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
            propertyChanged: PlaceholderTextPropertyChanged
        );

        private static void PlaceholderTextPropertyChanged(
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

        public Entry entry = new Entry
        {
            Background = Colors.Transparent,
            FontFamily = "InterRegular",
            FontSize = 20,
            VerticalOptions = LayoutOptions.Fill,
            HorizontalOptions = LayoutOptions.Fill,
        };

        public BorderedEntry()
        {
            StrokeThickness = .5;
            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(30) };
            Padding = new Thickness(0);

            this.AppThemeBinding(Border.StrokeProperty, Brush.Black, Brush.White);
            this.AppThemeBinding(Border.BackgroundProperty, Colors.White, Colors.Black);

            HorizontalOptions = LayoutOptions.Start;
            entry
                .Bind(Entry.TextProperty, nameof(EntryText), BindingMode.TwoWay, source: this)
                .AppThemeBinding(Entry.TextColorProperty, Colors.Black, Colors.White)
                .AppThemeBinding(Entry.PlaceholderColorProperty, Colors.Black, Colors.White);

            Content = entry;
        }
    }
}
