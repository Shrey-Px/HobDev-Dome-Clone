using System;

namespace Player.CustomControls.CollectionViews;

public class PlannedBookingsTemplate : CollectionView
{
    public PlannedBookingsTemplate()
    {
        SelectionMode = SelectionMode.Single;
        EmptyView = new Regular16Label
        {
            Text = "No planned Bookings",
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
        };
        ItemsLayout = new LinearItemsLayout(ItemsLayoutOrientation.Vertical) { ItemSpacing = 10 };
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
                            .Bind(
                                Regular14Label.TextProperty,
                                binding1: new Binding(nameof(PlannedBooking.GameName)),
                                binding2: new Binding(nameof(PlannedBooking.SkillLevel)),
                                convert: ((string? gameName, string? skillLevel) values) =>
                                    $"{values.gameName} | {values.skillLevel}"
                            )
                            .Row(BodyRow.first)
                            .Column(BodyColumn.first)
                            .ColumnSpan(3),
                        new ImageButton
                        {
                            HeightRequest = 35,
                            WidthRequest = 35,
                            HorizontalOptions = LayoutOptions.End,
                            VerticalOptions = LayoutOptions.Center,
                        }
                            .Bind(
                                ImageButton.IsVisibleProperty,
                                nameof(PlannedBooking.IsApplied),
                                convert: (bool isApplied) => isApplied == true ? false : true
                            )
                            .BindCommand("BindingContext.ReviewRequestsCommand", source: this)
                            .AppThemeBinding(
                                ImageButton.SourceProperty,
                                "review_requests_black.png",
                                "review_requests_white.png"
                            )
                            .Row(BodyRow.first)
                            .Column(BodyColumn.third)
                            .Margins(0, 4, 12, 0),
                        new AvatarView
                        {
                            HeightRequest = 20,
                            WidthRequest = 20,
                            CornerRadius = 17.5,
                            Padding = 0,
                            Background = Color.FromArgb("#EF2F50"),
                            TextColor = Colors.White,
                            HorizontalOptions = LayoutOptions.End,
                            BorderWidth = 0,
                        }
                            .Bind(
                                AvatarView.IsVisibleProperty,
                                nameof(PlannedBooking.IsApplied),
                                convert: (bool isApplied) => isApplied == true ? false : true
                            )
                            .Bind(AvatarView.TextProperty, nameof(PlannedBooking.PendingRequests))
                            .Row(BodyRow.first)
                            .Column(BodyColumn.third)
                            .Margins(0, 0, 0, 0),
                        new MediumButton
                        {
                            Padding = new Thickness(10, 3),
                            FontFamily = "InterRegular",
                            FontSize = 10,
                            HorizontalOptions = LayoutOptions.End,
                            VerticalOptions = LayoutOptions.End,
                            CornerRadius = 5,
                        }
                            .Bind(
                                MediumButton.TextProperty,
                                nameof(PlannedBooking.IsApplied),
                                convert: (bool applied) => applied == true ? "Applied" : "Planned"
                            )
                            .Row(BodyRow.second)
                            .Column(BodyColumn.third),
                        // team status in the planned booking is the accepted (approved requests + 1)/numberOfplayers(Players requirement selected while creating the planned booking)
                        new Medium12Label
                        {
                            HorizontalOptions = LayoutOptions.End,
                            VerticalOptions = LayoutOptions.Start,
                        }
                            .Bind(Medium12Label.TextProperty, nameof(PlannedBooking.TeamStatus))
                            .Row(BodyRow.third)
                            .Column(BodyColumn.third),
                        // display the images of the joined players
                        new HorizontalStackLayout
                        {
                            Children =
                            {
                                new AvatarView
                                {
                                    HeightRequest = 35,
                                    WidthRequest = 35,
                                    CornerRadius = 17.5,
                                    Padding = 0,
                                }
                                    .Bind(
                                        AvatarView.ImageSourceProperty,
                                        "PlayerImage",
                                        source: BindingContext,
                                        converter: new ByteArrayToImageSourceConverter(),
                                        targetNullValue: "profile_image_fallback.png"
                                    )
                                    .Margins(10, 0, 0, 0),
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

                                    ItemTemplate = new DataTemplate(() =>
                                    {
                                        return new AvatarView
                                        {
                                            HeightRequest = 35,
                                            WidthRequest = 35,
                                            CornerRadius = 17.5,
                                            Padding = 0,
                                        }.Bind(
                                            AvatarView.ImageSourceProperty,
                                            nameof(VenueUser.ProfileImage),
                                            converter: new ByteArrayToImageSourceConverter(),
                                            targetNullValue: "profile_image_fallback.png"
                                        );
                                    }),
                                }.Bind(
                                    ItemsView.ItemsSourceProperty,
                                    $"{nameof(PlannedBooking.ConnectedBooking)}.{nameof(PlannedBooking.ConnectedBooking.JoinedPlayers)}"
                                ),
                            },
                        }
                            .Row(BodyRow.second)
                            .Column(BodyColumn.first)
                            .ColumnSpan(3)
                            .Margins(0, 0, 0, 10),
                        new Medium12Label { }
                            .Bind(
                                Medium12Label.TextProperty,
                                $"{nameof(PlannedBooking.Host)}.{nameof(PlannedBooking.Host.FullName)}"
                            )
                            .Row(BodyRow.third)
                            .Column(BodyColumn.first)
                            .ColumnSpan(3)
                            .Margins(0, 20, 0, 0),
                        new SemiBold12Label { }
                            .Bind(
                                SemiBold12Label.TextProperty,
                                binding1: new Binding(
                                    $"{nameof(PlannedBooking.SelectedVenue)}.{nameof(PlannedBooking.SelectedVenue.FullName)}"
                                ),
                                binding2: new Binding(nameof(PlannedBooking.City)),
                                convert: ((string? venueName, string? cityName) values) =>
                                    $"{values.venueName} , {values.cityName}"
                            )
                            .Row(BodyRow.fourth)
                            .Column(BodyColumn.first)
                            .ColumnSpan(3),
                        new HorizontalStackLayout
                        {
                            Children =
                            {
                                new SemiBold12Label { }.Bind(
                                    SemiBold10Label.TextProperty,
                                    nameof(PlannedBooking.PlannedDate),
                                    stringFormat: "{0:dd MMM}, "
                                ),
                                //                                          new SemiBold12Label {  }.Bind(SemiBold12Label.TextProperty,
                                //    binding1: new Binding(nameof(PlannedBooking.Timing)),
                                //    binding2: new Binding(nameof(PlannedBooking.TimingDisplay)),
                                //    convert: ((string? timing, string? timingDisplay) values) => $"{values.timing} | {values.timingDisplay}"),

                                new SemiBold12Label { }.Bind(
                                    SemiBold12Label.TextProperty,
                                    nameof(PlannedBooking.Timing)
                                ),
                            },
                        }
                            .Row(BodyRow.fifth)
                            .Column(BodyColumn.first)
                            .ColumnSpan(3)
                            .Margins(0, 0, 0, 20),
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
                            .BindCommand("ChatFromPlannedBookingCommand", source: BindingContext),
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
                            .Row(BodyRow.sixth)
                            .RowSpan(2)
                            .Column(BodyColumn.second),
                        new MediumButton
                        {
                            Text = "Cancel Hosting",
                            FontFamily = "InterRegular",
                            FontSize = 14,
                            Background = Colors.Transparent,
                        }
                            .Bind(
                                MediumButton.IsVisibleProperty,
                                nameof(PlannedBooking.IsApplied),
                                convert: (bool isApplied) => isApplied == true ? false : true
                            )
                            .AppThemeBinding(
                                MediumButton.TextColorProperty,
                                Color.FromArgb("#626262"),
                                Color.FromArgb("#E0E0E0")
                            )
                            .Row(BodyRow.seventh)
                            .Column(BodyColumn.third)
                            .BindCommand("CancelPlannedBookingCommand", source: BindingContext)
                            .Margins(5, 0, 0, 0),
                        new MediumButton
                        {
                            Text = "Quit Game",
                            FontFamily = "InterRegular",
                            FontSize = 14,
                            Background = Colors.Transparent,
                        }
                            .Bind(MediumButton.IsVisibleProperty, nameof(PlannedBooking.IsApplied))
                            .AppThemeBinding(
                                MediumButton.TextColorProperty,
                                Color.FromArgb("#626262"),
                                Color.FromArgb("#E0E0E0")
                            )
                            .Row(BodyRow.seventh)
                            .Column(BodyColumn.third)
                            .BindCommand("QuitPlannedBookingCommand", source: BindingContext)
                            .Margins(5, 0, 0, 0),
                    },
                },
            }.AppThemeBinding(
                Border.BackgroundProperty,
                Colors.White,
                Color.FromArgb("#23262A")
            );
        });

        this.Bind(ItemsView.ItemsSourceProperty, "PlannedBookings");
        this.Bind(SelectableItemsView.SelectionChangedCommandProperty, "SelectionChangedCommand");
        this.Bind(SelectableItemsView.SelectedItemProperty, "SelectedBooking");
    }
}
