using System.Threading.Tasks;

namespace Admin.Popups;

public class TimePopup : Popup<DateTimeOffset>
{
    int hour = 0;
    int minute = 0;

    public TimePopup()
    {
        try
        {
            Resources.Add(
                new Style<Border>(
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
                                                Value = Colors.Gray,
                                            },
                                        },
                                    },
                                },
                            },
                        }
                    ),
                    (Border.StrokeThicknessProperty, 0),
                    (Border.PaddingProperty, new Thickness(8, 4)),
                    (
                        Border.StrokeShapeProperty,
                        new RoundRectangle { CornerRadius = new CornerRadius(10) }
                    )
                )
            );

            // Color = Color.FromArgb("#F3F4F6");

            WidthRequest = 420;
            HeightRequest = 600;

            ObservableCollection<LocalTimeData> HourList = new ObservableCollection<LocalTimeData>
            {
                new LocalTimeData(time: 0),
                new LocalTimeData(time: 1),
                new LocalTimeData(time: 2),
                new LocalTimeData(time: 3),
                new LocalTimeData(time: 4),
                new LocalTimeData(time: 5),
                new LocalTimeData(time: 6),
                new LocalTimeData(time: 7),
                new LocalTimeData(time: 8),
                new LocalTimeData(time: 9),
                new LocalTimeData(time: 10),
                new LocalTimeData(time: 11),
                new LocalTimeData(time: 12),
                new LocalTimeData(time: 13),
                new LocalTimeData(time: 14),
                new LocalTimeData(time: 15),
                new LocalTimeData(time: 16),
                new LocalTimeData(time: 17),
                new LocalTimeData(time: 18),
                new LocalTimeData(time: 19),
                new LocalTimeData(time: 20),
                new LocalTimeData(time: 21),
                new LocalTimeData(time: 22),
                new LocalTimeData(time: 23),
            };

            CollectionView hoursCollection = new CollectionView
            {
                ItemsSource = HourList,
                SelectionMode = SelectionMode.Single,
                ItemsLayout = new GridItemsLayout(6, ItemsLayoutOrientation.Vertical)
                {
                    HorizontalItemSpacing = 5,
                    VerticalItemSpacing = 5,
                },
                ItemTemplate = new DataTemplate(() =>
                {
                    return new Border
                    {
                        Content = new Regular18Label { HorizontalOptions = LayoutOptions.Center }
                            .Bind(Regular18Label.TextProperty, static (LocalTimeData data) => data.Time)
                            .AppThemeBinding(
                                Regular16Label.TextColorProperty,
                                Colors.Black,
                                Colors.White
                            ),
                    };
                }),
            };
            hoursCollection.SelectedItem = HourList[0];

            //hoursCollection.SelectionChanged -= HoursCollection_SelectionChanged;
            hoursCollection.SelectionChanged += HoursCollection_SelectionChanged;

            ObservableCollection<LocalTimeData> MinuteList = new ObservableCollection<LocalTimeData>
            {
                new LocalTimeData(time: 0),
                new LocalTimeData(time: 30),
            };

            CollectionView minutesCollection = new CollectionView
            {
                // ItemSizingStrategy = ItemSizingStrategy.MeasureFirstItem,
                SelectionMode = SelectionMode.Single,
                HorizontalOptions = LayoutOptions.Start,
                ItemsLayout = new GridItemsLayout(1, ItemsLayoutOrientation.Vertical)
                {
                    HorizontalItemSpacing = 5,
                    VerticalItemSpacing = 5,
                },
                ItemTemplate = new DataTemplate(() =>
                {
                    return new Border
                    {
                        Content = new Regular18Label
                        {
                            HorizontalOptions = LayoutOptions.Center,
                            VerticalOptions = LayoutOptions.Center,
                        }
                            .Bind(Regular18Label.TextProperty, static (LocalTimeData data) => data.Time)
                            .AppThemeBinding(
                                Regular16Label.TextColorProperty,
                                Colors.Black,
                                Colors.White
                            ),
                    };
                }),

                ItemsSource = MinuteList,
            };
            // minutesCollection.SelectionChanged -= MinutesCollection_SelectionChanged;
            minutesCollection.SelectionChanged += MinutesCollection_SelectionChanged;
            minutesCollection.SelectedItem = MinuteList[0];

            Content = new VerticalStackLayout
            {
                Padding = new Thickness(20),
                Children =
                {
                    new Bold20Label { Text = "Select Hour" },
                    hoursCollection.Margins(0, 10, 0, 0),
                    new Bold20Label { Text = "Select Minute" }.Margins(0, 20, 0, 0),
                    minutesCollection.Margins(0, 10, 0, 0),
                    new RegularButton { Text = "DONE" }
                        .Invoke(button => button.Clicked += DoneButton_Clicked)
                        .Margins(0, 50, 0, 0),
                    new RegularButton { Text = "Cancel", Background = Colors.Black }
                        .Invoke(button => button.Clicked += CancelButton_Clicked)
                        .Margins(0, 10, 0, 0),
                },
            }.AppThemeBinding(VisualElement.BackgroundProperty, Colors.White, Colors.Black);
        }
        catch (Exception ex)
        {
            Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async void DoneButton_Clicked(object? sender, EventArgs e)
    {
        DateTimeOffset date = new DateTimeOffset(
            2020,
            2,
            2,
            hour,
            minute,
            0,
            new TimeSpan(0, 0, 0)
        );
        await CloseAsync(date);
    }

    private async void CancelButton_Clicked(object? sender, EventArgs e)
    {
        await CloseAsync();
    }

    private void HoursCollection_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        LocalTimeData? localTimeData = e.CurrentSelection.FirstOrDefault() as LocalTimeData;
        if (localTimeData != null)
        {
            hour = localTimeData.Time;
        }
    }

    private void MinutesCollection_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        LocalTimeData? localTimeData = e.CurrentSelection.FirstOrDefault() as LocalTimeData;
        if (localTimeData != null)
        {
            minute = localTimeData.Time;
        }
    }
}
