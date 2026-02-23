namespace Player.Views;

public class DashboardView : BaseView
{
    private readonly DashboardViewModel? viewModel;

    public DashboardView(DashboardViewModel viewModel)
        : base(viewModel)
    {
        try
        {
            this.viewModel = viewModel;

            Style borderStyle = new Style(typeof(Border))
            {
                Setters =
                {
                    new Setter { Property = PaddingProperty, Value = 0 },
                    new Setter { Property = Border.StrokeThicknessProperty, Value = 0 },
                    new Setter
                    {
                        Property = Border.StrokeShapeProperty,
                        Value = new RoundRectangle { CornerRadius = new CornerRadius(10) },
                    },
                },
            };

            Style verticalStackLayoutStyle = new Style(typeof(VerticalStackLayout))
            {
                Setters =
                {
                    new Setter { Property = VerticalStackLayout.SpacingProperty, Value = 5 },
                    new Setter
                    {
                        Property = VerticalStackLayout.VerticalOptionsProperty,
                        Value = LayoutOptions.Center,
                    },
                    new Setter { Property = VerticalStackLayout.WidthRequestProperty, Value = 100 },
                },
            };

            Resources.Add(borderStyle);
            Resources.Add(verticalStackLayoutStyle);

            base.TopGrid.Children.Add(
                new ImageButton
                {
                    HorizontalOptions = LayoutOptions.Start,
                    VerticalOptions = LayoutOptions.Start,
                }.AppThemeBinding(ImageButton.SourceProperty, "dome_light_theme", "dome_dark_theme")
            );

            base.BaseContentGrid.Add(
                new ScrollView
                {
                    Padding = new Thickness(16, 10, 16, 21),
                    Content = new Grid
                    {
                        RowDefinitions = Rows.Define(
                            (BodyRow.first, Auto),
                            (BodyRow.second, Auto),
                            (BodyRow.third, Auto),
                            (BodyRow.fourth, 210),
                            (BodyRow.fifth, Auto),
                            (BodyRow.sixth, Auto),
                            (BodyRow.seventh, Auto),
                            (BodyRow.eighth, Auto),
                            (BodyRow.ninth, Auto),
                            (BodyRow.tenth, Auto)
                        ),
                        ColumnDefinitions = Columns.Define(
                            (BodyColumn.first, Star),
                            (BodyColumn.second, Star)
                        ),

                        ColumnSpacing = 15,
                        Children =
                        {
                            new ExtraBold28Label { }
                                .Margins(16, 0, 0, 0)
                                .Row(BodyRow.first)
                                .ColumnSpan(2)
                                .Bind(
                                    Bold28Label.TextProperty,
                                    nameof(viewModel.UserName),
                                    stringFormat: "Hey {0}"
                                ),
                            new Border
                            {
                                Padding = new Thickness(17, 20),
                                Content = new Grid
                                {
                                    RowDefinitions = Rows.Define(
                                        (BodyRow.first, Auto),
                                        (BodyRow.second, Auto),
                                        (BodyRow.third, Auto),
                                        (BodyRow.fourth, Auto),
                                        (BodyRow.fifth, 2),
                                        (BodyRow.sixth, Auto)
                                    ),
                                    ColumnDefinitions = Columns.Define(
                                        (BodyColumn.first, Star),
                                        (BodyColumn.second, Auto)
                                    ),

                                    ColumnSpacing = 5,
                                    RowSpacing = 5,
                                    Children =
                                    {
                                        new Border
                                        {
                                            HorizontalOptions = LayoutOptions.Start,
                                            Padding = new Thickness(10, 5),
                                            Content = new Medium8Label
                                            {
                                                Text = "PLAY NOW",
                                                TextColor = Colors.White,
                                            },
                                        }
                                            .AppThemeBinding(
                                                Border.BackgroundProperty,
                                                Colors.Black,
                                                Color.FromArgb("#5D5D5D")
                                            )
                                            .Row(BodyRow.first)
                                            .Column(BodyColumn.first),
                                        new ExtraBold18Label { Text = "Book Now to Play" }
                                            .Row(BodyRow.second)
                                            .Column(BodyColumn.first),
                                        new Medium12Label
                                        {
                                            Text = "Archery | Badminton | Basketball | Golf",
                                        }
                                            .Row(BodyRow.third)
                                            .Column(BodyColumn.first),
                                        new Image
                                        {
                                            Source = "more.png",
                                            HorizontalOptions = LayoutOptions.Start,
                                        }
                                            .Row(BodyRow.fourth)
                                            .Column(BodyColumn.first)
                                            .Margins(0, 0, 0, 10),
                                        new Rectangle
                                        {
                                            HorizontalOptions = LayoutOptions.Fill,
                                            HeightRequest = 2,
                                            Background = Colors.Gray,
                                        }
                                            .Row(BodyRow.fifth)
                                            .ColumnSpan(2),
                                        new MediumButton
                                        {
                                            Text = "BOOK ",
                                            VerticalOptions = LayoutOptions.Center,
                                            FontSize = 11,
                                            WidthRequest = 74,
                                            Padding = new Thickness(20, 5),
                                        }
                                            .Row(BodyRow.second)
                                            .Column(BodyColumn.second)
                                            .BindCommand(nameof(viewModel.NavigateToPlayCommand)),
                                        new Button
                                        {
                                            Text = "My Bookings",
                                            Background = Colors.Transparent,
                                            FontSize = 13,
                                            FontFamily = "InterMedium",
                                            HorizontalOptions = LayoutOptions.Center,
                                            VerticalOptions = LayoutOptions.Start,
                                            Padding = 0,
                                        }
                                            .AppThemeBinding(
                                                Button.TextColorProperty,
                                                Colors.Black,
                                                Colors.White
                                            )
                                            .BindCommand(
                                                nameof(viewModel.NavigateToMyBookingsViewCommand)
                                            )
                                            .Row(BodyRow.sixth)
                                            .ColumnSpan(2)
                                            .Margins(0, 5, 0, 0),
                                    },
                                },
                            }
                                .AppThemeBinding(
                                    Border.BackgroundProperty,
                                    Colors.White,
                                    Color.FromArgb("#23262A")
                                )
                                .Row(BodyRow.second)
                                .ColumnSpan(2)
                                .Margins(0, 15, 0, 0),
                            new Border
                            {
                                StrokeThickness = 0,
                                StrokeShape = new RoundRectangle
                                {
                                    CornerRadius = new CornerRadius(7),
                                },
                                Padding = new Thickness(0, 10, 0, 15),
                                Content = new Grid
                                {
                                    RowDefinitions = Rows.Define(
                                        (BodyRow.first, Auto),
                                        (BodyRow.second, Auto)
                                    ),

                                    RowSpacing = 5,
                                    ColumnSpacing = 10,
                                    Children =
                                    {
                                        new SemiBold12Label
                                        {
                                            HorizontalOptions = LayoutOptions.Start,
                                            Text = "Game Yard",
                                        }
                                            .Row(BodyRow.first)
                                            .Margins(15, 0, 0, 0),
                                        new ScrollView
                                        {
                                            Orientation = ScrollOrientation.Horizontal,
                                            HorizontalScrollBarVisibility =
                                                ScrollBarVisibility.Always,
                                            Background = Colors.Transparent,
                                            Content = new Grid
                                            {
                                                RowDefinitions = Rows.Define((BodyRow.first, Auto)),

                                                ColumnDefinitions = Columns.Define(
                                                    (BodyColumn.first, Star),
                                                    (BodyColumn.second, Star),
                                                    (BodyColumn.third, Star)
                                                ),

                                                RowSpacing = 5,
                                                ColumnSpacing = 10,
                                                Children =
                                                {
                                                    new Border
                                                    {
                                                        StrokeThickness = 0,
                                                        StrokeShape = new RoundRectangle
                                                        {
                                                            CornerRadius = new CornerRadius(7),
                                                        },
                                                        Padding = new Thickness(0),
                                                        Background = Colors.Transparent,
                                                        Content = new Image
                                                        {
                                                            Source = "coach_banner.png",

                                                            Aspect = Aspect.AspectFill,
                                                        },
                                                    }
                                                        .BindTapGesture(
                                                            nameof(
                                                                viewModel.NavigateToCoachViewCommand
                                                            )
                                                        )
                                                        .Row(BodyRow.second)
                                                        .Column(BodyColumn.first),
                                                    new Border
                                                    {
                                                        StrokeThickness = 0,
                                                        StrokeShape = new RoundRectangle
                                                        {
                                                            CornerRadius = new CornerRadius(7),
                                                        },
                                                        Padding = new Thickness(0),
                                                        Background = Colors.Transparent,
                                                        Content = new Image
                                                        {
                                                            Source = "learn_banner.png",

                                                            Aspect = Aspect.AspectFit,
                                                        }.BindTapGesture(
                                                            nameof(
                                                                viewModel.NavigateToLearnViewCommand
                                                            )
                                                        ),
                                                    }
                                                        .Row(BodyRow.second)
                                                        .Column(BodyColumn.second),
                                                    new Border
                                                    {
                                                        StrokeThickness = 0,
                                                        StrokeShape = new RoundRectangle
                                                        {
                                                            CornerRadius = new CornerRadius(7),
                                                        },
                                                        Padding = new Thickness(0),
                                                        Background = Colors.Transparent,
                                                        Content = new Image
                                                        {
                                                            Source = "connect_banner.png",

                                                            Aspect = Aspect.AspectFill,
                                                        },
                                                    }
                                                        .BindTapGesture(
                                                            nameof(
                                                                viewModel.NavigateToConnectViewCommand
                                                            )
                                                        )
                                                        .Row(BodyRow.second)
                                                        .Column(BodyColumn.third),
                                                },
                                            },
                                        }.Row(BodyRow.second),
                                    },
                                },
                            }
                                .AppThemeBinding(
                                    Border.BackgroundProperty,
                                    Colors.White,
                                    Color.FromArgb("#23262A")
                                )
                                .Row(BodyRow.third)
                                .ColumnSpan(2)
                                .Margins(0, 15, 0, 0),
                            new Image { Source = "coupon.png" }
                                .Row(BodyRow.fourth)
                                .ColumnSpan(2)
                                .Margins(0, 15, 0, 0),
                            new Bold16Label { Text = "Know More About Us!" }
                                .Row(BodyRow.fifth)
                                .ColumnSpan(2)
                                .Margins(0, 15, 0, 0),
                            new Border
                            {
                                Padding = new Thickness(15, 10, 10, 10),
                                Content = new Grid
                                {
                                    HorizontalOptions = LayoutOptions.Fill,
                                    VerticalOptions = LayoutOptions.Center,
                                    RowDefinitions = Rows.Define(
                                        (BodyRow.first, Auto),
                                        (BodyRow.second, Auto)
                                    ),
                                    ColumnDefinitions = Columns.Define(
                                        (BodyColumn.first, Auto),
                                        (BodyColumn.second, Star)
                                    ),

                                    ColumnSpacing = 20,

                                    Children =
                                    {
                                        new Image { HeightRequest = 26, WidthRequest = 28 }
                                            .AppThemeBinding(
                                                Image.SourceProperty,
                                                "connect_black.png",
                                                "connect.png"
                                            )
                                            .Row(BodyRow.first)
                                            .RowSpan(2),
                                        new ExtraBold16Label
                                        {
                                            HorizontalOptions = LayoutOptions.Start,
                                            VerticalOptions = LayoutOptions.End,
                                            Text = "Connect",
                                        }
                                            .Row(BodyRow.first)
                                            .Column(BodyColumn.second),
                                        new Regular10Label
                                        {
                                            HorizontalOptions = LayoutOptions.Start,
                                            VerticalOptions = LayoutOptions.Start,
                                            Text = "Build Community",
                                        }
                                            .Row(BodyRow.second)
                                            .Column(BodyColumn.second),
                                    },
                                },
                            }
                                .AppThemeBinding(
                                    Border.BackgroundProperty,
                                    Colors.White,
                                    Color.FromArgb("#23262A")
                                )
                                .Row(BodyRow.sixth)
                                .Column(BodyColumn.first)
                                .Margins(0, 10, 0, 0)
                                .BindTapGesture(
                                    nameof(viewModel.NavigateToConnectViewCommand),
                                    commandSource: viewModel
                                ),
                            new Border
                            {
                                Padding = new Thickness(15, 10, 10, 10),
                                Content = new Grid
                                {
                                    HorizontalOptions = LayoutOptions.Fill,
                                    VerticalOptions = LayoutOptions.Center,
                                    RowDefinitions = Rows.Define(
                                        (BodyRow.first, Auto),
                                        (BodyRow.second, Auto)
                                    ),
                                    ColumnDefinitions = Columns.Define(
                                        (BodyColumn.first, Auto),
                                        (BodyColumn.second, Star)
                                    ),

                                    ColumnSpacing = 20,
                                    Children =
                                    {
                                        new Image
                                        {
                                            HorizontalOptions = LayoutOptions.Center,
                                            HeightRequest = 23,
                                            WidthRequest = 26,
                                        }
                                            .AppThemeBinding(
                                                Image.SourceProperty,
                                                "learn_black.png",
                                                "learn.png"
                                            )
                                            .Row(BodyRow.first)
                                            .RowSpan(2),
                                        new ExtraBold16Label
                                        {
                                            HorizontalOptions = LayoutOptions.Start,
                                            VerticalOptions = LayoutOptions.End,
                                            Text = "Learn",
                                        }
                                            .Row(BodyRow.first)
                                            .Column(BodyColumn.second),
                                        new Regular10Label
                                        {
                                            HorizontalOptions = LayoutOptions.Start,
                                            VerticalOptions = LayoutOptions.Start,
                                            Text = "Up Your Game",
                                        }
                                            .Row(BodyRow.second)
                                            .Column(BodyColumn.second),
                                    },
                                },
                            }
                                .AppThemeBinding(
                                    Border.BackgroundProperty,
                                    Colors.White,
                                    Color.FromArgb("#23262A")
                                )
                                .Row(BodyRow.sixth)
                                .Column(BodyColumn.second)
                                .Margins(0, 10, 0, 0)
                                .BindTapGesture(
                                    nameof(viewModel.NavigateToLearnViewCommand),
                                    commandSource: viewModel
                                ),
                            new Image
                            {
                                HorizontalOptions = LayoutOptions.Center,
                                HeightRequest = 98,
                                WidthRequest = 98,
                            }
                                .AppThemeBinding(
                                    Image.SourceProperty,
                                    "dome_logo.png",
                                    "dome_logo_white.png"
                                )
                                .Row(BodyRow.seventh)
                                .ColumnSpan(2)
                                .Margins(0, 20, 0, 0),
                            new Regular12Label
                            {
                                Text = "Play Your Sport Everyday",
                                HorizontalOptions = LayoutOptions.Center,
                            }
                                .Row(BodyRow.eighth)
                                .ColumnSpan(2),
                            new Regular10Label { HorizontalOptions = LayoutOptions.Center }
                                .Row(BodyRow.ninth)
                                .ColumnSpan(2)
                                .Bind(
                                    Regular10Label.TextProperty,
                                    nameof(viewModel.CurrentVersion),
                                    stringFormat: "V {0}"
                                ),
                        },
                    },
                }
                    .Row(BodyRow.second)
                    .ColumnSpan(2)
            );

            Loaded += DashboardView_Loaded;
        }
        catch (Exception ex)
        {
            Shell.Current.DisplayAlert("Error", ex.Message, "Okay");
        }
    }

    private async void DashboardView_Loaded(object? sender, EventArgs e)
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
