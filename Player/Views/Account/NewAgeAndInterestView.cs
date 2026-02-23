namespace Player.Views.Account;

public class NewAgeAndInterestView : AccountBaseView
{
    NewAgeAndInterestViewModel viewModel;

    public NewAgeAndInterestView(NewAgeAndInterestViewModel viewModel)
        : base(viewModel)
    {
        try
        {
            this.viewModel = viewModel;

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
                                            Value = Colors.LightGray,
                                        },
                                    },
                                },
                            },
                        },
                    }
                )
            );

            Content = new Grid
            {
                RowDefinitions = Rows.Define(
                    (BodyRow.first, 165),
                    (BodyRow.second, 55),
                    (BodyRow.third, 150),
                    (BodyRow.fourth, 250),
                    (BodyRow.fifth, 50),
                    (BodyRow.sixth, 70)
                ),
                Children =
                {
                    new AccountTopControl().Row(BodyRow.first),
                    new ExtraBold28Label { Text = "Create Profile" }
                        .Margins(20, 20, 0, 0)
                        .Row(BodyRow.second)
                        .ColumnSpan(2),
                    new Border
                    {
                        StrokeThickness = .5,
                        StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(7) },
                        Padding = new Thickness(10, 10, 10, 20),
                        Stroke = Brush.Black,
                        Content = new VerticalStackLayout
                        {
                            Children =
                            {
                                new Medium16Label { Text = "Your Age " },
                                new CollectionView
                                {
                                    SelectionChangedCommand = viewModel.AgeGroupSelectedCommand,
                                    ItemTemplate = new DataTemplate(() =>
                                    {
                                        return new Border
                                        {
                                            Style = borderStyle,
                                            StrokeThickness = 0,
                                            StrokeShape = new RoundRectangle
                                            {
                                                CornerRadius = new CornerRadius(7),
                                            },
                                            Padding = new Thickness(10, 5),
                                            Stroke = Brush.Black,
                                            Content = new Medium14Label
                                            {
                                                VerticalOptions = LayoutOptions.Center,
                                                HorizontalOptions = LayoutOptions.Center,
                                            }
                                                .Bind(
                                                    Medium14Label.TextProperty,
                                                    nameof(AgeGroup.AgeGroupName)
                                                )
                                                .Bind(
                                                    Medium14Label.TextColorProperty,
                                                    nameof(AgeGroup.IsSelected),
                                                    convert: (bool state) =>
                                                        state == false ? Colors.Black : Colors.White
                                                ),
                                        }.Bind(
                                            Border.BackgroundProperty,
                                            nameof(AgeGroup.IsSelected),
                                            convert: (bool state) =>
                                                state == false
                                                    ? Colors.LightGray
                                                    : Color.FromArgb("#EF2F50")
                                        );
                                    }),

                                    SelectionMode = SelectionMode.Single,
                                    ItemSizingStrategy = ItemSizingStrategy.MeasureAllItems,
                                    ItemsLayout = new GridItemsLayout(
                                        4,
                                        ItemsLayoutOrientation.Vertical
                                    )
                                    {
                                        HorizontalItemSpacing = 10,
                                        VerticalItemSpacing = 10,
                                    },
                                }
                                    .Bind(
                                        ItemsView.ItemsSourceProperty,
                                        nameof(viewModel.AllAgeGroups),
                                        source: viewModel
                                    )
                                    .Bind(
                                        SelectableItemsView.SelectedItemProperty,
                                        nameof(viewModel.SelectedAgeGroup),
                                        BindingMode.TwoWay
                                    )
                                    .Margins(0, 10, 0, 0),
                                new Regular16Label { TextColor = Colors.Red }
                                    .Bind(
                                        Regular16Label.TextProperty,
                                        nameof(viewModel.SelectedAgeGroupError)
                                    )
                                    .Bind(
                                        IsVisibleProperty,
                                        nameof(viewModel.SelectedAgeGroupError),
                                        converter: new IsStringNotNullOrEmptyConverter()
                                    ),
                            },
                        },
                    }
                        .AppThemeBinding(
                            Border.BackgroundProperty,
                            Colors.White,
                            Color.FromArgb("#23262A")
                        )
                        .Row(BodyRow.third)
                        .Margins(20, 20, 20, 0),
                    new Border
                    {
                        StrokeThickness = .5,
                        StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(7) },
                        Padding = new Thickness(10, 10, 10, 20),
                        Stroke = Brush.Black,
                        Content = new VerticalStackLayout
                        {
                            Children =
                            {
                                new Medium16Label { Text = "Your Interests " },
                                new CollectionView
                                {
                                    SelectionChangedCommand = viewModel.InterestSelectedCommand,
                                    ItemTemplate = new DataTemplate(() =>
                                    {
                                        return new Border
                                        {
                                            Style = borderStyle,
                                            StrokeThickness = 0,
                                            StrokeShape = new RoundRectangle
                                            {
                                                CornerRadius = new CornerRadius(7),
                                            },
                                            Padding = new Thickness(10, 5),
                                            Stroke = Brush.Black,
                                            Content = new Medium14Label
                                            {
                                                VerticalOptions = LayoutOptions.Center,
                                                HorizontalOptions = LayoutOptions.Center,
                                            }
                                                .Bind(
                                                    Medium14Label.TextProperty,
                                                    nameof(FavouriteGame.GameName)
                                                )
                                                .Bind(
                                                    Medium14Label.TextColorProperty,
                                                    nameof(FavouriteGame.IsSelected),
                                                    convert: (bool state) =>
                                                        state == false ? Colors.Black : Colors.White
                                                ),
                                        }.Bind(
                                            Border.BackgroundProperty,
                                            nameof(FavouriteGame.IsSelected),
                                            convert: (bool state) =>
                                                state == false
                                                    ? Colors.LightGray
                                                    : Color.FromArgb("#EF2F50")
                                        );
                                    }),

                                    SelectionMode = SelectionMode.Multiple,
                                    ItemSizingStrategy = ItemSizingStrategy.MeasureAllItems,
                                    ItemsLayout = new GridItemsLayout(
                                        3,
                                        ItemsLayoutOrientation.Vertical
                                    )
                                    {
                                        HorizontalItemSpacing = 10,
                                        VerticalItemSpacing = 10,
                                    },
                                }
                                    .Bind(
                                        ItemsView.ItemsSourceProperty,
                                        nameof(viewModel.AllGames),
                                        source: viewModel
                                    )
                                    .Bind(
                                        SelectableItemsView.SelectedItemsProperty,
                                        nameof(viewModel.SelectedGames),
                                        BindingMode.TwoWay
                                    )
                                    .Margins(0, 10, 10, 0),
                                new Regular16Label { TextColor = Colors.Red }
                                    .Bind(
                                        Regular16Label.TextProperty,
                                        nameof(viewModel.SelectedGamesError)
                                    )
                                    .Bind(
                                        IsVisibleProperty,
                                        nameof(viewModel.SelectedGamesError),
                                        converter: new IsStringNotNullOrEmptyConverter()
                                    ),
                            },
                        },
                    }
                        .AppThemeBinding(
                            Border.BackgroundProperty,
                            Colors.White,
                            Color.FromArgb("#23262A")
                        )
                        .Margins(20, 20, 20, 0)
                        .Row(BodyRow.fourth),
                    new MediumButton
                    {
                        Text = "NEXT",
                        WidthRequest = 284,
                        HeightRequest = 47,
                    }
                        .Row(BodyRow.fifth)
                        .BindCommand(nameof(viewModel.SaveCommand), source: viewModel)
                        .Margins(0, 30, 0, 0),
                },
            };

            Loaded += NewAgeAndInterestView_Loaded;
        }
        catch (Exception ex)
        {
            Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async void NewAgeAndInterestView_Loaded(object? sender, EventArgs e)
    {
        await viewModel.InitializeAsync();
    }
}
