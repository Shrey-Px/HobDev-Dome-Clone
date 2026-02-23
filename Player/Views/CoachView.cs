namespace Player.Views;

public class CoachView : BaseView
{
    public CoachView(CoachViewModel viewModel)
        : base(viewModel)
    {
        try
        {
            Resources.Add(
                new Style<UnderlinedTabItem>(
                    (UnderlinedTabItem.FontFamilyProperty, "InterExtraBold"),
                    (UnderlinedTabItem.UnderlineHeightProperty, 6),
                    (UnderlinedTabItem.MarginProperty, new Thickness(10, 0, 10, 0)),
                    (UnderlinedTabItem.LabelSizeProperty, 12),
                    (UnderlinedTabItem.SelectedTabColorProperty, Color.FromArgb("#EF2F50"))
                ).AddAppThemeBinding(
                    UnderlinedTabItem.UnselectedLabelColorProperty,
                    Color.FromArgb("7D7D7D"),
                    Color.FromArgb("#E0E0E0")
                )
            );

            FormattedString formattedString = new FormattedString();
            Span emailSpan = new Span { }.Bind(
                Span.TextProperty,
                nameof(Coach.Email),
                stringFormat: "{0} | "
            );

            Span phoneSpan = new Span { }.Bind(Span.TextProperty, nameof(Coach.ContactNumber));

            formattedString.Spans.Add(emailSpan);
            formattedString.Spans.Add(phoneSpan);

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

            base.BaseContentGrid.Children.Add(
                new Grid
                {
                    RowDefinitions = Rows.Define((BodyRow.first, Auto), (BodyRow.second, Star)),
                    Padding = new Thickness(15, 0, 15, 0),
                    Children =
                    {
                        new Border
                        {
                            StrokeThickness = 0,
                            Padding = new Thickness(10, 20, 10, 0),
                            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(7) },
                            Content = new VerticalStackLayout
                            {
                                Children =
                                {
                                    new Bold28Label { }.FormattedText(
                                        new[]
                                        {
                                            new Span
                                            {
                                                FontFamily = "InterBold",
                                                TextColor = Color.FromArgb("AEAEAE"),
                                            }.Bind(
                                                Span.TextProperty,
                                                nameof(viewModel.UserName),
                                                stringFormat: "Hey {0}\n"
                                            ),
                                            new Span
                                            {
                                                FontFamily = "InterBold",
                                                Text = "Find Your Guru",
                                            },
                                        }
                                    ),
                                    new TabHostView
                                    {
                                        TabType = TabType.Scrollable,
                                        HeightRequest = 50,
                                        Tabs =
                                        {
                                            new UnderlinedTabItem
                                            {
                                                Label = "Pickleball",
                                                HorizontalOptions = LayoutOptions.Center,
                                                Padding = new Thickness(5),
                                            },
                                            new UnderlinedTabItem { Label = "Badminton" },
                                            new UnderlinedTabItem { Label = "Volleyball" },
                                            new UnderlinedTabItem { Label = "Basketball" },
                                            new UnderlinedTabItem { Label = "Archery" },
                                            new UnderlinedTabItem { Label = "Bowling" },
                                            new UnderlinedTabItem { Label = "Golf" },
                                            new UnderlinedTabItem { Label = "Escaperoom" },
                                            new UnderlinedTabItem { Label = "Karting" },
                                            new UnderlinedTabItem { Label = "Snooker" },
                                            new UnderlinedTabItem { Label = "Table tennis" },
                                        },
                                    }
                                        .Bind(
                                            TabHostView.SelectedIndexProperty,
                                            nameof(viewModel.SelectedIndex),
                                            BindingMode.TwoWay
                                        )
                                        .Margins(0, 15, 0, 0),
                                },
                            },
                        }
                            .AppThemeBinding(
                                Border.BackgroundProperty,
                                Colors.White,
                                Color.FromArgb("#23262A")
                            )
                            .Row(BodyRow.first),
                        new CollectionView
                        {
                            Margin = new Thickness(0, 20, 0, 0),
                            ItemsLayout = new LinearItemsLayout(ItemsLayoutOrientation.Vertical)
                            {
                                ItemSpacing = 10,
                            },

                            EmptyView = new Regular16Label
                            {
                                Text = "No coaches available",
                                HorizontalOptions = LayoutOptions.Center,
                                VerticalOptions = LayoutOptions.Center,
                            },

                            ItemTemplate = new DataTemplate(() =>
                            {
                                return new Border
                                {
                                    StrokeThickness = 0,
                                    StrokeShape = new RoundRectangle { CornerRadius = 7 },
                                    Padding = new Thickness(20, 10, 10, 20),
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
                                            (BodyColumn.second, Auto)
                                        ),
                                        Children =
                                        {
                                            new Regular10Label { Text = "Coach Name" }
                                                .Row(BodyRow.first)
                                                .Column(BodyColumn.first),
                                            new SemiBold16Label { }
                                                .Bind(
                                                    SemiBold16Label.TextProperty,
                                                    nameof(Coach.CoachName)
                                                )
                                                .Row(BodyRow.second)
                                                .Column(BodyColumn.first),
                                            new SemiBold12Label { }
                                                .Bind(
                                                    SemiBold12Label.TextProperty,
                                                    nameof(Coach.Venue.FullName)
                                                )
                                                .Row(BodyRow.third)
                                                .Column(BodyColumn.first)
                                                .Margins(0, 5, 0, 0),
                                            new Regular10Label { }
                                                .Bind(
                                                    Regular10Label.TextProperty,
                                                    nameof(Coach.Competancy)
                                                )
                                                .Row(BodyRow.fourth)
                                                .Column(BodyColumn.first),
                                            new Regular12Label { }
                                                .Bind(
                                                    Regular12Label.TextProperty,
                                                    nameof(Coach.CoachDescription)
                                                )
                                                .Row(BodyRow.fifth)
                                                .Column(BodyColumn.first)
                                                .ColumnSpan(2)
                                                .Margins(0, 12, 0, 0),
                                            new HorizontalStackLayout
                                            {
                                                Children =
                                                {
                                                    new Regular12Label { }.Bind(
                                                        Regular12Label.TextProperty,
                                                        nameof(Coach.Email),
                                                        stringFormat: "{0} | "
                                                    ),
                                                    new Regular12Label { }.Bind(
                                                        Regular12Label.TextProperty,
                                                        nameof(Coach.ContactNumber)
                                                    ),
                                                },
                                            }
                                                .Row(BodyRow.sixth)
                                                .Column(BodyColumn.first)
                                                .ColumnSpan(2)
                                                .Margins(0, 10, 0, 0),
                                            new Image
                                            {
                                                HorizontalOptions = LayoutOptions.Center,
                                                WidthRequest = 22,
                                                HeightRequest = 22,
                                            }
                                                .Bind(
                                                    Image.SourceProperty,
                                                    nameof(viewModel.GameImage),
                                                    converter: new ByteArrayToImageSourceConverter(),
                                                    source: viewModel
                                                )
                                                .Row(BodyRow.first)
                                                .RowSpan(2)
                                                .Column(BodyColumn.second)
                                                .Margins(0, 0, 10, 0),
                                        },
                                    },
                                }.AppThemeBinding(
                                    Border.BackgroundProperty,
                                    Colors.White,
                                    Color.FromArgb("#23262A")
                                );
                            })
                            { },
                        }
                            .Row(BodyRow.second)
                            .Bind(ItemsView.ItemsSourceProperty, nameof(viewModel.FilteredCoaches)),
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
