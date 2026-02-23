namespace Player.Views.Book;

public class TeamView : BaseView
{
    TeamViewModel? viewModel;

    public TeamView(TeamViewModel viewModel)
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
                    .BindCommand(nameof(viewModel.CloseCommand))
                    .Column(BodyColumn.first)
            );

            base.BaseContentGrid.Children.Add(
                new ScrollView
                {
                    Content = new VerticalStackLayout
                    {
                        Padding = new Thickness(15, 0, 15, 0),
                        Children =
                        {
                            new Border
                            {
                                StrokeThickness = 0,
                                StrokeShape = new RoundRectangle
                                {
                                    CornerRadius = new CornerRadius(7),
                                },
                                Content = new VerticalStackLayout
                                {
                                    Padding = new Thickness(18, 14, 0, 18),
                                    Children =
                                    {
                                        new Regular26Label { Text = "Member List" },
                                        new Medium12Label { }.Bind(
                                            Medium12Label.TextProperty,
                                            nameof(viewModel.NameAndLevel)
                                        ),
                                        new Medium12Label { }.Bind(
                                            Medium12Label.TextProperty,
                                            nameof(viewModel.OtherDetails)
                                        ),
                                        new SemiBold12Label { }.Bind(
                                            SemiBold12Label.TextProperty,
                                            binding1: new Binding(
                                                $"{nameof(viewModel.SelectedBooking)}.{nameof(viewModel.SelectedBooking.StartTime)}",
                                                stringFormat: "{0:dd MMMM, HH.mm}"
                                            ),
                                            binding2: new Binding(
                                                $"{nameof(viewModel.SelectedBooking)}.{nameof(viewModel.SelectedBooking.TotalDuration)}",
                                                stringFormat: "{0} hours"
                                            ),
                                            binding3: new Binding(
                                                $"{nameof(viewModel.SelectedBooking)}.{nameof(viewModel.SelectedBooking.FieldName)}"
                                            ),
                                            convert: (
                                                (string? start, string? end, string? field) values
                                            ) => $"{values.start} | {values.end} | {values.field}"
                                        ),
                                    },
                                },
                            }.AppThemeBinding(
                                Border.BackgroundProperty,
                                Colors.White,
                                Color.FromArgb("#23262A")
                            ),
                            new SemiBold12Label
                            {
                                FontAttributes = FontAttributes.Bold,
                                FontSize = 14,
                                Margin = new Thickness(0, 10, 0, 0),
                            }
                                .Bind(
                                    SemiBold12Label.TextProperty,
                                    nameof(viewModel.PlayerCount),
                                    stringFormat: "Players going ({0})"
                                )
                                .Margins(0, 15, 0, 0),
                            new Border
                            {
                                HeightRequest = 300,
                                Margin = new Thickness(0, 10, 0, 0),
                                StrokeThickness = 0,
                                Padding = new Thickness(10, 12, 10, 12),
                                StrokeShape = new RoundRectangle
                                {
                                    CornerRadius = new CornerRadius(7),
                                },
                                VerticalOptions = LayoutOptions.Start,
                                Content = new CollectionView
                                {
                                    Header = new Grid
                                    {
                                        RowDefinitions = Rows.Define(
                                            (BodyRow.first, Auto),
                                            (BodyRow.second, Auto),
                                            (BodyRow.third, Auto)
                                        ),

                                        ColumnDefinitions = Columns.Define(
                                            (BodyColumn.first, Auto),
                                            (BodyColumn.second, Star)
                                        ),
                                        ColumnSpacing = 10,
                                        Children =
                                        {
                                            new AvatarView
                                            {
                                                HeightRequest = 35,
                                                WidthRequest = 35,
                                                CornerRadius = 17.5,
                                                Padding = 0,
                                                BorderWidth = 0,
                                                VerticalOptions = LayoutOptions.Start,
                                            }
                                                .Bind(
                                                    AvatarView.ImageSourceProperty,
                                                    $"{nameof(viewModel.SelectedBooking)}.{nameof(viewModel.SelectedBooking.Player)}.{nameof(viewModel.SelectedBooking.Player.ProfileImage)}",
                                                    converter: new ByteArrayToImageSourceConverter(),
                                                    fallbackValue: "profile_image_fallback.png",
                                                    targetNullValue: "profile_image_fallback",
                                                    source: viewModel
                                                )
                                                .Row(BodyRow.first)
                                                .RowSpan(2)
                                                .Column(BodyColumn.first),
                                            new Medium12Label { }
                                                .Bind(
                                                    Medium12Label.TextProperty,
                                                    $"{nameof(viewModel.SelectedBooking)}.{nameof(viewModel.SelectedBooking.Player)}.{nameof(viewModel.SelectedBooking.Player.FullName)}",
                                                    source: viewModel
                                                )
                                                .Row(BodyRow.first)
                                                .Column(BodyColumn.second)
                                                .Margins(0, 3, 0, 0),
                                            new Medium10Label { Text = "Host" }
                                                .Row(BodyRow.second)
                                                .Column(BodyColumn.second)
                                                .Margins(0, 0, 0, 20),
                                            new Rectangle
                                            {
                                                HeightRequest = 2,
                                                Background = Color.FromArgb("#D4D4D4"),
                                            }
                                                .Row(BodyRow.third)
                                                .Column(BodyColumn.first)
                                                .ColumnSpan(2),
                                        },
                                    }.Margins(0, 0, 0, 10),

                                    ItemsLayout = new LinearItemsLayout(
                                        ItemsLayoutOrientation.Vertical
                                    )
                                    {
                                        ItemSpacing = 0,
                                    },
                                    ItemSizingStrategy = ItemSizingStrategy.MeasureFirstItem,
                                    ItemTemplate = new DataTemplate(() =>
                                    {
                                        return new Grid
                                        {
                                            RowDefinitions = Rows.Define(
                                                (BodyRow.first, Auto),
                                                (BodyRow.second, Auto),
                                                (BodyRow.third, Auto)
                                            ),

                                            ColumnDefinitions = Columns.Define(
                                                (BodyColumn.first, Auto),
                                                (BodyColumn.second, Star)
                                            ),
                                            ColumnSpacing = 10,

                                            Children =
                                            {
                                                new AvatarView
                                                {
                                                    HeightRequest = 35,
                                                    WidthRequest = 35,
                                                    CornerRadius = 17.5,
                                                    Padding = 0,
                                                    BorderWidth = 0,
                                                    VerticalOptions = LayoutOptions.Start,
                                                }
                                                    .Bind(
                                                        AvatarView.ImageSourceProperty,
                                                        nameof(VenueUser.ProfileImage),
                                                        converter: new ByteArrayToImageSourceConverter(),
                                                        fallbackValue: "profile_image_fallback.png",
                                                        targetNullValue: "profile_image_fallback"
                                                    )
                                                    .Row(BodyRow.first)
                                                    .RowSpan(2)
                                                    .Column(BodyColumn.first)
                                                    .Margins(0, 10, 0, 10),
                                                new Medium12Label { }
                                                    .Bind(
                                                        Medium12Label.TextProperty,
                                                        nameof(VenueUser.FullName)
                                                    )
                                                    .Row(BodyRow.first)
                                                    .Column(BodyColumn.second)
                                                    .Margins(0, 14, 0, 0),
                                                new Medium10Label { Text = "Member" }
                                                    .Row(BodyRow.second)
                                                    .Column(BodyColumn.second)
                                                    .Margins(0, 0, 0, 20),
                                                new Rectangle
                                                {
                                                    HeightRequest = 2,
                                                    Background = Color.FromArgb("#D4D4D4"),
                                                }
                                                    .Row(BodyRow.third)
                                                    .Column(BodyColumn.first)
                                                    .ColumnSpan(2),
                                            },
                                        };
                                    }),
                                }.Bind(
                                    ItemsView.ItemsSourceProperty,
                                    $"{nameof(viewModel.SelectedBooking)}.{nameof(viewModel.SelectedBooking.JoinedPlayers)}",
                                    source: viewModel
                                ),
                            }.AppThemeBinding(
                                Border.BackgroundProperty,
                                Colors.White,
                                Color.FromArgb("#23262A")
                            ),
                        },
                    },
                }
                    .Row(BodyRow.second)
                    .ColumnSpan(2)
            );

            BindingContext = viewModel;
        }
        catch (Exception ex)
        {
            Shell.Current.DisplayAlert("Error", ex.Message, "Ok");
        }
    }
}
