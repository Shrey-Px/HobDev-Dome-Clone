namespace Player.Views.Connect;

public class JoinRequestsView : BaseView
{
    JoinRequestsViewModel viewModel;

    public JoinRequestsView(JoinRequestsViewModel viewModel)
        : base(viewModel)
    {
        try
        {
            this.viewModel = viewModel;
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
                        (BodyRow.third, Auto),
                        (BodyRow.fourth, 250),
                        (BodyRow.fifth, 250)
                    ),

                    RowSpacing = 10,

                    Children =
                    {
                        new Regular14Label { }
                            .Bind(
                                Label.TextProperty,
                                $"{nameof(viewModel.FutureBooking)}.{nameof(viewModel.FutureBooking.VenueName)}"
                            )
                            .Row(BodyRow.first)
                            .Margins(16, 0, 16, 0),
                        new Regular14Label { }
                            .Bind(
                                Label.TextProperty,
                                $"{nameof(viewModel.FutureBooking)}.{nameof(viewModel.FutureBooking.BookingInformation)}"
                            )
                            .Row(BodyRow.second)
                            .Margins(16, 0, 16, 0),
                        new Rectangle
                        {
                            HeightRequest = 2,
                            Background = Color.FromArgb("4E4E4E"),
                        }.Row(BodyRow.third),
                        new Border
                        {
                            Padding = new Thickness(15, 19),
                            StrokeShape = new RoundRectangle
                            {
                                CornerRadius = new CornerRadius(22),
                            },
                            Content = new VerticalStackLayout
                            {
                                Spacing = 10,
                                Children =
                                {
                                    new ExtraBold18Label { Text = "Approvals" },
                                    new Rectangle
                                    {
                                        HeightRequest = 2,
                                        Background = Color.FromArgb("4E4E4E"),
                                    },
                                    new CollectionView
                                    {
                                        ItemsLayout = new LinearItemsLayout(
                                            ItemsLayoutOrientation.Vertical
                                        )
                                        {
                                            ItemSpacing = 10,
                                        },
                                        EmptyView = new Regular14Label { Text = "No requests" },
                                        ItemTemplate = new DataTemplate(() =>
                                        {
                                            return new Grid
                                            {
                                                RowDefinitions = Rows.Define(
                                                    (BodyRow.first, Auto),
                                                    (BodyRow.second, Auto)
                                                ),
                                                ColumnDefinitions = Columns.Define(
                                                    (BodyColumn.first, Star),
                                                    (BodyColumn.second, Auto),
                                                    (BodyColumn.third, Auto)
                                                ),
                                                ColumnSpacing = 20,
                                                Children =
                                                {
                                                    new Medium12Label { }
                                                        .Bind(
                                                            Label.TextProperty,
                                                            $"{nameof(JoinRequest.AppliedBy)}.{nameof(JoinRequest.AppliedBy.FullName)}"
                                                        )
                                                        .Row(BodyRow.first)
                                                        .Column(BodyColumn.first),
                                                    new Regular10Label { }
                                                        .Bind(
                                                            Label.TextProperty,
                                                            nameof(JoinRequest.ApplicantMessage)
                                                        )
                                                        .Row(BodyRow.second)
                                                        .Column(BodyColumn.first),
                                                    new MediumButton
                                                    {
                                                        Text = "Remove",
                                                        FontSize = 12,
                                                        Padding = new Thickness(10, 5),
                                                        VerticalOptions = LayoutOptions.Center,
                                                    }
                                                        .BindCommand(
                                                            nameof(
                                                                viewModel.UpdateJoinRequestsCommand
                                                            ),
                                                            source: viewModel
                                                        )
                                                        .Row(BodyRow.first)
                                                        .RowSpan(2)
                                                        .Column(BodyColumn.second),
                                                },
                                            };
                                        }),
                                    }.Bind(
                                        ItemsView.ItemsSourceProperty,
                                        nameof(viewModel.AcceptedRequests)
                                    ),
                                },
                            },
                        }
                            .AppThemeBinding(
                                Border.BackgroundProperty,
                                Colors.White,
                                Color.FromArgb("#23262A")
                            )
                            .Row(BodyRow.fourth)
                            .Margins(16, 0, 16, 0),
                        new Border
                        {
                            Padding = new Thickness(15, 19),
                            StrokeShape = new RoundRectangle
                            {
                                CornerRadius = new CornerRadius(22),
                            },
                            Content = new VerticalStackLayout
                            {
                                Spacing = 10,
                                Children =
                                {
                                    new ExtraBold18Label { Text = "Pending" },
                                    new Rectangle
                                    {
                                        HeightRequest = 2,
                                        Background = Color.FromArgb("4E4E4E"),
                                    },
                                    new CollectionView
                                    {
                                        ItemsLayout = new LinearItemsLayout(
                                            ItemsLayoutOrientation.Vertical
                                        )
                                        {
                                            ItemSpacing = 10,
                                        },
                                        EmptyView = new Regular14Label { Text = "No requests" },
                                        ItemTemplate = new DataTemplate(() =>
                                        {
                                            return new Grid
                                            {
                                                RowDefinitions = Rows.Define(
                                                    (BodyRow.first, Auto),
                                                    (BodyRow.second, Auto)
                                                ),
                                                ColumnDefinitions = Columns.Define(
                                                    (BodyColumn.first, Star),
                                                    (BodyColumn.second, Auto),
                                                    (BodyColumn.third, Auto)
                                                ),
                                                ColumnSpacing = 20,
                                                Children =
                                                {
                                                    new Medium12Label { }
                                                        .Bind(
                                                            Label.TextProperty,
                                                            $"{nameof(JoinRequest.AppliedBy)}.{nameof(JoinRequest.AppliedBy.FullName)}"
                                                        )
                                                        .Row(BodyRow.first)
                                                        .Column(BodyColumn.first),
                                                    new Regular10Label { }
                                                        .Bind(
                                                            Label.TextProperty,
                                                            nameof(JoinRequest.ApplicantMessage)
                                                        )
                                                        .Row(BodyRow.second)
                                                        .Column(BodyColumn.first),
                                                    new MediumButton
                                                    {
                                                        Text = "Approve",
                                                        FontSize = 12,
                                                        Padding = new Thickness(10, 5),
                                                        VerticalOptions = LayoutOptions.Center,
                                                    }
                                                        .BindCommand(
                                                            nameof(
                                                                viewModel.UpdateJoinRequestsCommand
                                                            ),
                                                            source: viewModel
                                                        )
                                                        .Row(BodyRow.first)
                                                        .RowSpan(2)
                                                        .Column(BodyColumn.second),
                                                    new ImageButton { }
                                                        .AppThemeBinding(
                                                            ImageButton.SourceProperty,
                                                            "cancel_booking.png",
                                                            "cancel_booking_white.png"
                                                        )
                                                        .BindCommand(
                                                            nameof(viewModel.DeleteRequestCommand),
                                                            source: viewModel
                                                        )
                                                        .Row(BodyRow.first)
                                                        .RowSpan(2)
                                                        .Column(BodyColumn.third),
                                                },
                                            };
                                        }),
                                    }.Bind(
                                        ItemsView.ItemsSourceProperty,
                                        nameof(viewModel.AppliedRequests)
                                    ),
                                },
                            },
                        }
                            .AppThemeBinding(
                                Border.BackgroundProperty,
                                Colors.White,
                                Color.FromArgb("#23262A")
                            )
                            .Row(BodyRow.fifth)
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
