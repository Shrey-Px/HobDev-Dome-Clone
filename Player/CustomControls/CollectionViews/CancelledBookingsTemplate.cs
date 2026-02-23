namespace Player.CustomControls.CollectionViews
{
    public class CancelledBookingsTemplate : CollectionView
    {
        public CancelledBookingsTemplate()
        {
            ItemsLayout = new LinearItemsLayout(ItemsLayoutOrientation.Vertical)
            {
                ItemSpacing = 10,
            };
            SelectionMode = SelectionMode.Single;

            EmptyView = new Regular16Label
            {
                Text = "No Cancelled Bookings",
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
            };
            ItemTemplate = new DataTemplate(() =>
            {
                return new Border
                {
                    StrokeThickness = 0,
                    StrokeShape = new RoundRectangle { CornerRadius = 8 },
                    Content = new Grid
                    {
                        RowDefinitions = Rows.Define(
                            (BodyRow.first, Auto),
                            (BodyRow.second, Auto),
                            (BodyRow.third, Auto),
                            (BodyRow.fourth, Auto),
                            (BodyRow.fifth, Auto),
                            (BodyRow.sixth, Auto)
                        ),

                        ColumnDefinitions = Columns.Define(
                            (BodyColumn.first, Star),
                            (BodyColumn.second, Star)
                        ),

                        Padding = new Thickness(8),
                        Children =
                        {
                            new Regular14Label { }
                                .Bind(Regular14Label.TextProperty, nameof(Booking.GameName))
                                .Row(BodyRow.first)
                                .Column(BodyColumn.first)
                                .ColumnSpan(3),
                            new SemiBold12Label { }
                                .Bind(
                                    SemiBold12Label.TextProperty,
                                    $"{nameof(Booking.Venue)}.{nameof(Booking.Venue.FullName)}"
                                )
                                .Row(BodyRow.fifth)
                                .Column(BodyColumn.first)
                                .ColumnSpan(3),
                            new Medium16Label
                            {
                                HorizontalOptions = LayoutOptions.End,
                                VerticalOptions = LayoutOptions.Center,
                            }
                                .AppThemeBinding(
                                    Medium10Label.TextColorProperty,
                                    Color.FromArgb("#626262"),
                                    Color.FromArgb("#E0E0E0")
                                )
                                .Bind(
                                    Medium16Label.TextProperty,
                                    nameof(Booking.TotalPrice),
                                    stringFormat: "{0:C}"
                                )
                                .Row(BodyRow.fourth)
                                .RowSpan(2)
                                .Column(BodyColumn.third),
                            new SemiBold12Label { }
                                .Bind(SemiBold10Label.TextProperty, nameof(Booking.BookingInfo))
                                .Row(BodyRow.sixth)
                                .Column(BodyColumn.first)
                                .ColumnSpan(3)
                                .Margins(0, 0, 0, 20),
                            new HorizontalStackLayout
                            {
                                Spacing = 10,
                                Children =
                                {
                                    new Medium12Label { VerticalOptions = LayoutOptions.Center }
                                        .Bind(
                                            Medium12Label.TextProperty,
                                            nameof(Booking.TeamStatus)
                                        )
                                        .Bind(
                                            Medium12Label.IsVisibleProperty,
                                            nameof(Booking.JoinedPlayers),
                                            convert: (IList<VenueUser>? joinedPlayers) =>
                                                joinedPlayers?.Count > 0 ? true : false
                                        ),
                                    new CollectionView
                                    {
                                        HeightRequest = 40,
                                        HorizontalScrollBarVisibility = ScrollBarVisibility.Always,
                                        WidthRequest = 150,
                                        ItemsLayout = new LinearItemsLayout(
                                            ItemsLayoutOrientation.Horizontal
                                        )
                                        {
                                            ItemSpacing = 0,
                                        },
                                        // the image is misplaced in iOS
                                        Header = new AvatarView
                                        {
                                            HeightRequest = 35,
                                            WidthRequest = 35,
                                            CornerRadius = 17,
                                            Padding = 0,
                                        }
                                            .Bind(
                                                AvatarView.ImageSourceProperty,
                                                "PlayerImage",
                                                converter: new ByteArrayToImageSourceConverter(),
                                                fallbackValue: "profile_image_fallback",
                                                targetNullValue: "profile_image_fallback"
                                            )
                                            .Bind(
                                                AvatarView.IsVisibleProperty,
                                                nameof(Booking.JoinedPlayers),
                                                convert: (IList<VenueUser>? joinedPlayers) =>
                                                    joinedPlayers?.Count > 0 ? true : false
                                            ),

                                        ItemTemplate = new DataTemplate(() =>
                                        {
                                            return new AvatarView
                                            {
                                                HeightRequest = 35,
                                                WidthRequest = 35,
                                                CornerRadius = 17,
                                                Padding = 0,
                                            }.Bind(
                                                AvatarView.ImageSourceProperty,
                                                nameof(VenueUser.ProfileImage),
                                                converter: new ByteArrayToImageSourceConverter(),
                                                fallbackValue: "profile_image_fallback",
                                                targetNullValue: "profile_image_fallback"
                                            );
                                        }),
                                    }.Bind(
                                        ItemsView.ItemsSourceProperty,
                                        nameof(Booking.JoinedPlayers)
                                    ),
                                },
                            }
                                .Row(BodyRow.third)
                                .Column(BodyColumn.first)
                                .ColumnSpan(3)
                                .Margins(0, 0, 0, 10),
                            new Medium12Label { }
                                .Bind(Medium12Label.TextProperty, nameof(Booking.PlayerName))
                                .AppThemeBinding(
                                    Medium10Label.TextColorProperty,
                                    Color.FromArgb("#626262"),
                                    Color.FromArgb("#E0E0E0")
                                )
                                .Row(BodyRow.fourth)
                                .Column(BodyColumn.first)
                                .ColumnSpan(3),
                            new MediumButton
                            {
                                Padding = new Thickness(10, 5),
                                FontFamily = "InterRegular",
                                FontSize = 8,
                                HorizontalOptions = LayoutOptions.End,
                            }
                                .Bind(MediumButton.IsVisibleProperty, nameof(Booking.IsHosted))
                                .Bind(
                                    MediumButton.TextProperty,
                                    nameof(Booking.IsJoined),
                                    convert: (bool joined) => joined == true ? "Joined" : "Hosted"
                                )
                                .Row(BodyRow.second)
                                .Column(BodyColumn.third),
                        },
                    },
                }.AppThemeBinding(
                    Border.BackgroundProperty,
                    Colors.White,
                    Color.FromArgb("#23262A")
                );
            });
            this.Bind(
                ItemsView.ItemsSourceProperty,
                "BindingContext.CancelledBookings",
                source: this
            );
            this.Bind(
                SelectableItemsView.SelectionChangedCommandProperty,
                "BindingContext.SelectionChangedCommand",
                source: this
            );
            this.Bind(
                SelectableItemsView.SelectedItemProperty,
                "BindingContext.SelectedBooking",
                source: this
            );
        }
    }
}
