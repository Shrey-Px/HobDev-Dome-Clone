namespace Player.Views.Connect;

public class PlanBookingView : Grid
{
    public PlanBookingView()
    {
        try
        {
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

            Style<Border> borderStyle = new Style<Border>(
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
                                            Value = Color.FromArgb("#EF2F50"),
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
                                            Value = Color.FromArgb("#EDEDED"),
                                        },
                                    },
                                },
                            },
                        },
                    }
                )
            );

            RowDefinitions = Rows.Define(
                (BodyRow.first, Auto),
                (BodyRow.second, Auto),
                (BodyRow.third, Auto),
                (BodyRow.fourth, Auto),
                (BodyRow.fifth, Auto),
                (BodyRow.sixth, Auto),
                (BodyRow.seventh, Auto),
                (BodyRow.eighth, Auto),
                (BodyRow.ninth, Auto),
                (BodyRow.tenth, Auto),
                (BodyRow.eleventh, Auto)
            );

            Border topImage = new Border
            {
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(7) },
                StrokeThickness = 0,
                Padding = new Thickness(10),
                Background = Color.FromArgb("#EF2F50"),
                Content = new Grid
                {
                    ColumnDefinitions = Columns.Define(
                        (BodyColumn.first, Stars(2)),
                        (BodyColumn.second, Stars(1))
                    ),
                    ColumnSpacing = 10,
                    Children =
                    {
                        new Regular12Label
                        {
                            Text =
                                "Host your favorite game and connect with players of similar skill levels for an enjoyable experience together.",
                            TextColor = Colors.White,
                        }
                            .Column(BodyColumn.first)
                            .Margins(0, 0, 0, 0),
                        new Image { Source = "hostgame_icon.png" }.Column(BodyColumn.second),
                    },
                },
            }
                .Margins(0, 20, 0, 0)
                .Row(BodyRow.first);

            Regular12Label hostLabel = new Regular12Label { Text = "Choose Sports" }
                .Margins(10, 15, 0, 0)
                .Row(BodyRow.second);

            CollectionView sportsCollection = new CollectionView
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
                .Margins(18, 5, 0, 10)
                .Row(BodyRow.third);

            Rectangle line = new Rectangle { HeightRequest = 2 }
                .AppThemeBinding(
                    Rectangle.BackgroundProperty,
                    Color.FromArgb("#626262"),
                    Color.FromArgb("#D2D2D2")
                )
                .Row(BodyRow.fourth);

            CollectionView dateGrid = new CollectionView
            {
                HeightRequest = 70,
                SelectionMode = SelectionMode.Single,
                ItemsLayout = new LinearItemsLayout(ItemsLayoutOrientation.Horizontal)
                {
                    ItemSpacing = 5,
                },
                ItemTemplate = new DataTemplate(() =>
                {
                    return new Border
                    {
                        Padding = 5,
                        Style = borderStyle,
                        StrokeThickness = 0,
                        StrokeShape = new RoundRectangle { CornerRadius = 10 },
                        Content = new VerticalStackLayout
                        {
                            Spacing = 1,
                            Margin = new Thickness(4),
                            Children =
                            {
                                new Medium10Label { }
                                    .Bind(
                                        Label.TextProperty,
                                        nameof(VenueDate.Date),
                                        stringFormat: "{0: MMM}"
                                    )
                                    .Bind(
                                        Label.TextColorProperty,
                                        nameof(VenueDate.IsSelected),
                                        BindingMode.OneWay,
                                        convert: (bool state) =>
                                            state == false ? Colors.Black : Colors.White
                                    ),
                                new Bold16Label { }
                                    .Bind(
                                        Label.TextProperty,
                                        nameof(VenueDate.Date),
                                        stringFormat: "{0: dd}"
                                    )
                                    .Bind(
                                        Label.TextColorProperty,
                                        nameof(VenueDate.IsSelected),
                                        BindingMode.OneWay,
                                        convert: (bool state) =>
                                            state == false ? Colors.Black : Colors.White
                                    ),
                                new Medium10Label { }
                                    .Bind(
                                        Label.TextProperty,
                                        nameof(VenueDate.Date),
                                        stringFormat: "{0: ddd}"
                                    )
                                    .Bind(
                                        Label.TextColorProperty,
                                        nameof(VenueDate.IsSelected),
                                        BindingMode.OneWay,
                                        convert: (bool state) =>
                                            state == false ? Colors.Black : Colors.White
                                    ),
                            },
                        },
                    }.Bind(
                        BackgroundProperty,
                        nameof(VenueDate.IsSelected),
                        convert: (bool state) =>
                            state == false ? Color.FromArgb("#EDEDED") : Color.FromArgb("#EF2F50")
                    );
                }),
            }
                .Bind(ItemsView.ItemsSourceProperty, "BindingContext.VenueDates", source: this)
                .Bind(
                    SelectableItemsView.SelectedItemProperty,
                    "PlannedDate",
                    source: BindingContext
                )
                .Margins(18, 15, 0, 0)
                .Row(BodyRow.fifth);

            Grid skillsGrid = new Grid
            {
                ColumnDefinitions = Columns.Define(
                    (BodyColumn.first, Auto),
                    (BodyColumn.second, Star)
                ),
                Children =
                {
                    new Regular14Label
                    {
                        Text = "Skill",
                        VerticalOptions = LayoutOptions.Center,
                    }.Column(BodyColumn.first),
                    new Border
                    {
                        HorizontalOptions = LayoutOptions.End,
                        StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(7) },
                        Stroke = Color.FromArgb("#828282"),
                        Background = Colors.Transparent,

                        Content = new Grid
                        {
                            ColumnDefinitions = Columns.Define(
                                (BodyColumn.first, Auto),
                                (BodyColumn.second, Auto),
                                (BodyColumn.third, Auto),
                                (BodyColumn.fourth, Auto),
                                (BodyColumn.fifth, Auto)
                            ),

                            Children =
                            {
                                new Button
                                {
                                    Text = "Beginner",
                                    FontFamily = "InterRegular",
                                    FontSize = 10,
                                }
                                    .Bind(Button.CommandProperty, "BeginnerSelectedCommand")
                                    .Column(BodyColumn.first)
                                    .Bind(Button.BackgroundProperty, "BeginnerBackgroundColor")
                                    .Bind(Button.TextColorProperty, "BeginnerTextColor"),
                                new Rectangle { WidthRequest = 3, HeightRequest = 20 }
                                    .AppThemeBinding(
                                        Rectangle.BackgroundProperty,
                                        Color.FromArgb("#626262"),
                                        Color.FromArgb("#D2D2D2")
                                    )
                                    .Column(BodyColumn.second),
                                new Button
                                {
                                    Text = "Intermediate",
                                    FontFamily = "InterRegular",
                                    FontSize = 10,
                                }
                                    .Bind(Button.CommandProperty, "IntermediateSelectedCommand")
                                    .Column(BodyColumn.third)
                                    .Bind(
                                        Button.BackgroundProperty,
                                        "IntermediateBackgroundColor"
                                    )
                                    .Bind(Button.TextColorProperty, "IntermediateTextColor"),
                                new Rectangle { WidthRequest = 3, HeightRequest = 20 }
                                    .AppThemeBinding(
                                        Rectangle.BackgroundProperty,
                                        Color.FromArgb("#626262"),
                                        Color.FromArgb("#D2D2D2")
                                    )
                                    .Column(BodyColumn.fourth),
                                new Button
                                {
                                    Text = "Advanced",
                                    FontFamily = "InterRegular",
                                    FontSize = 10,
                                }
                                    .Bind(Button.CommandProperty, "AdvancedSelectedCommand")
                                    .Column(BodyColumn.fifth)
                                    .Bind(Button.BackgroundProperty, "AdvancedBackgroundColor")
                                    .Bind(Button.TextColorProperty, "AdvancedTextColor"),
                            },
                        },
                    }.Column(BodyColumn.second),
                },
            }
                .Margins(18, 15, 0, 0)
                .Row(BodyRow.sixth);

            Border locationBorder = new Border
            {
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(7) },
                Stroke = Color.FromArgb("#828282"),
                Background = Colors.Transparent,
#if IOS
                Padding = new Thickness(10, 15),
#elif ANDROID
                Padding = new Thickness(10, 5),
#endif
                Content = new Grid
                {
                    ColumnDefinitions = Columns.Define(
                        (BodyColumn.first, Auto),
                        (BodyColumn.second, Star),
                        (BodyColumn.third, Auto)
                    ),
                    ColumnSpacing = 20,
                    Children =
                    {
                        new Regular14Label
                        {
                            VerticalOptions = LayoutOptions.Center,
                            Text = "Select Location:",
                        }.Column(BodyColumn.first),
                        new Picker { Background = Colors.Transparent }
                            .AppThemeBinding(
                                Picker.TextColorProperty,
                                Color.FromArgb("#626262"),
                                Colors.White
                            )
                            .Bind(Picker.ItemsSourceProperty, "CityNames")
                            .Bind(Picker.SelectedItemProperty, "SelectedCity")
                            .Column(BodyColumn.second),
                        new Image { WidthRequest = 15, HeightRequest = 15 }
                            .AppThemeBinding(
                                Image.SourceProperty,
                                "skill_dropdown_lightmode",
                                "skill_dropdown_darkmode"
                            )
                            .Column(BodyColumn.third),
                    },
                },
            }
                .Margins(0, 15, 0, 0)
                .Row(BodyRow.seventh);

            Border venueBorder = new Border
            {
                StrokeThickness = .5,
                Stroke = Color.FromArgb("#828282"),
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(7) },
                Background = Colors.Transparent,
#if IOS
                Padding = new Thickness(10, 15),
#elif ANDROID
                Padding = new Thickness(10, 5),
#endif
                Content = new Grid
                {
                    ColumnDefinitions = Columns.Define(
                        (BodyColumn.first, Auto),
                        (BodyColumn.second, Star),
                        (BodyColumn.third, Auto)
                    ),
                    ColumnSpacing = 20,
                    Children =
                    {
                        new Regular14Label
                        {
                            VerticalOptions = LayoutOptions.Center,
                            Text = "Select Venue:",
                        }.Column(BodyColumn.first),
                        new Picker
                        {
                            Background = Colors.Transparent,
                            ItemDisplayBinding = new Binding(nameof(Venue.FullName)),
                        }
                            .AppThemeBinding(
                                Picker.TextColorProperty,
                                Color.FromArgb("#626262"),
                                Colors.White
                            )
                            .Bind(Picker.ItemsSourceProperty, "FilteredVendors")
                            .Bind(Picker.SelectedItemProperty, "SelectedVendor")
                            .Column(BodyColumn.second),
                        new Image { WidthRequest = 15, HeightRequest = 15 }
                            .AppThemeBinding(
                                Image.SourceProperty,
                                "skill_dropdown_lightmode",
                                "skill_dropdown_darkmode"
                            )
                            .Column(BodyColumn.third),
                    },
                },
            }
                .Margins(0, 10, 0, 0)
                .Row(BodyRow.eighth);

            Border playerCountBorder = new Border
            {
                StrokeThickness = .5,
                Stroke = Color.FromArgb("#828282"),
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(7) },
                Background = Colors.Transparent,
#if IOS
                Padding = new Thickness(10),
#elif ANDROID
                Padding = new Thickness(10, 10),
#endif
                Content = new Grid
                {
                    ColumnDefinitions = Columns.Define(
                        (BodyColumn.first, Auto),
                        (BodyColumn.second, Auto),
                        (BodyColumn.third, Star)
                    ),
                    ColumnSpacing = 10,
                    Children =
                    {
                        new Regular14Label
                        {
                            VerticalOptions = LayoutOptions.Center,
                            Text = "Number of Players:",
                        },
                        new Regular16Label { VerticalOptions = LayoutOptions.Center }
                            .Bind(
                                Regular14Label.TextProperty,
                                "RequiredPlayers",
                                convert: (double value) => value.ToString()
                            )
                            .Column(BodyColumn.second),
                        new Slider
                        {
                            ThumbColor = Color.FromArgb("#EF2F50"),
                            MinimumTrackColor = Color.FromArgb("#EF2F50"),
                            Minimum = 2,
                        }
                            .Bind(Slider.MaximumProperty, "NumberOfPlayers")
                            .Bind(Slider.ValueProperty, "RequiredPlayers")
                            .Column(BodyColumn.third)
                            .Invoke(slider => slider.ValueChanged += SetIntValue),
                    },
                },
            }
                .Margins(0, 10, 0, 0)
                .Row(BodyRow.ninth);

            Border timingBorder = new Border
            {
                StrokeThickness = .5,
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(7) },
                Stroke = Color.FromArgb("#828282"),
                Background = Colors.Transparent,

                Padding = new Thickness(10, 15),

                Content = new Grid
                {
                    ColumnDefinitions = Columns.Define(
                        (BodyColumn.first, Auto),
                        (BodyColumn.second, Star),
                        (BodyColumn.third, Auto)
                    ),
                    ColumnSpacing = 10,
                    Children =
                    {
                        new Regular14Label
                        {
                            VerticalOptions = LayoutOptions.Center,
                            Text = "Select Timing:",
                        },
                        new Regular14Label { HorizontalOptions = LayoutOptions.Start }
                            .Bind(Regular14Label.TextProperty, "Timing")
                            .Column(BodyColumn.second),
                        new Image { }
                            .AppThemeBinding(
                                Image.SourceProperty,
                                "forward_lightmode",
                                "forward_darkmode"
                            )
                            .Margins(0, 0, 0, 0)
                            .Column(BodyColumn.third),
                    },
                },
            }
                .Row(BodyRow.tenth)
                .Margins(0, 15, 0, 0)
                .BindTapGesture("SelectTimingCommand");

            MediumButton reviewButton = new MediumButton { Text = "Review" }
                .BindCommand("ReviewHostingCommand")
                .Row(BodyRow.eleventh)
                .Margins(10, 17, 10, 0);

            this.Children.Add(topImage);
            this.Children.Add(hostLabel);
            this.Children.Add(sportsCollection);
            this.Children.Add(line);
            this.Children.Add(dateGrid);
            this.Children.Add(skillsGrid);
            this.Children.Add(locationBorder);
            this.Children.Add(venueBorder);
            this.Children.Add(playerCountBorder);
            this.Children.Add(timingBorder);
            this.Children.Add(reviewButton);
        }
        catch (Exception ex)
        {
            Shell.Current.DisplayAlert("Error", ex.Message, "Okay");
        }
    }

    private void SetIntValue(object sender, ValueChangedEventArgs e)
    {
        Slider slider = (Slider)sender;
        slider.Value = Math.Round(e.NewValue);
    }
}
