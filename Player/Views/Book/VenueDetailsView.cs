namespace Player.Views.Book;

public class VenueDetailsView : BaseView
{
    public VenueDetailsView(VenueDetailsViewModel viewModel)
        : base(viewModel)
    {
        try
        {
            this.AppThemeBinding(ContentPage.BackgroundProperty, Colors.White, Colors.Black);

            FormattedString weekDayFormattedString = new FormattedString();
            Span rupeeSpan = new Span { }.Bind(
                Span.TextProperty,
                $"{nameof(viewModel.SelectedGame)}.{nameof(viewModel.SelectedGame.WeekdayHourlyRate)}",
                stringFormat: "${0} "
            );
            Span textSpan = new Span { Text = "per hour", FontSize = 9 };

            weekDayFormattedString.Spans.Add(rupeeSpan);
            weekDayFormattedString.Spans.Add(textSpan);

            FormattedString weekEndFormattedString = new FormattedString();
            Span weekEndRupeeSpan = new Span { }.Bind(
                Span.TextProperty,
                $"{nameof(viewModel.SelectedGame)}.{nameof(viewModel.SelectedGame.WeekendHourlyRate)}",
                stringFormat: "${0} "
            );
            Span weekEndTextSpan = new Span { Text = "per hour", FontSize = 9 };

            weekEndFormattedString.Spans.Add(weekEndRupeeSpan);
            weekEndFormattedString.Spans.Add(weekEndTextSpan);

            IndicatorView indicatorView = new IndicatorView
            {
                IndicatorColor = Colors.DarkGray,
                SelectedIndicatorColor = Color.FromArgb("#EF2F50"),
                IndicatorSize = 8,
                HorizontalOptions = LayoutOptions.Center,
            };

            CollectionView amenitiesCollection = new CollectionView
            {
                ItemSizingStrategy = ItemSizingStrategy.MeasureAllItems,
                HeightRequest = 40,
                ItemTemplate = new DataTemplate(() =>
                {
                    return new Border
                    {
                        StrokeThickness = .5,
                        Background = Colors.Transparent,
                        StrokeShape = new RoundRectangle { CornerRadius = 7 },
                        HorizontalOptions = LayoutOptions.Center,
                        Content = new HorizontalStackLayout
                        {
                            Padding = new Thickness(10, 5),
                            Spacing = 5,
                            Children =
                            {
                                new Image { HeightRequest = 20, WidthRequest = 20 }.Bind(
                                    Image.SourceProperty,
                                    nameof(Amenity.AmenityImage),
                                    converter: new ByteArrayToImageSourceConverter()
                                ),
                                new Medium11Label
                                {
                                    VerticalOptions = LayoutOptions.Center,
                                    HorizontalOptions = LayoutOptions.Center,
                                }.Bind(
                                    Medium11Label.TextProperty,
                                    nameof(Amenity.AmenityName),
                                    stringFormat: "{0:mm}"
                                ),
                            },
                        },
                    }
                        .AppThemeBinding(Border.StrokeProperty, Colors.Black, Colors.White)
                        .Margins(0, 7, 0, 7);
                }),
                ItemsLayout = new LinearItemsLayout(ItemsLayoutOrientation.Horizontal)
                {
                    ItemSpacing = 10,
                },
            }.Bind(ItemsView.ItemsSourceProperty, nameof(viewModel.Amenities));
            SemiBold14Label weekDayLabel = new SemiBold14Label { }.AppThemeBinding(
                SemiBold14Label.TextColorProperty,
                Color.FromArgb("#626262"),
                Color.FromArgb("BBBBBB")
            );
            weekDayLabel.SetBinding(
                Label.TextProperty,
                new MultiBinding
                {
                    Bindings = new Collection<BindingBase>
                    {
                        new Binding(
                            $"{nameof(viewModel.SelectedGame)}.{nameof(viewModel.SelectedGame.Timing)}.{nameof(viewModel.SelectedGame.Timing.WeekdayOpenTime)}",
                            stringFormat: "{0: HH:mm}"
                        ),
                        new Binding(
                            $"{nameof(viewModel.SelectedGame)}.{nameof(viewModel.SelectedGame.Timing)}.{nameof(viewModel.SelectedGame.Timing.WeekdayCloseTime)}",
                            stringFormat: "{0: HH:mm}"
                        ),
                    },
                    StringFormat = "{0} - {1}",
                }
            );

            SemiBold14Label weekEndLabel = new SemiBold14Label { }.AppThemeBinding(
                SemiBold14Label.TextColorProperty,
                Color.FromArgb("#626262"),
                Color.FromArgb("BBBBBB")
            );
            weekEndLabel.SetBinding(
                Label.TextProperty,
                new MultiBinding
                {
                    Bindings = new Collection<BindingBase>
                    {
                        new Binding(
                            $"{nameof(viewModel.SelectedGame)}.{nameof(viewModel.SelectedGame.Timing)}.{nameof(viewModel.SelectedGame.Timing.WeekendOpenTime)}",
                            stringFormat: "{0: HH:mm}"
                        ),
                        new Binding(
                            $"{nameof(viewModel.SelectedGame)}.{nameof(viewModel.SelectedGame.Timing)}.{nameof(viewModel.SelectedGame.Timing.WeekendCloseTime)}",
                            stringFormat: "{0: HH:mm}"
                        ),
                    },
                    StringFormat = "{0} - {1}",
                }
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
                    .BindCommand(nameof(viewModel.NavigateBackCommand))
                    .Column(BodyColumn.first)
            );

            base.BaseContentGrid.Children.Add(
                new ScrollView
                {
                    Padding = new Thickness(36, 20, 36, 64),
                    Content = new Grid
                    {
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
                            (BodyRow.eleventh, Auto),
                            (BodyRow.twelfth, Auto),
                            (BodyRow.thirteenth, Auto),
                            (BodyRow.fourteenth, Auto)
                        ),

                        ColumnDefinitions = Columns.Define(
                            (BodyColumn.first, Auto),
                            (BodyColumn.second, Star),
                            (BodyColumn.third, Auto)
                        ),

                        Children =
                        {
                            new Medium14Label { }
                                .AppThemeBinding(
                                    Medium14Label.TextColorProperty,
                                    Color.FromArgb("#626262"),
                                    Color.FromArgb("BBBBBB")
                                )
                                .Bind(
                                    Medium14Label.TextProperty,
                                    $"{nameof(viewModel.SelectedVenue)}.{nameof(viewModel.SelectedVenue.SelectedGamesName)}"
                                )
                                .Row(BodyRow.first)
                                .Column(BodyColumn.first)
                                .ColumnSpan(3),
                            new Bold28Label { }
                                .Bind(
                                    Bold28Label.TextProperty,
                                    $"{nameof(viewModel.SelectedVenue)}.{nameof(viewModel.SelectedVenue.FullName)}"
                                )
                                .Row(BodyRow.second)
                                .Column(BodyColumn.first)
                                .ColumnSpan(3),
                            new Medium12Label { }
                                .AppThemeBinding(
                                    Medium12Label.TextColorProperty,
                                    Color.FromArgb("#626262"),
                                    Color.FromArgb("BBBBBB")
                                )
                                .Bind(
                                    Medium12Label.TextProperty,
                                    $"{nameof(viewModel.SelectedVenue)}.{nameof(viewModel.SelectedVenue.FullAddress)}",
                                    BindingMode.OneWay
                                )
                                .Row(BodyRow.third)
                                .Column(BodyColumn.first)
                                .ColumnSpan(3),
                            new CarouselView
                            {
                                HeightRequest = 200,
                                ItemTemplate = new DataTemplate(() =>
                                {
                                    return new Border
                                    {
                                        StrokeThickness = 0,
                                        StrokeShape = new RoundRectangle { CornerRadius = 8 },
                                        Padding = 0,
                                        Content = new Image
                                        {
                                            HeightRequest = 200,
                                            Aspect = Aspect.AspectFill,
                                        }.Bind(
                                            Image.SourceProperty,
                                            ".",
                                            converter: new ByteArrayToImageSourceConverter()
                                        ),
                                    };
                                }),
                                ItemsLayout = new LinearItemsLayout(
                                    ItemsLayoutOrientation.Horizontal
                                )
                                {
                                    SnapPointsAlignment = SnapPointsAlignment.Center,
                                    SnapPointsType = SnapPointsType.MandatorySingle,
                                    ItemSpacing = 0,
                                },

                                IndicatorView = indicatorView,
                            }
                                .Bind(
                                    ItemsView.ItemsSourceProperty,
                                    $"{nameof(viewModel.SelectedVenue)}.{nameof(viewModel.SelectedVenue.SelectedGamesImages)}"
                                )
                                .Row(BodyRow.fourth)
                                .Margins(0, 10, 0, 0)
                                .Column(BodyColumn.first)
                                .ColumnSpan(3),
                            indicatorView
                                .Row(BodyRow.fifth)
                                .Column(BodyColumn.first)
                                .ColumnSpan(3)
                                .Margins(0, 10, 0, 0),
                            //  new Border
                            //  {
                            //         StrokeThickness = 0,
                            //         StrokeShape = new RoundRectangle { CornerRadius = 8 },
                            //         Padding = 0,
                            //         Content=new Image {HeightRequest=200, Aspect= Aspect.AspectFill }.Bind(Image.SourceProperty,  $"{nameof(viewModel.SelectedVenue)}.{nameof(viewModel.SelectedVenue.SelectedGamesImage)}", converter: new ByteArrayToImageSourceConverter(), source:viewModel)
                            //  }.Row(BodyRow.fourth).Margins(0,10,0,0).Column(BodyColumn.first).ColumnSpan(3),

                            new Medium12Label
                            {
                                Text = "Weekday",
                                TextColor = Color.FromArgb("#EF2F50"),
                            }
                                .Margins(0, 15, 0, 0)
                                .Row(BodyRow.fifth)
                                .Column(BodyColumn.first),
                            weekDayLabel.Row(BodyRow.sixth).Column(BodyRow.first)
                            .Margins(0,0,0,10),
                            new Medium12Label
                            {
                                Text = "Weekend",
                                TextColor = Color.FromArgb("#EF2F50"),
                            }
                                .Margins(0, 15, 0, 0)
                                .Row(BodyRow.fifth)
                                .Column(BodyColumn.third),
                            weekEndLabel.Row(BodyRow.sixth).Column(BodyColumn.third)
                            .Margins(0,0,0,10),
                            // new SemiBold14Label { FormattedText = weekDayFormattedString }
                            //     .AppThemeBinding(
                            //         SemiBold14Label.TextColorProperty,
                            //         Color.FromArgb("#626262"),
                            //         Color.FromArgb("BBBBBB")
                            //     )
                            //     .Margins(0, 0, 0, 10)
                            //     .Row(BodyRow.seventh)
                            //     .Column(BodyColumn.first),
                            // new SemiBold14Label { FormattedText = weekEndFormattedString }
                            //     .AppThemeBinding(
                            //         SemiBold14Label.TextColorProperty,
                            //         Color.FromArgb("#626262"),
                            //         Color.FromArgb("BBBBBB")
                            //     )
                            //     .Margins(0, 0, 0, 10)
                            //     .Row(BodyRow.seventh)
                            //     .Column(BodyColumn.third),
                            new Rectangle
                            {
                                HeightRequest = 2,
                                Background = Color.FromArgb("4E4E4E"),
                            }
                                .Row(BodyRow.eighth)
                                .Column(BodyColumn.first)
                                .ColumnSpan(3),
                            new SemiBold13Label { Text = "About Venue" }
                                .Margins(0, 7, 0, 0)
                                .Row(BodyRow.ninth)
                                .Column(BodyColumn.first)
                                .ColumnSpan(3),
                            new Medium12Label { }
                                .Margins(0, 0, 0, 7)
                                .Bind(
                                    Medium12Label.TextProperty,
                                    nameof(viewModel.AboutVenue),
                                    source: viewModel
                                )
                                .Row(BodyRow.tenth)
                                .Column(BodyColumn.first)
                                .ColumnSpan(3),
                            new Rectangle
                            {
                                HeightRequest = 2,
                                Background = Color.FromArgb("4E4E4E"),
                            }
                                .Row(BodyRow.eleventh)
                                .Column(BodyColumn.first)
                                .ColumnSpan(3),
                            new SemiBold13Label { Text = "Amenities" }
                                .AppThemeBinding(
                                    SemiBold13Label.TextColorProperty,
                                    Color.FromArgb("#626262"),
                                    Color.FromArgb("BBBBBB")
                                )
                                .Margins(0, 5, 0, 0)
                                .Row(BodyRow.twelfth)
                                .Column(BodyColumn.first)
                                .ColumnSpan(3),
                            amenitiesCollection
                                .Row(BodyRow.thirteenth)
                                .Column(BodyColumn.first)
                                .ColumnSpan(3),
                            new MediumButton
                            {
                                Text = "Book Now",
                                HeightRequest = 40,
                                WidthRequest = 316,
                                Padding = new Thickness(2),
                            }
                                .BindCommand(nameof(viewModel.BookNowCommand), source: viewModel)
                                .Margins(0, 25, 0, 0)
                                .Row(BodyRow.fourteenth)
                                .Column(BodyColumn.first)
                                .ColumnSpan(3),
                        },
                    },
                }
                    .Row(BodyRow.second)
                    .ColumnSpan(2)
            );
        }
        catch (Exception ex)
        {
            Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
    }
}
