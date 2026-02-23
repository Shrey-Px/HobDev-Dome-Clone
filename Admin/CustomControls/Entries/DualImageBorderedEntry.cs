namespace Admin.CustomControls.Entries
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

        readonly Entry entry;
        readonly Image endImage;
        readonly Image startImage;

        public DualImageBorderedEntry()
        {
            ChangeIsPasswordCommand = new Command(async () => await ChangeIsPassword());

            StrokeThickness = .5;
            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(30) };
            Padding = new Thickness(0);
            Background = Colors.Transparent;
            this.AppThemeBinding(Border.StrokeProperty, Brush.Black, Brush.White);
            this.AppThemeBinding(Border.BackgroundProperty, Colors.White, Colors.Black);

            entry = new Entry
            {
                IsPassword = true,
                FontFamily = "InterRegular",
                FontSize = 25,
                HeightRequest = 70,
                Background = Colors.Transparent,
            }
                .Bind(Entry.TextProperty, nameof(EntryText), BindingMode.TwoWay, source: this)
                .AppThemeBinding(Entry.TextColorProperty, Colors.Black, Colors.White)
                .AppThemeBinding(Entry.PlaceholderColorProperty, Colors.Black, Colors.White);

            startImage = new Image
            {
                Margin = new Thickness(5, 5, 0, 5),
                Background = Colors.Transparent,
            };
            startImage.SetAppTheme<FileImageSource>(
                Image.SourceProperty,
                "password_black_24dp.png",
                "password_white_24dp.png"
            );

            endImage = new Image
            {
                Margin = new Thickness(0, 5, 5, 5),
                Background = Colors.Transparent,
            };
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

                Children =
                {
                    startImage.Column(BodyColumn.first),
                    entry.Column(BodyColumn.second),
                    endImage.Column(BodyColumn.third),
                },
            };
        }

        private async Task ChangeIsPassword()
        {
            entry.IsPassword = !entry.IsPassword;

            if (
                Application.Current?.RequestedTheme == AppTheme.Light
                || Application.Current?.RequestedTheme == AppTheme.Unspecified
            )
            {
                endImage.Source = entry.IsPassword
                    ? "visibility_off_black_24dp.png"
                    : "visibility_black_24dp.png";
            }
            else if (Application.Current?.RequestedTheme == AppTheme.Dark)
            {
                endImage.Source = entry.IsPassword
                    ? "visibility_off_white_24dp.png"
                    : "visibility_white_24dp.png";
            }
        }
    }
}
