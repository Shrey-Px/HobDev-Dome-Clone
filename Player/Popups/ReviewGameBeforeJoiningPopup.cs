using Mopups.Pages;

namespace Player.Views.Connect;

/// <summary>
/// This popup is shown to the player before joining a game. It shows the game hosting information and allows the player to join the game or cancel the join request. This popup is shown when the player clicks on the join button in the Chat view.
/// </summary>
public class ReviewGameBeforeJoiningPopup : PopupPage
{
    public ReviewGameBeforeJoiningPopup(ChatViewModel viewModel)
    {
        this.CloseWhenBackgroundIsClicked = false;

        Content = new ScrollView
        {
            Margin = new Thickness(10),
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            Content = new Border
            {
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(15) },
                StrokeThickness = 2,
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
                        new Regular18Label { }
                            .Bind(Regular18Label.TextProperty, nameof(viewModel.VenueName))
                            .Margins(0, 20, 0, 0),
                        new Regular14Label { }
                            .Bind(Regular14Label.TextProperty, nameof(viewModel.CityProvince))
                            .Center()
                            .Margins(0, 0, 0, 0),
                        new Bold14Label { }
                            .Bind(
                                Bold14Label.TextProperty,
                                $"{nameof(viewModel.PurchasedBooking)}.{nameof(viewModel.PurchasedBooking.NumberOfPlayersRequiredForHosting)}",
                                stringFormat: "{0} Players (Including host)"
                            )
                            .Center()
                            .Margins(0, 20, 0, 0),
                        new Regular14Label { }
                            .Bind(
                                Regular14Label.TextProperty,
                                $"{nameof(viewModel.PurchasedBooking)}.{nameof(viewModel.PurchasedBooking.GameName)}"
                            )
                            .Center()
                            .Margins(0, 0, 0, 0),
                        new Regular14Label { }
                            .Bind(
                                Regular14Label.TextProperty,
                                $"{nameof(viewModel.PurchasedBooking)}.{nameof(viewModel.PurchasedBooking.PlayerCompetancyLevel)}"
                            )
                            .Center()
                            .Margins(0, 0, 0, 0),
                        new SemiBold14Label { HorizontalOptions = LayoutOptions.Center }.Bind(
                            SemiBold12Label.TextProperty,
                            binding1: new Binding(
                                $"{nameof(viewModel.PurchasedBooking)}.{nameof(viewModel.PurchasedBooking.StartTime)}",
                                stringFormat: "{0:dd, MMM, yyyy (HH:mm}"
                            ),
                            binding2: new Binding(
                                $"{nameof(viewModel.PurchasedBooking)}.{nameof(viewModel.PurchasedBooking.EndTime)}",
                                stringFormat: "{0: HH:mm)}"
                            ),
                            convert: ((string? start, string? end) values) =>
                                $"{values.start} - {values.end}"
                        ),
                        new MediumButton { Text = "Join Game" }
                            .BindCommand(nameof(viewModel.JoinCommand))
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
            }.AppThemeBinding(Border.StrokeProperty, Brush.Black, Brush.White),
        }.AppThemeBinding(
            ScrollView.BackgroundProperty,
            Colors.White,
            Color.FromArgb("#23262A")
        );

        BindingContext = viewModel;
    }
}
