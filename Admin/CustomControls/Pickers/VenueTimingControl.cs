namespace Admin.CustomControls.Pickers
{
    public class VenueTimingControl : Grid
    {
        public static BindableProperty TimeProperty = BindableProperty.Create(
            nameof(Time),
            typeof(DateTimeOffset),
            typeof(VenueTimingControl),
            new DateTimeOffset(),
            BindingMode.TwoWay
        );

        public DateTimeOffset Time
        {
            get => (DateTimeOffset)GetValue(TimeProperty);
            set => SetValue(TimeProperty, value);
        }

        protected override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == nameof(Time))
            {
                this.timeLabel.Text = Time.ToString("HH:mm");
            }
        }

        ImageButton timeButton;

        Regular18Label timeLabel;

        public VenueTimingControl()
        {
            timeLabel = new Regular18Label { Background = Colors.Transparent }
                .Bind(Regular18Label.TextProperty, nameof(this.Time))
                .AppThemeBinding(Regular18Label.TextColorProperty, Colors.Black, Colors.White)
                .Column(BodyColumn.first);
            timeButton = new ImageButton
            {
                Source = "dropdown.png",
                HeightRequest = 20,
                WidthRequest = 20,
            }.Column(BodyColumn.second);
            timeButton.Clicked -= TimeButton_Clicked;
            timeButton.Clicked += TimeButton_Clicked;

            Children.Add(
                new Border
                {
                    HorizontalOptions = LayoutOptions.Start,
                    WidthRequest = 105,
                    StrokeThickness = .5,
                    StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(10) },
                    Padding = new Thickness(0),
                    Content = new Grid
                    {
                        VerticalOptions = LayoutOptions.Center,
                        Padding = new Thickness(7),
                        RowDefinitions = Rows.Define((BodyRow.first, Auto)),

                        ColumnDefinitions = Columns.Define(
                            (BodyColumn.first, 60),
                            (BodyColumn.second, 22)
                        ),
                        ColumnSpacing = 7,
                        Children = { timeLabel, timeButton },
                    },
                }
                    .AppThemeBinding(Border.StrokeProperty, Brush.Black, Brush.White)
                    .AppThemeBinding(Border.BackgroundProperty, Colors.White, Colors.Black)
            );
        }

        private async void TimeButton_Clicked(object? sender, EventArgs e)
        {
            TimePopup timePopup = new TimePopup();
            IPopupResult<DateTimeOffset> result =
                await Shell.Current.ShowPopupAsync<DateTimeOffset>(
                    timePopup,
                    PopupOptions.Empty,
                    CancellationToken.None
                );
            if (result.WasDismissedByTappingOutsideOfPopup)
            {
                return;
            }
            else if (result != null)
            {
                DateTimeOffset timeSelection = result.Result;

                Time = timeSelection;
            }
        }
    }
}
