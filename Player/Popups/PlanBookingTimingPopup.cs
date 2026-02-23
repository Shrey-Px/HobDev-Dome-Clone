using System;
using Microsoft.Maui.Handlers;

namespace Player.Popups;

public class PlanBookingTimingPopup : PopupPage
{
    public PlanBookingTimingPopup(PlanBookingViewModel viewModel)
    {
        Picker startTimePicker = new Picker { IsVisible = false }
            .Bind(Picker.ItemsSourceProperty, nameof(viewModel.TimeList), source: viewModel)
            .Bind(
                Picker.SelectedItemProperty,
                nameof(viewModel.SelectedStartTime),
                source: viewModel
            );

        TapGestureRecognizer startTapGesture = new TapGestureRecognizer();
        startTapGesture.Invoke(tapGesture =>
            tapGesture.Tapped += (s, e) =>
            {
#if ANDROID
                var handler = startTimePicker.Handler as IPickerHandler;
                handler?.PlatformView.PerformClick();
#endif
                startTimePicker.Focus();
            }
        );

        Picker endTimePicker = new Picker { IsVisible = false }
            .Bind(Picker.ItemsSourceProperty, nameof(viewModel.TimeList), source: viewModel)
            .Bind(
                Picker.SelectedItemProperty,
                nameof(viewModel.SelectedEndTime),
                source: viewModel
            );

        TapGestureRecognizer endTapGesture = new TapGestureRecognizer();
        endTapGesture.Invoke(tapGesture =>
            tapGesture.Tapped += (s, e) =>
            {
#if ANDROID
                var handler = endTimePicker.Handler as IPickerHandler;
                handler?.PlatformView.PerformClick();
#endif
                endTimePicker.Focus();
            }
        );

        Content = new Grid
        {
            ColumnDefinitions = Columns.Define((BodyColumn.first, Star), (BodyColumn.second, Star)),

            RowDefinitions = Rows.Define(
                (BodyRow.first, Auto),
                (BodyRow.second, Auto),
                (BodyRow.third, Auto),
                (BodyRow.fourth, Auto),
                (BodyRow.fifth, Auto),
                (BodyRow.sixth, Auto),
                (BodyRow.seventh, Auto)
            ),
            ColumnSpacing = 20,
            Padding = new Thickness(20),

            Children =
            {
                new Bold14Label { Text = "Select time" }
                    .Margins(0, 20, 0, 0)
                    .ColumnSpan(2)
                    .Row(BodyRow.first),
                new Border
                {
                    Background = Colors.Transparent,
                    StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(8) },
                    Padding = new Thickness(10),
                    Content = new VerticalStackLayout
                    {
                        Children =
                        {
                            new Image { }
                                .Bind(
                                    Image.SourceProperty,
                                    nameof(viewModel.MorningImage),
                                    source: viewModel
                                )
                                .Center()
                                .Margins(0, 0, 0, 0),
                            new Medium8Label { Text = "Morning" }
                                .Center()
                                .Margins(0, 10, 0, 0)
                                .Bind(
                                    Medium8Label.TextColorProperty,
                                    nameof(viewModel.MorningTextColor),
                                    source: viewModel
                                ),
                            new Medium8Label { Text = "(5:00 - 12:00)" }
                                .Center()
                                .Margins(0, 0, 0, 0)
                                .Bind(
                                    Medium8Label.TextColorProperty,
                                    nameof(viewModel.MorningTextColor),
                                    source: viewModel
                                ),
                        },
                    },
                }
                    .BindTapGesture(
                        nameof(viewModel.MorningSelectedCommand),
                        commandSource: viewModel
                    )
                    .Bind(
                        Border.BackgroundProperty,
                        nameof(viewModel.Timing),
                        source: viewModel,
                        convert: (string? time) =>
                            time == PlayerConstants.morning
                                ? Color.FromArgb("#EF2F50")
                                : Colors.Transparent
                    )
                    .Bind(
                        Border.StrokeThicknessProperty,
                        nameof(viewModel.Timing),
                        source: viewModel,
                        convert: (string? time) => time == PlayerConstants.morning ? 0 : 1
                    )
                    .AppThemeBinding(Border.StrokeProperty, Brush.Black, Brush.White)
                    .Row(BodyRow.second)
                    .Column(BodyColumn.first)
                    .Margins(0, 10, 0, 0),
                new Border
                {
                    Background = Colors.Transparent,
                    StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(8) },
                    StrokeThickness = 1,
                    Padding = new Thickness(10),
                    Content = new VerticalStackLayout
                    {
                        Children =
                        {
                            new Image { }
                                .Bind(
                                    Image.SourceProperty,
                                    nameof(viewModel.AfternoonImage),
                                    source: viewModel
                                )
                                .Center()
                                .Margins(0, 0, 0, 0),
                            new Medium8Label { Text = "Afternoon" }
                                .Center()
                                .Margins(0, 10, 0, 0)
                                .Bind(
                                    Medium8Label.TextColorProperty,
                                    nameof(viewModel.AfternoonTextColor),
                                    source: viewModel
                                ),
                            new Medium8Label { Text = "(12:00 - 16:00)" }
                                .Center()
                                .Margins(0, 0, 0, 0)
                                .Bind(
                                    Medium8Label.TextColorProperty,
                                    nameof(viewModel.AfternoonTextColor),
                                    source: viewModel
                                ),
                        },
                    },
                }
                    .BindTapGesture(
                        nameof(viewModel.AfternoonSelectedCommand),
                        commandSource: viewModel
                    )
                    .Bind(
                        Border.BackgroundProperty,
                        nameof(viewModel.Timing),
                        source: viewModel,
                        convert: (string? time) =>
                            time == PlayerConstants.afternoon
                                ? Color.FromArgb("#EF2F50")
                                : Colors.Transparent
                    )
                    .Bind(
                        Border.StrokeThicknessProperty,
                        nameof(viewModel.Timing),
                        source: viewModel,
                        convert: (string? time) => time == PlayerConstants.afternoon ? 0 : 1
                    )
                    .AppThemeBinding(Border.StrokeProperty, Brush.Black, Brush.White)
                    .Row(BodyRow.second)
                    .Column(BodyColumn.second)
                    .Margins(0, 10, 0, 0),
                new Border
                {
                    Background = Colors.Transparent,
                    StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(8) },
                    StrokeThickness = 1,
                    Padding = new Thickness(10),
                    Content = new VerticalStackLayout
                    {
                        Children =
                        {
                            new Image { }
                                .Bind(
                                    Image.SourceProperty,
                                    nameof(viewModel.EveningImage),
                                    source: viewModel
                                )
                                .Center()
                                .Margins(0, 0, 0, 0),
                            new Medium8Label { Text = "Evening" }
                                .Center()
                                .Margins(0, 10, 0, 0)
                                .Bind(
                                    Medium8Label.TextColorProperty,
                                    nameof(viewModel.EveningTextColor),
                                    source: viewModel
                                ),
                            new Medium8Label { Text = "(16:00 - 23:00)" }
                                .Center()
                                .Margins(0, 0, 0, 0)
                                .Bind(
                                    Medium8Label.TextColorProperty,
                                    nameof(viewModel.EveningTextColor),
                                    source: viewModel
                                ),
                        },
                    },
                }
                    .BindTapGesture(
                        nameof(viewModel.EveningSelectedCommand),
                        commandSource: viewModel
                    )
                    .Bind(
                        Border.BackgroundProperty,
                        nameof(viewModel.Timing),
                        source: viewModel,
                        convert: (string? time) =>
                            time == PlayerConstants.evening
                                ? Color.FromArgb("#EF2F50")
                                : Colors.Transparent
                    )
                    .Bind(
                        Border.StrokeThicknessProperty,
                        nameof(viewModel.Timing),
                        source: viewModel,
                        convert: (string? time) => time == PlayerConstants.evening ? 0 : 1
                    )
                    .AppThemeBinding(Border.StrokeProperty, Brush.Black, Brush.White)
                    .Row(BodyRow.third)
                    .Column(BodyColumn.first)
                    .Margins(0, 10, 0, 0),
                new Border
                {
                    Background = Colors.Transparent,
                    StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(8) },
                    StrokeThickness = 1,
                    Padding = new Thickness(10),
                    Content = new VerticalStackLayout
                    {
                        Children =
                        {
                            new Image { }
                                .Bind(
                                    Image.SourceProperty,
                                    nameof(viewModel.NightImage),
                                    source: viewModel
                                )
                                .Center()
                                .Margins(0, 0, 0, 0),
                            new Medium8Label { Text = "Night" }
                                .Center()
                                .Margins(0, 10, 0, 0)
                                .Bind(
                                    Medium8Label.TextColorProperty,
                                    nameof(viewModel.NightTextColor),
                                    source: viewModel
                                ),
                            new Medium8Label { Text = "(23:00 - 5:00)" }
                                .Center()
                                .Margins(0, 0, 0, 0)
                                .Bind(
                                    Medium8Label.TextColorProperty,
                                    nameof(viewModel.NightTextColor),
                                    source: viewModel
                                ),
                        },
                    },
                }
                    .BindTapGesture(
                        nameof(viewModel.NightSelectedCommand),
                        commandSource: viewModel
                    )
                    .Bind(
                        Border.BackgroundProperty,
                        nameof(viewModel.Timing),
                        source: viewModel,
                        convert: (string? time) =>
                            time == PlayerConstants.night
                                ? Color.FromArgb("#EF2F50")
                                : Colors.Transparent
                    )
                    .Bind(
                        Border.StrokeThicknessProperty,
                        nameof(viewModel.Timing),
                        source: viewModel,
                        convert: (string? time) => time == PlayerConstants.night ? 0 : 1
                    )
                    .AppThemeBinding(Border.StrokeProperty, Brush.Black, Brush.White)
                    .Row(BodyRow.third)
                    .Column(BodyColumn.second)
                    .Margins(0, 10, 0, 0),
                new HorizontalStackLayout
                {
                    Spacing = 10,
                    Children =
                    {
                        new CheckBox
                        {
                            Color = Color.FromArgb("#EF2F50"),
                            HorizontalOptions = LayoutOptions.Start,
                        }.Bind(
                            CheckBox.IsCheckedProperty,
                            nameof(viewModel.IsSpecificTimeSelected),
                            source: viewModel
                        ),
                        new Bold14Label
                        {
                            Text = "Select specific time",
                            VerticalOptions = LayoutOptions.Center,
                        },
                    },
                }
                    .Margins(0, 30, 0, 0)
                    .ColumnSpan(2)
                    .Row(BodyRow.fourth),
                new Border
                {
                    StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(7) },
                    Stroke = Color.FromArgb("#828282"),
                    Background = Colors.Transparent,

                    Content = new Grid
                    {
                        ColumnDefinitions = Columns.Define(
                            (BodyColumn.first, Star),
                            (BodyColumn.second, Auto),
                            (BodyColumn.third, Star)
                        ),

                        Children =
                        {
                            new VerticalStackLayout
                            {
                                Padding = new Thickness(0, 10, 0, 20),
                                VerticalOptions = LayoutOptions.Start,
                                HorizontalOptions = LayoutOptions.Center,
                                Spacing = 7,
                                Children =
                                {
                                    new Regular12Label
                                    {
                                        Text = "Start Time",
                                        HorizontalOptions = LayoutOptions.Center,
                                    },
                                    new SemiBold16Label
                                    {
                                        HorizontalOptions = LayoutOptions.Center,
                                    }.Bind(
                                        SemiBold16Label.TextProperty,
                                        nameof(viewModel.SelectedStartTime),
                                        source: viewModel
                                    ),
                                    startTimePicker,
                                },
                            }
                                .Invoke(vertical =>
                                    vertical.GestureRecognizers.Add(startTapGesture)
                                )
                                .Column(BodyColumn.first),
                            new Rectangle
                            {
                                Background = Color.FromArgb("4E4E4E"),
                                WidthRequest = 2,
                                VerticalOptions = LayoutOptions.Fill,
                            }.Column(BodyColumn.second),
                            new VerticalStackLayout
                            {
                                Padding = new Thickness(0, 10, 0, 20),
                                VerticalOptions = LayoutOptions.Start,
                                HorizontalOptions = LayoutOptions.Center,
                                Spacing = 7,
                                Children =
                                {
                                    new Regular12Label
                                    {
                                        Text = "End Time",
                                        HorizontalOptions = LayoutOptions.Center,
                                    },
                                    new SemiBold16Label
                                    {
                                        HorizontalOptions = LayoutOptions.Center,
                                    }.Bind(
                                        SemiBold16Label.TextProperty,
                                        nameof(viewModel.SelectedEndTime),
                                        source: viewModel
                                    ),
                                },
                            }
                                .Invoke(vertical => vertical.GestureRecognizers.Add(endTapGesture))
                                .Column(BodyColumn.third),
                            endTimePicker,
                        },
                    },
                }
                    .Row(BodyRow.fifth)
                    .ColumnSpan(2)
                    .Margins(0, 10, 0, 0),
                new Regular8Label { Text = "*Selected time slot should not exceed 3 hours" }
                    .ColumnSpan(2)
                    .Row(BodyRow.sixth),
                new MediumButton
                {
                    Text = "Done",
                    WidthRequest = 300,
                    HorizontalOptions = LayoutOptions.Center,
                }
                    .Bind(
                        Button.CommandProperty,
                        nameof(viewModel.TimeSelectionCompleteCommand),
                        source: viewModel
                    )
                    .Column(BodyColumn.first)
                    .ColumnSpan(2)
                    .Row(BodyRow.seventh)
                    .Margins(0, 30, 0, 0),
            },
        }.AppThemeBinding(Grid.BackgroundProperty, Colors.White, Colors.Black);
    }
}
