namespace Player.CustomControls.Entries
{
    public class DualImageBorderedEntry : Border
    {
        public ICommand ChangeIsPasswordCommand { get; set; }

        public static readonly BindableProperty EntryTextProperty = BindableProperty.Create(
            nameof(EntryText),
            typeof(string),
            typeof(DualImageBorderedEntry),
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
            typeof(DualImageBorderedEntry),
            string.Empty,
            BindingMode.OneTime
        );

        public string PlaceholderText
        {
            get => (string)GetValue(PlaceholderTextProperty);
            set => SetValue(PlaceholderTextProperty, value);
        }

        protected override void OnPropertyChanged(string? propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (entry != null)
            {
                if (propertyName == EntryTextProperty.PropertyName)
                {
                    entry.Text = EntryText;
                }
                else if (propertyName == PlaceholderTextProperty.PropertyName)
                {
                    entry.Placeholder = PlaceholderText;
                }
            }
        }

        CustomEntry entry;
        Image endImage;
        Image startImage;

        public DualImageBorderedEntry()
        {
            try
            {
                ChangeIsPasswordCommand = new Command(async () => await ChangeIsPassword());

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

                entry = new CustomEntry { IsPassword = true }.Bind(
                    CustomEntry.TextProperty,
                    nameof(EntryText),
                    BindingMode.TwoWay,
                    source: this
                );

                startImage = new Image { };
                startImage.SetAppTheme<FileImageSource>(
                    Image.SourceProperty,
                    "password_light_theme.png",
                    "password_dark_theme.png"
                );

                endImage = new Image { };
                endImage.SetAppTheme<FileImageSource>(
                    Image.SourceProperty,
                    "visibility_off_black_24dp.png",
                    "visibility_off_white_24dp.png"
                );
                endImage.GestureRecognizers.Add(
                    new TapGestureRecognizer { Command = ChangeIsPasswordCommand }
                );

                Content = new Grid
                {
                    Background = Colors.Transparent,
                    RowDefinitions = Rows.Define((BodyRow.first, Auto)),
                    ColumnDefinitions = Columns.Define(
                        (BodyColumn.first, Auto),
                        (BodyColumn.second, Star),
                        (BodyColumn.third, Auto)
                    ),
                    ColumnSpacing = 10,
                    Children =
                    {
                        startImage.Column(BodyColumn.first),
                        entry.Column(BodyColumn.second),
                        endImage.Column(BodyColumn.third),
                    },
                };
            }
            catch (Exception ex)
            {
                Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async Task ChangeIsPassword()
        {
            entry.IsPassword = !entry.IsPassword;

            if (
                Application.Current.RequestedTheme == AppTheme.Light
                || Application.Current.RequestedTheme == AppTheme.Unspecified
            )
            {
                endImage.Source = entry.IsPassword
                    ? "visibility_off_black_24dp.png"
                    : "visibility_black_24dp.png";
            }
            else if (Application.Current.RequestedTheme == AppTheme.Dark)
            {
                endImage.Source = entry.IsPassword
                    ? "visibility_off_white_24dp.png"
                    : "visibility_white_24dp.png";
            }
        }
    }
}
