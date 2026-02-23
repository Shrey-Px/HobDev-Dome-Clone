using Microsoft.Maui.Controls;
using Syncfusion.Maui.Toolkit.BottomSheet;

namespace Player.CustomControls.CollectionViews
{
    public class UpcomingBookingsTemplate : Grid
    {
        public UpcomingBookingsTemplate()
        {
            CollectionView collection = new CollectionView
            {
                SelectionMode = SelectionMode.Single,
                EmptyView = new Regular16Label
                {
                    Text = "No Upcoming Bookings",
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                },
                ItemsLayout = new LinearItemsLayout(ItemsLayoutOrientation.Vertical)
                {
                    ItemSpacing = 10,
                },
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
                                (BodyRow.sixth, Auto),
                                (BodyRow.seventh, Auto)
                            ),

                            ColumnDefinitions = Columns.Define(
                                (BodyColumn.first, Star),
                                (BodyColumn.second, Auto),
                                (BodyColumn.third, Star)
                            ),

                            Padding = new Thickness(8),
                            Children =
                            {
                                new Regular14Label { }
                                    .Bind(Regular14Label.TextProperty, nameof(Booking.GameName))
                                    .Row(BodyRow.first)
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
                                        convert: (bool joined) =>
                                            joined == true ? "Joined" : "Hosted"
                                    )
                                    .Row(BodyRow.first)
                                    .Column(BodyColumn.third),
                                // new HorizontalStackLayout
                                // {
                                //     Children =
                                //     {
                                //         //  the team status in Booking is JoinedPlayers.Count+1/{NumberOfPlayersRequiredForHosting (Players requirement selected while creating the planned booking)
                                //         new Medium12Label
                                //         {
                                //             VerticalOptions = LayoutOptions.Center,
                                //         }.Bind(
                                //             Medium12Label.TextProperty,
                                //             nameof(Booking.TeamStatus)
                                //         ),
                                //         new AvatarView
                                //         {
                                //             HeightRequest = 35,
                                //             WidthRequest = 35,
                                //             CornerRadius = 17,
                                //             Padding = 0,
                                //         }
                                //             .Bind(
                                //                 AvatarView.ImageSourceProperty,
                                //                 "PlayerImage",
                                //                 source: BindingContext,
                                //                 converter: new ByteArrayToImageSourceConverter(),
                                //                 targetNullValue: "profile_image_fallback"
                                //             )
                                //             .Margins(10, 0, 0, 0),
                                //         new CollectionView
                                //         {
                                //             HeightRequest = 40,
                                //             HorizontalScrollBarVisibility =
                                //                 ScrollBarVisibility.Always,
                                //             WidthRequest = 150,
                                //             ItemsLayout = new LinearItemsLayout(
                                //                 ItemsLayoutOrientation.Horizontal
                                //             )
                                //             {
                                //                 ItemSpacing = 0,
                                //             },

                                //             ItemTemplate = new DataTemplate(() =>
                                //             {
                                //                 return new AvatarView
                                //                 {
                                //                     HeightRequest = 35,
                                //                     WidthRequest = 35,
                                //                     CornerRadius = 17,
                                //                     Padding = 0,
                                //                 }.Bind(
                                //                     AvatarView.ImageSourceProperty,
                                //                     nameof(VenueUser.ProfileImage),
                                //                     converter: new ByteArrayToImageSourceConverter(),
                                //                     targetNullValue: "profile_image_fallback"
                                //                 );
                                //             }),
                                //         }.Bind(
                                //             ItemsView.ItemsSourceProperty,
                                //             nameof(Booking.JoinedPlayers)
                                //         ),
                                //     },
                                // }
                                //     .Row(BodyRow.second)
                                //     .Column(BodyColumn.first)
                                //     .ColumnSpan(3)
                                //     .Margins(0, 0, 0, 10),
                                // new Medium12Label { }
                                //     .Bind(Medium12Label.TextProperty, nameof(Booking.PlayerName))
                                //     .AppThemeBinding(
                                //         Medium12Label.TextColorProperty,
                                //         Color.FromArgb("#626262"),
                                //         Color.FromArgb("#E0E0E0")
                                //     )
                                //     .Row(BodyRow.third)
                                //     .Column(BodyColumn.first)
                                //     .ColumnSpan(3),
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
                                    .Bind(SemiBold12Label.TextProperty, nameof(Booking.VenueName))
                                    .Row(BodyRow.fourth)
                                    .Column(BodyColumn.first)
                                    .ColumnSpan(3),
                                new SemiBold12Label { }
                                    .Bind(SemiBold10Label.TextProperty, nameof(Booking.BookingInfo))
                                    .Row(BodyRow.fifth)
                                    .Column(BodyColumn.first)
                                    .ColumnSpan(3)
                                    .Margins(0, 0, 0, 20),
                                new Rectangle
                                {
                                    HeightRequest = 2,
                                    Background = Color.FromArgb("#D4D4D4"),
                                }
                                    .Row(BodyRow.sixth)
                                    .Column(BodyColumn.first)
                                    .ColumnSpan(3),
                                new Rectangle
                                {
                                    WidthRequest = 2,
                                    Background = Color.FromArgb("#D4D4D4"),
                                }
                                 .Bind(Rectangle.IsVisibleProperty, nameof(Booking.IsJoined))
                                    .Row(BodyRow.sixth)
                                    .RowSpan(2)
                                    .Column(BodyColumn.second),

                                 new MediumButton
                                {
                                    Text = "Cancel Game",
                                    FontFamily = "InterRegular",
                                    FontSize = 14,
                                    TextColor = Color.FromArgb("EF2F50"),
                                    Background = Colors.Transparent,
                                }
                                    .Row(BodyRow.seventh)
                                    .Column(BodyColumn.first)
                                    .ColumnSpan(3)
                                    .BindCommand("BindingContext.CancelBookingCommand", source: this)
                                    .Bind(
                                        MediumButton.IsVisibleProperty,
                                        nameof(Booking.NotHostedbyLoggedInUser)
                                    ),
                                // new MediumButton
                                // {
                                //     Text = "Share Game",
                                //     FontFamily = "InterRegular",
                                //     FontSize = 14,
                                //     TextColor = Color.FromArgb("EF2F50"),
                                //     Background = Colors.Transparent,
                                // }
                                //     .Row(BodyRow.seventh)
                                //     .Column(BodyColumn.first)
                                //     .ColumnSpan(3)
                                //     .BindCommand("BindingContext.ShareGameCommand", source: this)
                                //     .Bind(
                                //         MediumButton.IsVisibleProperty,
                                //         nameof(Booking.NotHostedbyLoggedInUser)
                                //     ),
                                new MediumButton
                                {
                                    Text = "Chat",
                                    FontFamily = "InterRegular",
                                    FontSize = 14,
                                    TextColor = Color.FromArgb("EF2F50"),
                                    Background = Colors.Transparent,
                                }
                                    .Row(BodyRow.seventh)
                                    .Column(BodyColumn.first)
                                    .ColumnSpan(3)
                                    .BindCommand(
                                        "ChatFromPurchasedBookingCommand",
                                        source: BindingContext
                                    )
                                    .Bind(MediumButton.IsVisibleProperty, nameof(Booking.IsHosted))
                                    ,
                                // new MediumButton { Text = "Share Booking",FontFamily="InterRegular", FontSize = 14, TextColor=Color.FromArgb("EF2F50"), Background =Colors.Transparent  }.Row(BodyRow.eighth).Column(BodyColumn.first).BindCommand("BindingContext.ShareHostedGameLinkCommand", source: this).Bind(MediumButton.IsVisibleProperty, nameof(Booking.IsHostedbyLoggedInUser)),

                                // new MediumButton
                                // {
                                //     Text = "Cancel Booking",
                                //     FontFamily = "InterRegular",
                                //     FontSize = 14,
                                //     Background = Colors.Transparent,
                                //     HorizontalOptions = LayoutOptions.Center,
                                // }
                                //     .AppThemeBinding(
                                //         MediumButton.TextColorProperty,
                                //         Color.FromArgb("#626262"),
                                //         Color.FromArgb("#E0E0E0")
                                //     )
                                //     .Row(BodyRow.seventh)
                                //     .Column(BodyColumn.third)
                                //     .BindCommand(
                                //         "BindingContext.CancelBookingCommand",
                                //         source: this
                                //     )
                                //     .Bind(
                                //         MediumButton.IsVisibleProperty,
                                //         nameof(Booking.IsJoined),
                                //         convert: (bool joined) => joined == true ? false : true
                                //     )
                                //     .Margins(5, 0, 0, 0),
                                new MediumButton
                                {
                                    Text = "Quit Game",
                                    FontFamily = "InterRegular",
                                    FontSize = 14,
                                    Background = Colors.Transparent,
                                    HorizontalOptions = LayoutOptions.Center,
                                }
                                    .AppThemeBinding(
                                        MediumButton.TextColorProperty,
                                        Colors.Black,
                                        Colors.White
                                    )
                                    .Row(BodyRow.seventh)
                                    .Column(BodyColumn.third)
                                    .BindCommand("BindingContext.QuitGameCommand", source: this)
                                    .Bind(MediumButton.IsVisibleProperty, nameof(Booking.IsJoined)),
                            },
                        },
                    }.AppThemeBinding(
                        Border.BackgroundProperty,
                        Colors.White,
                        Color.FromArgb("#23262A")
                    );
                }),
            }
                .Bind(ItemsView.ItemsSourceProperty, "UpComingBookings")
                .Bind(
                    SelectableItemsView.SelectionChangedCommandProperty,
                    "SelectionChangedCommand"
                )
                .Bind(SelectableItemsView.SelectedItemProperty, "SelectedBooking");

            //  SfBottomSheet sheet=  new SfBottomSheet
            //     {

            //        // HalfExpandedRatio = 0.35,
            //         AllowedState= BottomSheetAllowedState.HalfExpanded,

            //         Content = new Grid
            //         {
            //             RowDefinitions = Rows.Define(
            //                 (BodyRow.first, Auto),
            //                 (BodyRow.second, Star)
            //             ),
            //             ColumnDefinitions = Columns.Define(
            //                 (BodyColumn.first, Star),
            //                 (BodyColumn.second, Star)
            //             ),
            //             Children =
            //             {
            //                 new Regular16Label { Text = "Share", HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center }.Row(BodyRow.first).ColumnSpan(2),
            //                 new Grid
            //                 {
            //                     new Regular14Label { Text = "Share your booking with friends" }.Row(BodyRow.second).Column(BodyColumn.first),
            //                     new ImageButton { Source = "share" }.Row(BodyRow.second).Column(BodyColumn.second)
            //                 }.BindTapGesture("ShareGameCommand")
            //             }
            //         }

            //     }.Bind(SfBottomSheet.IsOpenProperty, "BindingContext.IsShareBottomSheetOpen", source: this);

            Children.Add(collection);
            //Children.Add(sheet);
        }
    }
}
