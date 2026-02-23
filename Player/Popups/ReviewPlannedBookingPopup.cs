namespace Player.Popups;

/// <summary>
/// This class is a popup page that displays the details of the planned booking before the player confirms the booking.
/// The player can see the selected date, vendor, city, province, number of players, selected game, skill level, timing, and additional information.
/// The player can also provide additional information in the editor.
/// The player can see the number of characters entered in the editor.
/// The player can host the game by clicking the Host button.
/// The player can cancel the booking by clicking the Cancel button.
/// </summary>
public class ReviewPlannedBookingPopup : PopupPage
{
    public ReviewPlannedBookingPopup(PlanBookingViewModel viewModel)
    {
        this.CloseWhenBackgroundIsClicked = false;

        Content = new Border
        {
            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(15) },
            StrokeThickness = 0,
            Padding = new Thickness(16, 5, 16, 20),

            Content = new VerticalStackLayout
            {
                Children =
                {
                    new Regular20Label { Text = "Game Hosting Information" }
                        .Center()
                        .Margins(0, 20, 10, 0),
                    new Rectangle { HeightRequest = 2 }.AppThemeBinding(
                        Rectangle.BackgroundProperty,
                        Color.FromArgb("#626262"),
                        Color.FromArgb("#D2D2D2")
                    ),
                    new Grid
                    {
                        ColumnDefinitions = Columns.Define(
                            (BodyColumn.first, Auto),
                            (BodyColumn.second, Star)
                        ),

                        RowDefinitions = Rows.Define(
                            (BodyRow.first, Auto),
                            (BodyRow.second, Auto),
                            (BodyRow.third, Auto),
                            (BodyRow.fourth, Auto),
                            (BodyRow.fifth, Auto),
                            (BodyRow.sixth, Auto),
                            (BodyRow.seventh, Auto),
                            (BodyRow.eighth, Auto)
                        ),
                        ColumnSpacing = 10,
                        Children =
                        {
                            new Button
                            {
                                Background = Color.FromArgb("6B6B6B"),
                                FontFamily = "InterRegular",
                                FontSize = 10,
                                CornerRadius = 5,
                                TextColor = Colors.White,
                            }
                                .Bind(
                                    Button.TextProperty,
                                    "SelectedDate",
                                    stringFormat: "{0:dd MMM, yyyy}"
                                )
                                .Center()
                                .Column(BodyColumn.first)
                                .Margins(0, 0, 0, 0)
                                .Row(BodyRow.first),
                            new Regular20Label { }
                                .Bind(
                                    Regular18Label.TextProperty,
                                    $"{nameof(viewModel.SelectedVendor)}.{nameof(viewModel.SelectedVendor.FullName)}"
                                )
                                .Column(BodyColumn.second)
                                .Margins(0, 0, 0, 0)
                                .Row(BodyRow.first),
                            new Regular14Label { }
                                .Bind(Regular14Label.TextProperty, nameof(viewModel.CityProvince))
                                .Margins(10, 0, 0, 0)
                                .Column(BodyColumn.second)
                                .Row(BodyRow.second),
                            new Bold14Label { }
                                .Bind(
                                    Bold14Label.TextProperty,
                                    nameof(viewModel.RequiredPlayers),
                                    stringFormat: "{0} Players (Including host)"
                                )
                                .Margins(0, 20, 0, 0)
                                .Column(BodyColumn.second)
                                .Row(BodyRow.third),
                            new Regular14Label { }
                                .Bind(
                                    Regular14Label.TextProperty,
                                    $"{nameof(viewModel.SelectedGame)}.{nameof(viewModel.SelectedGame.GameName)}"
                                )
                                .Margins(0, 0, 0, 0)
                                .Column(BodyColumn.second)
                                .Row(BodyRow.fourth),
                            new Regular14Label { }
                                .Bind(Regular14Label.TextProperty, nameof(viewModel.SkillLevel))
                                .Margins(0, 0, 0, 0)
                                .Column(BodyColumn.second)
                                .Row(BodyRow.fifth),
                            new SemiBold14Label { }
                                .Bind(SemiBold14Label.TextProperty, nameof(viewModel.Timing))
                                .Margins(0, 20, 0, 0)
                                .Column(BodyColumn.second)
                                .Row(BodyRow.sixth),
                            new Regular14Label { }
                                .Bind(Regular14Label.TextProperty, nameof(viewModel.TimingDisplay))
                                .Margins(0, 0, 0, 0)
                                .Column(BodyColumn.second)
                                .Row(BodyRow.seventh),
                        },
                    }
                        .Center()
                        .Margins(0, 20, 0, 0),
                    new Border
                    {
                        StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(15) },
                        StrokeThickness = 2,
                        Background = Colors.Transparent,
                        Padding = 10,
                        Content = new Editor
                        {
                            Placeholder = "Provide additional information",
                            FontFamily = "InterRegular",
                            FontSize = 12,
                            HeightRequest = 100,
                            MaxLength = 250,
                            Background = Colors.Transparent,
                        }
                            .Bind(
                                Editor.TextProperty,
                                nameof(viewModel.HostMessage),
                                BindingMode.TwoWay
                            )
                            .AppThemeBinding(Editor.TextColorProperty, Colors.Black, Colors.White)
                            .AppThemeBinding(
                                Editor.PlaceholderColorProperty,
                                Colors.Black,
                                Colors.White
                            ),
                    }
                        .AppThemeBinding(Border.StrokeProperty, Brush.Black, Brush.White)
                        .Margins(0, 20, 0, 0),
                    new Medium12Label { HorizontalOptions = LayoutOptions.End }
                        .Bind(
                            Medium12Label.TextProperty,
                            nameof(viewModel.HostMessageCharacterCount),
                            BindingMode.TwoWay,
                            stringFormat: "{0}/250 characters"
                        )
                        .Margins(0, 0, 0, 0),
                    new MediumButton { Text = "Host" }
                        .BindCommand(nameof(viewModel.HostCommand))
                        .Margins(20, 30, 20, 0),
                    new MediumButton
                    {
                        Text = "Cancel",
                        Background = Colors.Transparent,
                        BorderWidth = .5,
                    }
                        .AppThemeColorBinding(
                            MediumButton.TextColorProperty,
                            Colors.Black,
                            Colors.White
                        )
                        .AppThemeColorBinding(
                            MediumButton.BorderColorProperty,
                            Colors.Black,
                            Colors.White
                        )
                        .BindCommand(nameof(viewModel.CancelCommand))
                        .Margins(20, 10, 20, 0),
                },
            },
        }.AppThemeBinding(ScrollView.BackgroundProperty, Colors.White, Colors.Black);

        BindingContext = viewModel;
    }
}
