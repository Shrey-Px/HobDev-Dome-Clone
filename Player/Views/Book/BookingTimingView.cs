namespace Player.Views.Book;

public class BookingTimingView : BaseView
{
    BookingTimingViewModel viewModel;

    CollectionView FieldsView;

    public BookingTimingView(BookingTimingViewModel viewModel)
        : base(viewModel)
    {
        try
        {
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

            Style<Border> fieldStyle = new Style<Border>(
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
                                        new Setter
                                        {
                                            Property = Border.StrokeThicknessProperty,
                                            Value = 0,
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
                                            Value = Colors.Transparent,
                                        },
                                        new Setter
                                        {
                                            Property = Border.StrokeThicknessProperty,
                                            Value = .5,
                                        },
                                    },
                                },
                            },
                        },
                    }
                )
            );
            this.viewModel = viewModel;
            this.AppThemeBinding(ContentPage.BackgroundProperty, Colors.White, Colors.Black);
            DataTemplate datesTemplate = new DataTemplate(() =>
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
            });

            CollectionView DatesCollection = new CollectionView
            {
                HeightRequest = 70,
                SelectionMode = SelectionMode.Single,
                ItemsLayout = new LinearItemsLayout(ItemsLayoutOrientation.Horizontal)
                {
                    ItemSpacing = 5,
                },
                ItemTemplate = datesTemplate,
                SelectionChangedCommand = viewModel.DateSelectedCommand,
            };
            DatesCollection.Bind(ItemsView.ItemsSourceProperty, nameof(viewModel.VenueDates));
            DatesCollection.Bind(
                SelectableItemsView.SelectedItemProperty,
                nameof(viewModel.SelectedDate)
            );

            DataTemplate fieldsTemplate = new DataTemplate(() =>
            {
                return new Border
                {
                    Style = fieldStyle,
                    StrokeShape = new RoundRectangle { CornerRadius = 10 },
                    Padding = new Thickness(7, 5),
                    Content = new Light14Label
                    {
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.Center,
                    }
                        .Bind(
                            Light14Label.TextProperty,
                            nameof(Field.FieldName),
                            stringFormat: "{0:mm}"
                        )
                        .Bind(
                            Light14Label.TextColorProperty,
                            nameof(Field.IsSelected),
                            convert: (bool isSelected) =>
                            {
                                if (isSelected)
                                {
                                    return Colors.White;
                                }
                                else
                                {
                                    if (App.Current.UserAppTheme == AppTheme.Dark)
                                    {
                                        return Colors.White;
                                    }
                                    else if (App.Current.UserAppTheme == AppTheme.Light)
                                    {
                                        return Colors.Black;
                                    }
                                }
                                return Colors.Black;
                            }
                        ),
                }.AppThemeBinding(Border.StrokeProperty, Brush.Black, Brush.LightGray);
            });

            FieldsView = new CollectionView
            {
                SelectionMode = SelectionMode.Multiple,
                ItemTemplate = fieldsTemplate,
                SelectionChangedCommand = viewModel.FieldSelectedCommand,
                ItemsLayout = new GridItemsLayout(3, ItemsLayoutOrientation.Vertical)
                {
                    HorizontalItemSpacing = 10,
                    VerticalItemSpacing = 10,
                },
            };

            FieldsView.Bind(ItemsView.ItemsSourceProperty, nameof(viewModel.AvailableFields));
            FieldsView.Bind(
                SelectableItemsView.SelectedItemsProperty,
                nameof(viewModel.SelectedFields),
                BindingMode.TwoWay,
                source: viewModel
            );

            // empty view set to a label is not visible
            // FieldsView.EmptyView =new Regular16Label {Text= "All fields are full in the selected date and time" } ;

            // empty view set to a string is visible
            FieldsView.EmptyView = "No Available slots for selected date and time";

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
                            (BodyRow.thirteenth, Auto)
                        ),
                        ColumnDefinitions = Columns.Define(
                            (BodyColumn.first, Star),
                            (BodyColumn.second, Auto),
                            (BodyColumn.third, Star)
                        ),

                        Children =
                        {
                            new Medium14Label { }
                                .AppThemeBinding(
                                    Medium14Label.TextColorProperty,
                                    Color.FromArgb("#626262"),
                                    Color.FromArgb("BBBBBB")
                                )
                                .Bind(Label.TextProperty, nameof(viewModel.GameName))
                                .Margins(34, 20, 20, 6)
                                .Row(BodyRow.first)
                                .Column(BodyColumn.first)
                                .ColumnSpan(3),
                            new Bold28Label { }
                                .Bind(
                                    Label.TextProperty,
                                    $"{nameof(viewModel.SelectedVenue)}.{nameof(viewModel.SelectedVenue.FullName)}"
                                )
                                .Margins(34, 0, 44, 2)
                                .Row(BodyRow.second)
                                .Column(BodyColumn.first)
                                .ColumnSpan(3),
                            new Medium12Label { }
                                .AppThemeBinding(
                                    Medium14Label.TextColorProperty,
                                    Color.FromArgb("#626262"),
                                    Color.FromArgb("BBBBBB")
                                )
                                .Bind(
                                    Medium12Label.TextProperty,
                                    $"{nameof(viewModel.SelectedVenue)}.{nameof(viewModel.SelectedVenue.FullAddress)}",
                                    BindingMode.OneWay
                                )
                                .Margins(34, 0, 44, 13)
                                .Row(BodyRow.third)
                                .Column(BodyColumn.first)
                                .ColumnSpan(3),
                            new Rectangle
                            {
                                Background = Color.FromArgb("4E4E4E"),
                                HeightRequest = 2,
                            }
                                .Row(BodyRow.fourth)
                                .Column(BodyColumn.first)
                                .ColumnSpan(3),
                            new Bold14Label { TextColor = Color.FromArgb("#EF2F50") }
                                .Bind(
                                    Label.TextProperty,
                                    $"{nameof(viewModel.SelectedDate)}.{nameof(viewModel.SelectedDate.Date)}",
                                    stringFormat: "{0:dddd dd MMMM yyyy}"
                                )
                                .Margins(34, 16, 44, 0)
                                .Row(BodyRow.fifth)
                                .Column(BodyColumn.first)
                                .ColumnSpan(3),
                            DatesCollection
                                .Row(BodyRow.sixth)
                                .Column(BodyColumn.first)
                                .ColumnSpan(3)
                                .Margins(34, 16, 0, 28),
                            new Rectangle
                            {
                                Background = Color.FromArgb("4E4E4E"),
                                HeightRequest = 2,
                            }
                                .Row(BodyRow.seventh)
                                .Column(BodyColumn.first)
                                .ColumnSpan(3),
                            new VerticalStackLayout
                            {
                                Padding = new Thickness(0, 10, 0, 25),
                                VerticalOptions = LayoutOptions.Start,
                                Spacing = 7,
                                Children =
                                {
                                    new SemiBold12Label
                                    {
                                        Text = "TIME",
                                        TextColor = Color.FromArgb("#828282"),
                                        HorizontalOptions = LayoutOptions.Center,
                                    },
                                    new SemiBold16Label
                                    {
                                        VerticalOptions = LayoutOptions.Start,
                                        HorizontalOptions = LayoutOptions.Center,
                                    }
                                        .AppThemeBinding(
                                            SemiBold16Label.TextColorProperty,
                                            Color.FromArgb("#636363"),
                                            Color.FromArgb("#D4D4D4")
                                        )
                                        .Bind(
                                            SemiBold16Label.TextProperty,
                                            $"{nameof(viewModel.SelectedStartTime)}.{nameof(viewModel.SelectedStartTime.Date)}",
                                            converter: new CommunityToolkit.Maui.Converters.DateTimeOffsetConverter(),
                                            stringFormat: "{0:HH : mm}"
                                        ),
                                },
                            }
                                .BindTapGesture(nameof(viewModel.SelectStartTimeCommand))
                                .Row(BodyRow.eighth)
                                .Column(BodyColumn.first),
                            new Rectangle
                            {
                                Background = Color.FromArgb("4E4E4E"),
                                WidthRequest = 2,
                                VerticalOptions = LayoutOptions.Fill,
                            }
                                .Row(BodyRow.eighth)
                                .Column(BodyColumn.second),
                            new VerticalStackLayout
                            {
                                Padding = new Thickness(0, 10, 0, 25),
                                VerticalOptions = LayoutOptions.Start,
                                Spacing = 7,
                                Children =
                                {
                                    new SemiBold12Label
                                    {
                                        Text = "DURATION",
                                        HorizontalOptions = LayoutOptions.Center,
                                        TextColor = Color.FromArgb("#828282"),
                                    },
                                    new HorizontalStackLayout
                                    {
                                        HorizontalOptions = LayoutOptions.Center,
                                        VerticalOptions = LayoutOptions.Start,
                                        Spacing = 20,
                                        Children =
                                        {
                                            new ImageButton
                                            {
                                                Source = "substract_time.png",
                                                HeightRequest = 24,
                                                WidthRequest = 23,
                                            }.BindCommand(nameof(viewModel.SubtractTimeCommand)),
                                            new SemiBold16Label
                                            {
                                                VerticalOptions = LayoutOptions.Center,
                                            }
                                                .AppThemeBinding(
                                                    SemiBold16Label.TextColorProperty,
                                                    Color.FromArgb("#636363"),
                                                    Color.FromArgb("#D4D4D4")
                                                )
                                                .Bind(
                                                    SemiBold16Label.TextProperty,
                                                    nameof(viewModel.BookingDuration),
                                                    stringFormat: "{0} min"
                                                ),
                                            new ImageButton
                                            {
                                                Source = "add_time.png",
                                                HeightRequest = 24,
                                                WidthRequest = 23,
                                            }.BindCommand(nameof(viewModel.AddTimeCommand)),
                                        },
                                    },
                                },
                            }
                                .Row(BodyRow.eighth)
                                .Column(BodyColumn.third),
                            new Rectangle
                            {
                                Background = Color.FromArgb("4E4E4E"),
                                HeightRequest = 2,
                                HorizontalOptions = LayoutOptions.Fill,
                            }
                                .Row(BodyRow.ninth)
                                .Column(BodyColumn.first)
                                .ColumnSpan(3),
                            FieldsView
                                .Margins(40, 16, 45, 20)
                                .Row(BodyRow.tenth)
                                .Column(BodyColumn.first)
                                .ColumnSpan(3),
                            new Rectangle
                            {
                                Background = Color.FromArgb("4E4E4E"),
                                HeightRequest = 2,
                                HorizontalOptions = LayoutOptions.Fill,
                            }
                                .Row(BodyRow.eleventh)
                                .Column(BodyColumn.first)
                                .ColumnSpan(3),
                            new MediumButton
                            {
                                Text = "Next",
                                HeightRequest = 40,
                                Padding = new Thickness(2),
                            }
                                .BindCommand(nameof(viewModel.BookNowCommand), source: viewModel)
                                .Margins(37, 45, 45, 15)
                                .Row(BodyRow.twelfth)
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
