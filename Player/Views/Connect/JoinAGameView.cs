namespace Player.Views.Connect;

public class JoinAGameView : VerticalStackLayout
{
    public JoinAGameView()
    {
        try
        {
            // the list of games which are hosted by other players. The background should always be of primary color of the app
            Style<Border> hostedGamesStyle = new Style<Border>(
                (
                    VisualStateManager.VisualStateGroupsProperty,
                    new VisualStateGroupList
                    {
                        new VisualStateGroup
                        {
                            Name = nameof(VisualStateManager.CommonStates),
                            States =
                            {
                                new VisualState
                                {
                                    Name = VisualStateManager.CommonStates.Selected,
                                    Setters =
                                    {
                                        new Setter
                                        {
                                            Property = Border.BackgroundProperty,
                                            Value = Colors.Transparent,
                                        },
                                    },
                                },
                                new VisualState
                                {
                                    Name = VisualStateManager.CommonStates.Normal,
                                    Setters =
                                    {
                                        new Setter
                                        {
                                            Property = Border.BackgroundProperty,
                                            Value = Colors.Transparent,
                                        },
                                    },
                                },
                            },
                        },
                    }
                )
            );

            // the list of games which are onboarded by Dome. The background should always be transparent for the top layout. Only the child border which have game image needs to be highlighted when selected.
            Style<VerticalStackLayout> onBoardedGamesStyle = new Style<VerticalStackLayout>(
                (
                    VisualStateManager.VisualStateGroupsProperty,
                    new VisualStateGroupList
                    {
                        new VisualStateGroup
                        {
                            Name = nameof(VisualStateManager.CommonStates),
                            States =
                            {
                                new VisualState
                                {
                                    Name = VisualStateManager.CommonStates.Selected,
                                    Setters =
                                    {
                                        new Setter
                                        {
                                            Property = VerticalStackLayout.BackgroundProperty,
                                            Value = Colors.Transparent,
                                        },
                                    },
                                },
                                new VisualState
                                {
                                    Name = VisualStateManager.CommonStates.Normal,
                                    Setters =
                                    {
                                        new Setter
                                        {
                                            Property = VerticalStackLayout.BackgroundProperty,
                                            Value = Colors.Transparent,
                                        },
                                    },
                                },
                            },
                        },
                    }
                )
            );

            Spacing = 10;

            Border locationBorder = new Border
            {
                StrokeThickness = 0,
                Margin = new Thickness(0, 10, 0, 0),
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(7) },
                Padding = new Thickness(18),
                Content = new Grid
                {
                    ColumnDefinitions = Columns.Define((BodyColumn.first, Auto)),
                    RowDefinitions = Rows.Define((BodyRow.first, Auto), (BodyRow.second, Auto)),
                    RowSpacing = 10,
                    Children =
                    {
                        new Regular12Label { Text = "Location" },
                        new HorizontalStackLayout
                        {
                            Spacing = 10,
                            Children =
                            {
                                new Picker { Background = Colors.Transparent }
                                    .AppThemeBinding(
                                        Picker.TextColorProperty,
                                        Color.FromArgb("#626262"),
                                        Color.FromArgb("#E0E0E0")
                                    )
                                    .Bind(Picker.ItemsSourceProperty, "VenueLocations")
                                    .Bind(Picker.SelectedItemProperty, "SelectedLocation"),
                                new Image { WidthRequest = 10, HeightRequest = 6 }.AppThemeBinding(
                                    Image.SourceProperty,
                                    "picker_arrow_black.png",
                                    "picker_arrow.png"
                                ),
                            },
                        }.Row(BodyRow.second),
                    },
                },
            }.AppThemeBinding(
                Border.BackgroundProperty,
                Colors.White,
                Color.FromArgb("#23262A")
            );

            Regular12Label chooseSportsLabel = new Regular12Label
            {
                Text = "Choose Sports",
            }.Margins(18, 10, 0, 0);

            CollectionView gamesCollectionView = new CollectionView
            {
                HeightRequest = 60,
                Background = Colors.Transparent,
                SelectionMode = SelectionMode.Single,
                // ItemSizingStrategy= ItemSizingStrategy.MeasureFirstItem,
                EmptyView = new Regular16Label
                {
                    Text = "No games found",
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                },
                ItemsLayout = new LinearItemsLayout(orientation: ItemsLayoutOrientation.Horizontal)
                {
                    ItemSpacing = 15,
                },
                ItemTemplate = new DataTemplate(() =>
                {
                    return new VerticalStackLayout
                    {
                        Style = onBoardedGamesStyle,
                        Children =
                        {
                            new Border
                            {
                                StrokeThickness = 0,
                                HeightRequest = 40,
                                WidthRequest = 40,
                                StrokeShape = new RoundRectangle
                                {
                                    CornerRadius = new CornerRadius(20),
                                },
                                Padding = 5,
                                Content = new Image { HeightRequest = 25, WidthRequest = 25 }.Bind(
                                    Image.SourceProperty,
                                    nameof(Game.GameIcon),
                                    converter: new ByteArrayToImageSourceConverter()
                                ),
                            }
                                .AppThemeBinding(Border.StrokeProperty, Brush.Black, Brush.White)
                                .Bind(
                                    Border.BackgroundProperty,
                                    nameof(Game.IsSelected),
                                    convert: (bool isSelected) =>
                                        isSelected == true
                                            ? Color.FromArgb("#EF2F50")
                                            : Colors.Transparent
                                ),
                            new Regular8Label { HorizontalOptions = LayoutOptions.Center }.Bind(
                                Regular8Label.TextProperty,
                                nameof(Game.GameName)
                            ),
                        },
                    };
                }),
            }
                .Bind(ItemsView.ItemsSourceProperty, "OnboardedGames", BindingMode.OneWay)
                .Bind(SelectableItemsView.SelectedItemProperty, "SelectedGame")
                .Margins(18, 5, 0, 5);

            Rectangle line = new Rectangle { HeightRequest = 2 }.AppThemeBinding(
                Rectangle.BackgroundProperty,
                Color.FromArgb("#626262"),
                Color.FromArgb("#D2D2D2")
            );

            CollectionView plannmedBookingsView = new CollectionView
            {
                SelectionMode = SelectionMode.Single,
                // ItemSizingStrategy= ItemSizingStrategy.MeasureFirstItem,
                EmptyView = new Regular16Label
                {
                    Text = "No planned bookings found",
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                },
                ItemsLayout = new LinearItemsLayout(orientation: ItemsLayoutOrientation.Vertical)
                {
                    ItemSpacing = 10,
                },
                ItemTemplate = new DataTemplate(() =>
                {
                    return new Border
                    {
                        Padding = new Thickness(20, 18),
                        StrokeThickness = 0,
                        StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(7) },
                        Content = new Grid
                        {
                            ColumnDefinitions = Columns.Define(
                                (BodyColumn.first, Auto),
                                (BodyColumn.second, Star),
                                (BodyColumn.third, Auto)
                            ),

                            RowDefinitions = Rows.Define(
                                (BodyRow.first, Auto),
                                (BodyRow.second, Auto),
                                (BodyRow.third, Auto),
                                (BodyRow.fourth, Auto)
                            ),

                            Children =
                            {
                                new Regular14Label { }
                                    .Bind(
                                        SemiBold12Label.TextProperty,
                                        $"{nameof(PlannedBooking.SelectedVenue)}.{nameof(PlannedBooking.SelectedVenue.FullName)}"
                                    )
                                    .Row(BodyRow.first)
                                    .Column(BodyColumn.first)
                                    .ColumnSpan(2),
                                new Regular10Label { }
                                    .Bind(Regular10Label.TextProperty, nameof(PlannedBooking.City))
                                    .Row(BodyRow.second)
                                    .Column(BodyColumn.first)
                                    .ColumnSpan(2),
                                new SemiBold14Label { HorizontalOptions = LayoutOptions.End }
                                    .Bind(
                                        SemiBold10Label.TextProperty,
                                        nameof(PlannedBooking.Timing)
                                    )
                                    .Row(BodyRow.first)
                                    .Column(BodyColumn.third),
                                //                                 new Regular10Label {HorizontalOptions=LayoutOptions.End }.Bind(Regular10Label.TextProperty,
                                //    binding1: new Binding(nameof(PlannedBooking.PlannedDate), stringFormat: "{0:dd MMM, yyyy} "),
                                //    binding2: new Binding(nameof(PlannedBooking.TimingDisplay), stringFormat: "({0})"),
                                //    convert: ((string? date, string? timingDisplay) values) => $"{values.date} {values.timingDisplay}").Row(BodyRow.second).Column(BodyColumn.third),

                                new Regular10Label { HorizontalOptions = LayoutOptions.End }
                                    .Bind(
                                        Regular10Label.TextProperty,
                                        nameof(PlannedBooking.PlannedDate),
                                        stringFormat: "{0:dd MMM, yyyy} "
                                    )
                                    .Row(BodyRow.second)
                                    .Column(BodyColumn.third),
                                new AvatarView
                                {
                                    BorderWidth = 0,
                                    Padding = 0,
                                    HeightRequest = 35,
                                    WidthRequest = 35,
                                    CornerRadius = 17.5,
                                    HorizontalOptions = LayoutOptions.Start,
                                }
                                    .Bind(
                                        AvatarView.ImageSourceProperty,
                                        $"{nameof(PlannedBooking.Host)}.{nameof(PlannedBooking.Host.ProfileImage)}",
                                        converter: new ByteArrayToImageSourceConverter(),
                                        fallbackValue: "profile_image_fallback.png",
                                        targetNullValue: "profile_image_fallback.png"
                                    )
                                    .Row(BodyRow.third)
                                    .RowSpan(2)
                                    .Column(BodyColumn.first)
                                    .Margins(0, 20, 0, 0),
                                new Regular10Label
                                {
                                    Text = "Hosted By",
                                    VerticalOptions = LayoutOptions.End,
                                    HorizontalOptions = LayoutOptions.Start,
                                }
                                    .Row(BodyRow.third)
                                    .Column(BodyColumn.second)
                                    .Margins(5, 20, 0, 0),
                                new SemiBold10Label { HorizontalOptions = LayoutOptions.Start }
                                    .Bind(
                                        SemiBold10Label.TextProperty,
                                        $"{nameof(PlannedBooking.Host)}.{nameof(PlannedBooking.Host.FullName)}"
                                    )
                                    .Row(BodyRow.fourth)
                                    .Column(BodyColumn.second)
                                    .Margins(5, 0, 0, 0),
                                new Regular10Label
                                {
                                    HorizontalOptions = LayoutOptions.End,
                                    VerticalOptions = LayoutOptions.End,
                                }
                                    .Bind(
                                        Regular10Label.TextProperty,
                                        binding1: new Binding(nameof(PlannedBooking.GameName)),
                                        binding2: new Binding(nameof(PlannedBooking.SkillLevel)),
                                        convert: ((string? gameName, string? skillLevel) values) =>
                                            $"{values.gameName} | {values.skillLevel}"
                                    )
                                    .Row(BodyRow.third)
                                    .Column(BodyColumn.third)
                                    .Margins(0, 20, 0, 0),
                                new Bold10Label { HorizontalOptions = LayoutOptions.End }
                                    .Bind(
                                        Bold10Label.TextProperty,
                                        nameof(PlannedBooking.TeamStatus)
                                    )
                                    .Row(BodyRow.fourth)
                                    .Column(BodyColumn.third),
                            },
                        },
                    }.AppThemeBinding(
                        Border.BackgroundProperty,
                        Colors.White,
                        Color.FromArgb("#23262A")
                    );
                    ;
                }),
            }
                .Bind(
                    ItemsView.ItemsSourceProperty,
                    "OthersFilteredPlannedBookings",
                    BindingMode.OneWay
                )
                .Bind(
                    SelectableItemsView.SelectionChangedCommandProperty,
                    "BindingContext.JoinHostedGameCommand",
                    source: this
                )
                .Bind(
                    SelectableItemsView.SelectedItemProperty,
                    "BindingContext.SelectedBooking",
                    source: this
                )
                .Margins(0, 10, 0, 20);

            this.Children.Add(locationBorder);
            this.Children.Add(chooseSportsLabel);
            this.Children.Add(gamesCollectionView);
            this.Children.Add(line);
            this.Children.Add(plannmedBookingsView);
        }
        catch (Exception ex)
        {
            Shell.Current.DisplayAlert("Error", ex.Message, "Okay");
        }
    }
}
