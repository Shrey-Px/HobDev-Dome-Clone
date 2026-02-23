namespace Player.Views;

public class LearnView : BaseView
{
    public LearnView(LearnViewModel viewModel)
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
                    (UnderlinedTabItem.WidthProperty, 100),
                    (UnderlinedTabItem.SelectedTabColorProperty, Color.FromArgb("#EF2F50"))
                ).AddAppThemeBinding(
                    UnderlinedTabItem.UnselectedLabelColorProperty,
                    Color.FromArgb("7D7D7D"),
                    Color.FromArgb("#E0E0E0")
                )
            );

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
                    .BindCommand(
                        static (LearnViewModel vm) => vm.NavigateBackCommand,
                        source: viewModel
                    )
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
                            Content = new Grid
                            {
                                RowDefinitions = Rows.Define(
                                    (BodyRow.first, Auto),
                                    (BodyRow.second, Auto)
                                ),
                                Children =
                                {
                                    new Bold28Label { }
                                        .FormattedText(
                                            new[]
                                            {
                                                new Span
                                                {
                                                    FontFamily = "InterBold",
                                                    TextColor = Color.FromArgb("AEAEAE"),
                                                }.Bind(
                                                    Span.TextProperty,
                                                    static (LearnViewModel vm) => vm.UserName,
                                                    source: viewModel,
                                                    stringFormat: "Hey {0}\n"
                                                ),
                                                new Span
                                                {
                                                    FontFamily = "InterBold",
                                                    Text = "Learning is Growing",
                                                },
                                            }
                                        )
                                        .Row(BodyRow.first),
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
                                        .Row(BodyRow.second)
                                        .Bind(
                                            TabHostView.SelectedIndexProperty,
                                            static (LearnViewModel vm) => vm.SelectedIndex,
                                            source: viewModel,
                                            mode: BindingMode.TwoWay
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
                            // ItemSizingStrategy = ItemSizingStrategy.MeasureFirstItem,
                            EmptyView = "No learning content is available",

                            ItemTemplate = new DataTemplate(() =>
                            {
                                return new Border
                                {
                                    Padding = new Thickness(15, 10, 20, 10),
                                    StrokeThickness = 0,
                                    StrokeShape = new RoundRectangle { CornerRadius = 15 },
                                    Content = new Grid
                                    {
                                        ColumnDefinitions = Columns.Define(
                                            (BodyColumn.first, 200),
                                            (BodyColumn.second, Star),
                                            (BodyColumn.third, Auto)
                                        ),

                                        RowDefinitions = Rows.Define(
                                            (BodyRow.first, 15),
                                            (BodyRow.second, 25),
                                            (BodyRow.third, 25),
                                            (BodyRow.fourth, Auto)
                                        ),

                                        Children =
                                        {
                                            new Regular10Label { Text = "Created By" }
                                                .Row(BodyRow.first)
                                                .Column(BodyColumn.first),
                                            new SemiBold16Label { }
                                                .Bind(
                                                    SemiBold16Label.TextProperty,
                                                    nameof(LearningContent.PublishedBy)
                                                )
                                                .Row(BodyRow.second)
                                                .Column(BodyColumn.first),
                                            new SemiBold12Label { }
                                                .Bind(
                                                    SemiBold12Label.TextProperty,
                                                    nameof(LearningContent.Title)
                                                )
                                                .Row(BodyRow.third)
                                                .Column(BodyColumn.first)
                                                .Margins(0, 10, 0, 0),
                                            new Regular12Label { }
                                                .Bind(
                                                    Regular12Label.TextProperty,
                                                    nameof(LearningContent.Description)
                                                )
                                                .Row(BodyRow.fourth)
                                                .Column(BodyColumn.first),
                                            new Image { HorizontalOptions = LayoutOptions.Center }
                                                .Bind(
                                                    Image.IsVisibleProperty,
                                                    nameof(LearningContent.Type),
                                                    convert: (string? type) =>
                                                        type == "Video" ? true : false
                                                )
                                                .AppThemeBinding(
                                                    Image.SourceProperty,
                                                    "video_content_lightmode",
                                                    "video_content_darkmode"
                                                )
                                                .Row(BodyRow.first)
                                                .RowSpan(2)
                                                .Column(BodyColumn.third),
                                            new Image { HorizontalOptions = LayoutOptions.Center }
                                                .Bind(
                                                    Image.IsVisibleProperty,
                                                    nameof(LearningContent.Type),
                                                    convert: (string? type) =>
                                                        type == "Article" ? true : false
                                                )
                                                .AppThemeBinding(
                                                    Image.SourceProperty,
                                                    "text_content_lightmode",
                                                    "text_content_darkmode"
                                                )
                                                .Row(BodyRow.first)
                                                .RowSpan(2)
                                                .Column(BodyColumn.third),
                                            new MediumButton
                                            {
                                                Text = "View",
                                                Background = Colors.Black,
                                                FontFamily = "InterBold",
                                                CornerRadius = 20,
                                                FontSize = 10,
                                                HorizontalOptions = LayoutOptions.Center,
                                                VerticalOptions = LayoutOptions.End,
                                            }
                                                .BindCommand(
                                                    nameof(viewModel.ShowContentCommand),
                                                    source: viewModel,
                                                    parameterPath: "."
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
                            }),
                        }
                            .Row(BodyRow.second)
                            .Bind(
                                ItemsView.ItemsSourceProperty,
                                static (LearnViewModel vm) => vm.FilteredContentList,
                                source: viewModel,
                                mode: BindingMode.OneWay
                            ),
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
