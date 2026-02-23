using Syncfusion.Maui.Toolkit.BottomSheet;

namespace Player.Views;

public class MyBookingsView : BaseView
{
    public MyBookingsView(MyBookingsViewModel viewModel)
        : base(viewModel)
    {
        try
        {
            Resources.Add(
                new Style<UnderlinedTabItem>(
                    (UnderlinedTabItem.FontFamilyProperty, "InterExtraBold"),
                    (UnderlinedTabItem.UnderlineHeightProperty, 6),
                    (UnderlinedTabItem.LabelSizeProperty, 12),
                    (UnderlinedTabItem.SelectedTabColorProperty, Color.FromArgb("#EF2F50"))
                ).AddAppThemeBinding(
                    UnderlinedTabItem.UnselectedLabelColorProperty,
                    Color.FromArgb("7D7D7D"),
                    Color.FromArgb("#E0E0E0")
                )
            );

            CollectionView plannedBookingControl = new PlannedBookingsTemplate();
            Grid upComingBookingsControl = new UpcomingBookingsTemplate();
            CollectionView pastBookingsControl = new PastBookingsTemplate();
            CollectionView cancelledBookingsControl = new CancelledBookingsTemplate();

            ViewSwitcher switcher = new ViewSwitcher
            {
                new DelayedView { View = plannedBookingControl },
                new DelayedView { View = upComingBookingsControl },
                new DelayedView { View = pastBookingsControl },
                new DelayedView { View = cancelledBookingsControl },
            };

            TabHostView tabHostView = new TabHostView
            {
                HeightRequest = 50,
                Tabs =
                {
                    new UnderlinedTabItem { Label = "CONNECT" },
                    new UnderlinedTabItem { Label = "UPCOMING" },
                    new UnderlinedTabItem { Label = "PAST" },
                    new UnderlinedTabItem { Label = "CANCELLED" },
                },
            }.Bind(
                TabHostView.SelectedIndexProperty,
                path: "SelectedIndex",
                BindingMode.TwoWay,
                source: switcher
            );

            base.TopGrid.Children.Add(
                new ImageButton
                {
                    HorizontalOptions = LayoutOptions.Start,
                    VerticalOptions = LayoutOptions.Start,
                }.AppThemeBinding(ImageButton.SourceProperty, "dome_light_theme", "dome_dark_theme")
            );

            base.BaseContentGrid.Children.Add(
                new Grid
                {
                    RowDefinitions = Rows.Define((BodyRow.first, Auto), (BodyRow.second, Star)),
                    Padding = new Thickness(16, 0, 16, 0),
                    RowSpacing = 21,
                    Children =
                    {
                        new Border
                        {
                            StrokeThickness = 0,
                            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(7) },
                            Content = new VerticalStackLayout
                            {
                                Padding = new Thickness(18, 14, 18, 0),
                                Spacing = 0,
                                Children =
                                {
                                    new ExtraBold28Label
                                    {
                                        TextColor = Color.FromArgb("#AEAEAE"),
                                    }.Bind(
                                        Bold28Label.TextProperty,
                                        nameof(viewModel.UserName),
                                        stringFormat: "Hey {0}"
                                    ),
                                    new Regular28Label { Text = "View Your Bookings!" },
                                    tabHostView.Margins(0, 14, 0, 0),
                                },
                            },
                        }.AppThemeBinding(
                            Border.BackgroundProperty,
                            Colors.White,
                            Color.FromArgb("#23262A")
                        ),
                        switcher.Row(BodyRow.second),
                    },
                }
                    .Row(BodyRow.second)
                    .ColumnSpan(2)
            );

            switcher.SelectedIndex = 0;

            Loaded += async (s, e) =>
            {
                await viewModel.LoadData();
            };
        }
        catch (Exception ex)
        {
            Shell.Current.DisplayAlert("Error", ex.Message, "Ok");
        }
    }
}
