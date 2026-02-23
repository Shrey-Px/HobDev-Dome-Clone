namespace Player.Views.Connect;

/// <summary>
/// This class represents the view for the PlannedBookingsList. It is used to display the planned bookings list from which the user can select a planned booking to host (connect the planned booking with the purchased booking)
/// </summary>
public class PlannedBookingsListView : BaseView
{
    public PlannedBookingsListView(PlannedBookingsListViewModel viewModel)
        : base(viewModel)
    {
        try
        {
            base.TopGrid.Children.Add(
                new ImageButton
                {
                    HorizontalOptions = LayoutOptions.Start,
                    VerticalOptions = LayoutOptions.Start,
                }
                    .AppThemeBinding(
                        ImageButton.SourceProperty,
                        "back_button_light_theme",
                        "back_button_dark_theme"
                    )
                    .BindCommand(nameof(viewModel.NavigateBackCommand))
            );

            base.BaseContentGrid.Children.Add(
                new Grid
                {
                    RowDefinitions = Rows.Define(
                        (BodyRow.first, Auto),
                        (BodyRow.second, Auto),
                        (BodyRow.third, Auto)
                    ),

                    RowSpacing = 10,
                    Children =
                    {
                        new ExtraBold18Label { Text = "Select group to host" }
                            .Row(BodyRow.first)
                            .Margins(16, 0, 0, 0),
                        new Rectangle
                        {
                            HeightRequest = 2,
                            Background = Color.FromArgb("4E4E4E"),
                        }.Row(BodyRow.second),
                        new CollectionView
                        {
                            ItemsLayout = new LinearItemsLayout(ItemsLayoutOrientation.Vertical)
                            {
                                ItemSpacing = 10,
                            },
                            ItemTemplate = new DataTemplate(() =>
                            {
                                return new Border
                                {
                                    StrokeThickness = 0,
                                    StrokeShape = new RoundRectangle
                                    {
                                        CornerRadius = new CornerRadius(7),
                                    },
                                    Padding = new Thickness(10),
                                    Content = new Grid
                                    {
                                        RowDefinitions = Rows.Define(
                                            (BodyRow.first, Auto),
                                            (BodyRow.second, Auto)
                                        ),
                                        ColumnDefinitions = Columns.Define(
                                            (BodyColumn.first, Star),
                                            (BodyColumn.second, Auto)
                                        ),
                                        ColumnSpacing = 10,

                                        Children =
                                        {
                                            new Regular14Label { }
                                                .Bind(
                                                    Label.TextProperty,
                                                    nameof(PlannedBooking.VenueName)
                                                )
                                                .Row(BodyRow.first),
                                            new Regular14Label { }
                                                .Bind(
                                                    Label.TextProperty,
                                                    nameof(PlannedBooking.BookingInformation)
                                                )
                                                .Row(BodyRow.second),
                                            new MediumButton
                                            {
                                                Text = "Connect",
                                                HorizontalOptions = LayoutOptions.End,
                                                VerticalOptions = LayoutOptions.Center,
                                            }
                                                .BindCommand(
                                                    nameof(viewModel.ConnectCommand),
                                                    source: viewModel
                                                )
                                                .Row(BodyRow.first)
                                                .RowSpan(2)
                                                .Column(BodyColumn.second),
                                        },
                                    },
                                }.AppThemeBinding(
                                    Border.BackgroundProperty,
                                    Colors.White,
                                    Color.FromArgb("#23262A")
                                );
                            }),
                        }
                            .Bind(ItemsView.ItemsSourceProperty, nameof(viewModel.PlannedBookings))
                            .Row(BodyRow.third)
                            .Margins(16, 0, 16, 0),
                    },
                }
                    .Row(BodyRow.second)
                    .ColumnSpan(2)
            );
        }
        catch (Exception ex)
        {
            Shell.Current.DisplayAlert("Error", ex.Message, "Ok");
        }
    }
}
