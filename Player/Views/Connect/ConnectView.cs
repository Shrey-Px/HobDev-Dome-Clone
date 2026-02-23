namespace Player.Views.Connect;

public class ConnectView : BaseView
{
    PlanBookingViewModel? planBookingViewModel;
    JoinAGameViewModel? joinAGameViewModel;

    int count = 0;

    ConnectViewModel? viewModel;

    public ConnectView(ConnectViewModel viewModel)
        : base(viewModel)
    {
        try
        {
            this.viewModel = viewModel;
            Resources.Add(
                new Style<UnderlinedTabItem>(
                    (UnderlinedTabItem.FontFamilyProperty, "InterExtraBold"),
                    (UnderlinedTabItem.UnderlineHeightProperty, 6),
                    (UnderlinedTabItem.LabelSizeProperty, 15),
                    (UnderlinedTabItem.SelectedTabColorProperty, Color.FromArgb("#EF2F50"))
                ).AddAppThemeBinding(
                    UnderlinedTabItem.UnselectedLabelColorProperty,
                    Color.FromArgb("7D7D7D"),
                    Color.FromArgb("#E0E0E0")
                )
            );

            PlanBookingView bookingForHostingView = new PlanBookingView();
            JoinAGameView joinAGameView = new JoinAGameView();
            planBookingViewModel =
                Shell.Current.Handler.MauiContext.Services.GetRequiredService<PlanBookingViewModel>();
            joinAGameViewModel =
                Shell.Current.Handler.MauiContext.Services.GetRequiredService<JoinAGameViewModel>();

            ViewSwitcher switcher = new ViewSwitcher
            {
                new DelayedView
                {
                    View = bookingForHostingView,
                    BindingContext = planBookingViewModel,
                },
                new DelayedView { View = joinAGameView, BindingContext = joinAGameViewModel },
            };

            TabHostView tabHostView = new TabHostView
            {
                HeightRequest = 50,
                Tabs =
                {
                    new UnderlinedTabItem { Label = "HOST GAME" },
                    new UnderlinedTabItem { Label = "JOIN GAME" },
                },
            }.Bind(
                TabHostView.SelectedIndexProperty,
                path: "SelectedIndex",
                BindingMode.TwoWay,
                source: switcher
            );

            base.TopGrid.Children.Add(
                new ImageButton
                {
                    HorizontalOptions = LayoutOptions.Start,
                    VerticalOptions = LayoutOptions.Start,
                }.AppThemeBinding(ImageButton.SourceProperty, "dome_light_theme", "dome_dark_theme")
            );

            base.BaseContentGrid.Children.Add(
                new Grid
                {
                    RowDefinitions = Rows.Define((BodyRow.first, Auto), (BodyRow.second, Star)),

                    Children =
                    {
                        new Border
                        {
                            StrokeThickness = 0,
                            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(7) },
                            Content = new VerticalStackLayout
                            {
                                Padding = new Thickness(18, 14, 18, 0),
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
                                    new Regular28Label { Text = "Get Connected!" },
                                    tabHostView.Margins(0, 14, 0, 0),
                                },
                            },
                        }
                            .AppThemeBinding(
                                Border.BackgroundProperty,
                                Colors.White,
                                Color.FromArgb("#23262A")
                            )
                            .Row(BodyRow.first)
                            .Margins(16, 0, 16, 0),
                        new ScrollView
                        {
                            Padding = new Thickness(16, 0, 16, 10),
                            Content = switcher,
                        }.Row(BodyRow.second),
                    },
                }
                    .Row(BodyRow.second)
                    .ColumnSpan(2)
            );

            switcher.SelectedIndex = 0;

            Loaded += async (s, e) =>
            {
                if (count == 0)
                {
                    await planBookingViewModel.Initialize();
                    await joinAGameViewModel.InitializeAsync();
                    count++;
                }
                await viewModel.LoadData();
            };
        }
        catch (Exception ex)
        {
            Shell.Current.DisplayAlert("Error", ex.Message, "Ok");
        }
    }
}
