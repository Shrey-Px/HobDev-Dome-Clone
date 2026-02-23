namespace Player.Views.Book;

public class AvailableGamesView : BaseView
{
    CollectionView? collectionView;

    AvailableGamesViewModel? viewModel;

    public AvailableGamesView(AvailableGamesViewModel viewModel)
        : base(viewModel)
    {
        try
        {
            this.viewModel = viewModel;

            Resources.Add(
                new Style<CustomBorder>(
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
                                                Value = Color.FromArgb("#EF2F50"),
                                            },
                                        },
                                    },
                                },
                            },
                        }
                    )
                )
            );

            collectionView = new CollectionView
            {
                SelectionMode = SelectionMode.Single,

                ItemSizingStrategy = ItemSizingStrategy.MeasureFirstItem,

                ItemsLayout = new GridItemsLayout(2, ItemsLayoutOrientation.Vertical)
                {
                    VerticalItemSpacing = 20,
                    HorizontalItemSpacing = 20,
                },
                ItemTemplate = new DataTemplate(() =>
                {
                    return new CustomBorder
                    {
                        HeightRequest = 128,
                        WidthRequest = 144,
                        StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(22) },
                        Background = Color.FromArgb("#EF2F50"),
                        Padding = 20,
                        StrokeThickness = 0,

                        Content = new Grid
                        {
                            Children =
                            {
                                new SemiBold16Label
                                {
                                    TextColor = Colors.White,
                                    Margin = new Thickness(5, 0, 0, 0),
                                }.Bind(Bold16Label.TextProperty, nameof(Game.GameName)),
                                new Image
                                {
                                    HorizontalOptions = LayoutOptions.End,
                                    VerticalOptions = LayoutOptions.End,
                                    HeightRequest = 42,
                                    WidthRequest = 42,
                                }
                                    .Bind(
                                        Image.SourceProperty,
                                        nameof(Game.DarkModeGameIcon),
                                        converter: new ByteArrayToImageSourceConverter()
                                    )
                                    .Row(BodyRow.first),
                            },
                        },
                    };
                }),
                SelectionChangedCommand = viewModel.GameChoosenCommand,
            }
                .Bind(ItemsView.ItemsSourceProperty, nameof(viewModel.AvailableGames))
                .Bind(SelectableItemsView.SelectedItemProperty, nameof(viewModel.SelectedGame));

            base.TopGrid.Children.Add(
                new ImageButton
                {
                    HorizontalOptions = LayoutOptions.Start,
                    VerticalOptions = LayoutOptions.Start,
                }.AppThemeBinding(ImageButton.SourceProperty, "dome_light_theme", "dome_dark_theme")
            );

            base.BaseContentGrid.Add(
                new Grid
                {
                    Padding = new Thickness(16, 0, 16, 31),
                    RowDefinitions = Rows.Define(
                        (BodyRow.first, Auto),
                        (BodyRow.second, Auto),
                        (BodyRow.third, Star)
                    ),

                    RowSpacing = 10,
                    Children =
                    {
                        new Border
                        {
                            StrokeThickness = 0,
                            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(7) },
                            Content = new VerticalStackLayout
                            {
                                Padding = new Thickness(18, 14),
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
                                    new Regular28Label { Text = "What are you playing\ntoday?" },
                                },
                            },
                        }
                            .AppThemeBinding(
                                Border.BackgroundProperty,
                                Colors.White,
                                Color.FromArgb("#23262A")
                            )
                            .Row(BodyRow.first),
                        new Border
                        {
                            StrokeShape = new RoundRectangle
                            {
                                CornerRadius = new CornerRadius(10),
                            },
                            Padding = new Thickness(15, 10, 15, 25),
                            Content = new Grid
                            {
                                RowDefinitions = Rows.Define((BodyRow.first, Auto)),
                                ColumnDefinitions = Columns.Define(
                                    (BodyColumn.first, Star),
                                    (BodyColumn.second, Auto)
                                ),

                                ColumnSpacing = 5,

                                Children =
                                {
                                    new VerticalStackLayout
                                    {
                                        Children =
                                        {
                                            new Border
                                            {
                                                HorizontalOptions = LayoutOptions.Start,
                                                StrokeShape = new RoundRectangle
                                                {
                                                    CornerRadius = new CornerRadius(7),
                                                },
                                                Padding = new Thickness(10, 5),
                                                Background = Color.FromArgb("#5D5D5D"),
                                                Content = new Medium8Label
                                                {
                                                    Text = "RECENTLY BOOKED",
                                                    TextColor = Colors.White,
                                                },
                                            },
                                            new Medium10Label
                                            {
                                                VerticalOptions = LayoutOptions.Start,
                                                Text = "No recent bookings",
                                            }
                                                .Bind(
                                                    Medium10Label.IsVisibleProperty,
                                                    nameof(viewModel.RecentBooking),
                                                    convert: (Booking? booking) =>
                                                        booking == null ? true : false,
                                                    source: viewModel
                                                )
                                                .Margins(5, 0, 0, 0),
                                            new Medium14Label { }
                                                .Bind(
                                                    Medium14Label.TextProperty,
                                                    $"{nameof(viewModel.RecentBooking)}.{nameof(viewModel.RecentBooking.VenueName)}",
                                                    source: viewModel
                                                )
                                                .Margins(0, 8, 0, 0),
                                            new Medium10Label
                                            {
                                                VerticalOptions = LayoutOptions.Start,
                                            }.Bind(
                                                Medium10Label.TextProperty,
                                                $"{nameof(viewModel.RecentBooking)}.{nameof(viewModel.RecentBooking.BookingDate)}",
                                                stringFormat: "Last Booked: {0:dd MMMM, yyyy}",
                                                source: viewModel
                                            ),
                                        },
                                    }
                                        .Row(BodyRow.first)
                                        .Column(BodyColumn.first),
                                    new MediumButton
                                    {
                                        Text = "BOOK NOW",
                                        FontSize = 11,
                                        VerticalOptions = LayoutOptions.Center,
                                        Padding = new Thickness(20, 10),
                                    }
                                        .Row(BodyRow.first)
                                        .Column(BodyColumn.second)
                                        .BindCommand(nameof(viewModel.VenueDetailsCommand)),
                                },
                            },
                        }
                            .AppThemeBinding(
                                Border.BackgroundProperty,
                                Colors.White,
                                Color.FromArgb("#23262A")
                            )
                            .Row(BodyRow.second),
                        new Border
                        {
                            StrokeThickness = 0,
                            Padding = 0,
                            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(7) },
                            Content = collectionView.Margins(20, 15, 20, 10),
                        }
                            .AppThemeBinding(
                                Border.BackgroundProperty,
                                Colors.White,
                                Colors.Black
                            )
                            .Row(BodyRow.third),
                    },
                }
                    .Row(BodyRow.second)
                    .ColumnSpan(2)
            );

            Loaded += SportsListView_Loaded;
        }
        catch (Exception ex)
        {
            Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async void SportsListView_Loaded(object? sender, EventArgs e)
    {
        try
        {
            if (viewModel != null)
            {
                await viewModel.OnPageLoad();
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
    }
}
