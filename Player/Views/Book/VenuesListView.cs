namespace Player.Views.Book;

public class VenuesListView : BaseView
{
    public VenuesListView(VenuesListViewModel viewModel)
        : base(viewModel)
    {
        try
        {
            // FormattedString formattedString = new FormattedString();
            // Span rupeeSpan = new Span { }.Bind(Span.TextProperty, nameof(Venue.WeekdayHourlyRate), stringFormat: "{0:C} ");
            // Span textSpan = new Span { Text = "Onwards", FontSize = 9 };

            // formattedString.Spans.Add(rupeeSpan);
            // formattedString.Spans.Add(textSpan);

            CollectionView verticalList = new CollectionView
            {
                SelectionMode = SelectionMode.Single,
                ItemTemplate = new DataTemplate(() =>
                {
                    return new Border
                    {
                        Padding = new Thickness(5),
                        StrokeThickness = 0,
                        StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(7) },

                        Content = new Grid
                        {
                            RowDefinitions = Rows.Define(
                                (BodyRow.first, 137),
                                (BodyRow.second, Auto),
                                (BodyRow.third, Auto)
                            ),
                            ColumnDefinitions = Columns.Define(
                                (BodyColumn.first, 25),
                                (BodyColumn.second, Star),
                                (BodyColumn.third, Auto)
                            ),
                            RowSpacing = 2,
                            ColumnSpacing = 10,
                            Children =
                            {
                                new Border
                                {
                                    StrokeThickness = 0,
                                    StrokeShape = new RoundRectangle { CornerRadius = 7 },
                                    Padding = 0,

                                    Content = new Grid
                                    {
                                        Children =
                                        {
                                            //using imagebutton instead of image because the image have a bug in iOS due to which no content is displayed
                                            new ImageButton { Aspect = Aspect.AspectFill }
                                                .BindCommand(
                                                    nameof(viewModel.VenueDetailsCommand),
                                                    source: viewModel
                                                )
                                                .Bind(
                                                    ImageButton.SourceProperty,
                                                    nameof(Venue.SelectedGamesImage),
                                                    converter: new ByteArrayToImageSourceConverter()
                                                ),
                                            new Regular10Label
                                            {
                                                Text = "Dome Recommended",
                                                TextColor = Colors.White,
                                                Background = Color.FromArgb("#EF2F50"),
                                                HorizontalOptions = LayoutOptions.End,
                                                VerticalOptions = LayoutOptions.Start,
                                                Padding = new Thickness(10, 5),
                                            }
                                                .Bind(
                                                    VisualElement.IsVisibleProperty,
                                                    nameof(Venue.IsPromoted)
                                                )
                                                .Margins(0, 10, 0, 0),
                                        },
                                    },
                                }
                                    .Row(BodyRow.first)
                                    .ColumnSpan(3),
                                new Image
                                {
                                    HeightRequest = 25,
                                    WidthRequest = 25,
                                    VerticalOptions = LayoutOptions.Start,
                                }
                                    .Bind(
                                        Image.SourceProperty,
                                        nameof(viewModel.SelectedGameImage),
                                        converter: new ByteArrayToImageSourceConverter(),
                                        source: viewModel
                                    )
                                    .Margins(10, 7, 0, 0)
                                    .Row(BodyRow.second)
                                    .RowSpan(2)
                                    .Column(BodyColumn.first),
                                new SemiBold13Label { }
                                    .Bind(SemiBold13Label.TextProperty, nameof(Venue.FullName))
                                    .Row(BodyRow.second)
                                    .Column(BodyColumn.second)
                                    .Margins(0, 4, 0, 0),
                                new Medium11Label { MaxLines = 2 }
                                    .Bind(
                                        Medium11Label.TextProperty,
                                        nameof(Venue.FullAddress),
                                        BindingMode.OneWay
                                    )
                                    .AppThemeBinding(
                                        Medium11Label.TextColorProperty,
                                        Color.FromArgb("#626262"),
                                        Color.FromArgb("BBBBBB")
                                    )
                                    .Row(BodyRow.third)
                                    .Column(BodyColumn.second),

                                //  new Medium14Label{ HorizontalOptions=LayoutOptions.End , FormattedText= formattedString}.AppThemeBinding(Medium14Label.TextColorProperty, Color.FromArgb("#626262"), Color.FromArgb("BBBBBB")).Row(BodyRow.third).Column(BodyColumn.third),
                            },
                        },
                    }.AppThemeBinding(
                        Border.BackgroundProperty,
                        Colors.White,
                        Color.FromArgb("#23262A")
                    );
                }),
                ItemsLayout = new LinearItemsLayout(ItemsLayoutOrientation.Vertical)
                {
                    ItemSpacing = 20,
                },
            }
                .Bind(ItemsView.ItemsSourceProperty, nameof(viewModel.FilteredVendors))
                .Bind(
                    SelectableItemsView.SelectionChangedCommandProperty,
                    nameof(viewModel.VenueDetailsCommand),
                    source: viewModel
                )
                .Bind(SelectableItemsView.SelectedItemProperty, nameof(viewModel.ChoosenVendor));

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
                    .Column(BodyColumn.first)
            );

            base.BaseContentGrid.Add(
                new Grid
                {
                    Padding = new Thickness(15, 0, 15, 28),
                    RowDefinitions = Rows.Define((BodyRow.first, Auto), (BodyRow.second, Star)),
                    Children =
                    {
                        new Border
                        {
                            StrokeThickness = 0,
                            Padding = new Thickness(18, 14),
                            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(7) },
                            Content = new VerticalStackLayout
                            {
                                Children =
                                {
                                    new ExtraBold16Label
                                    {
                                        FontFamily = "InterExtraBold",
                                        FontSize = 16,
                                    }
                                        .Bind(
                                            ExtraBold16Label.TextProperty,
                                            nameof(viewModel.SelectedGameName),
                                            stringFormat: "{0}"
                                        )
                                        .Margins(10, 0, 0, 3),
                                    new ExtraBold16Label
                                    {
                                        FontFamily = "InterExtraBold",
                                        FontSize = 28,
                                        Text = "Let's Explore!",
                                    }.Margins(10, 0, 0, 0),
                                    new Border
                                    {
                                        StrokeThickness = .5,
                                        StrokeShape = new RoundRectangle
                                        {
                                            CornerRadius = new CornerRadius(23),
                                        },
                                        Padding = new Thickness(
                                            DeviceInfo.Platform == DevicePlatform.iOS ? 10 : 0
                                        ),
                                        Background = Colors.Transparent,
                                        Content = new Grid
                                        {
                                            ColumnDefinitions = Columns.Define(
                                                (BodyColumn.first, Auto),
                                                (BodyColumn.second, Star)
                                            ),
                                            ColumnSpacing = 10,
                                            Children =
                                            {
                                                new Image
                                                {
                                                    Margin = new Thickness(5, 0, 0, 0),
                                                    Source = "search_icon.png",
                                                },
                                                new Entry
                                                {
                                                    Placeholder =
                                                        "Search by Venue name or location",
                                                    FontFamily = "InterMedium",
                                                    FontSize = 14,
                                                    Background = Colors.Transparent,
                                                }
                                                    .AppThemeBinding(
                                                        Entry.PlaceholderColorProperty,
                                                        Color.FromArgb("#AEAEAE"),
                                                        Color.FromArgb("#E0E0E0")
                                                    )
                                                    .AppThemeBinding(
                                                        Entry.TextColorProperty,
                                                        Color.FromArgb("#AEAEAE"),
                                                        Color.FromArgb("#E0E0E0")
                                                    )
                                                    .Bind(
                                                        Entry.TextProperty,
                                                        nameof(viewModel.SearchQuery)
                                                    )
                                                    .Column(BodyColumn.second),
                                            },
                                        },
                                    }
                                        .Margins(0, 12, 0, 0)
                                        .AppThemeBinding(
                                            Border.StrokeProperty,
                                            Brush.Black,
                                            new SolidColorBrush(Color.FromArgb("#8E8E8E"))
                                        ),
                                },
                            },
                        }
                            .AppThemeBinding(
                                Border.BackgroundProperty,
                                Colors.White,
                                Color.FromArgb("#23262A")
                            )
                            .Row(BodyRow.first),
                        verticalList.Margins(0, 20, 0, 0).Row(BodyRow.second),
                    },
                }
                    .Row(BodyRow.second)
                    .ColumnSpan(2)
            );

            Loaded += async (s, e) =>
            {
                await viewModel.LoadData();
            };
        }
        catch (Exception ex)
        {
            Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
    }
}
