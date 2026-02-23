namespace Player.Views.Connect;

public class ReviewGameBeforeApplyingView : BaseView
{
    public ReviewGameBeforeApplyingView(ReviewGameBeforeApplyingViewModel viewModel)
        : base(viewModel)
    {
        try
        {
            base.TopGrid.Children.Add(
                new ImageButton
                {
                    HorizontalOptions = LayoutOptions.Start,
                    VerticalOptions = LayoutOptions.Start,
                }.AppThemeBinding(ImageButton.SourceProperty, "dome_light_theme", "dome_dark_theme")
            );

            base.BaseContentGrid.Children.Add(
                new VerticalStackLayout
                {
                    Padding = new Thickness(16, 0, 16, 10),
                    Children =
                    {
                        new ExtraBold18Label { Text = "Join Game" }.Margins(0, 20, 0, 0),
                        new Regular14Label { }
                            .Bind(
                                Regular14Label.TextProperty,
                                $"{nameof(viewModel.SelectedBooking)}.{nameof(viewModel.SelectedBooking.SelectedVenue)}.{nameof(viewModel.SelectedBooking.SelectedVenue.FullName)}"
                            )
                            .Margins(0, 20, 0, 0),
                        new Regular14Label { }
                            .Bind(
                                Regular14Label.TextProperty,
                                binding1: new Binding(
                                    $"{nameof(viewModel.SelectedBooking)}.{nameof(viewModel.SelectedBooking.BookingInformation)}"
                                ),
                                binding2: new Binding(
                                    $"{nameof(viewModel.SelectedBooking)}.{nameof(viewModel.SelectedBooking.SkillLevel)}"
                                ),
                                convert: (
                                    (string? bookingInformation, string? skillLevel) values
                                ) => $" {values.bookingInformation} | {values.skillLevel}"
                            )
                            .Margins(0, 0, 0, 0),
                        new Medium12Label { }
                            .Bind(
                                Medium12Label.TextProperty,
                                $"{nameof(viewModel.SelectedBooking)}.{nameof(viewModel.SelectedBooking.HostMessage)}"
                            )
                            .Margins(0, 25, 0, 0),
                        new Medium11Label
                        {
                            Text =
                                "Please refrain from making any payments until you receive booking confirmation from the host.",
                        }.Margins(0, 40, 0, 0),
                        new Border
                        {
                            StrokeShape = new RoundRectangle
                            {
                                CornerRadius = new CornerRadius(15),
                            },
                            StrokeThickness = 2,
                            Background = Colors.Transparent,
                            Padding = 10,
                            Content = new Editor
                            {
                                Placeholder =
                                    "Send message to the host with any request you might have. (Optional)",
                                FontFamily = "InterRegular",
                                FontSize = 12,
                                HeightRequest = 100,
                                MaxLength = 110,
                                Background = Colors.Transparent,
                            }
                                .Bind(
                                    Editor.TextProperty,
                                    nameof(viewModel.ApplicantMessage),
                                    BindingMode.TwoWay
                                )
                                .AppThemeBinding(
                                    Editor.TextColorProperty,
                                    Colors.Black,
                                    Colors.White
                                )
                                .AppThemeBinding(
                                    Editor.PlaceholderColorProperty,
                                    Colors.Black,
                                    Colors.White
                                ),
                        }
                            .AppThemeBinding(Border.StrokeProperty, Brush.Black, Brush.White)
                            .Margins(0, 10, 0, 0),
                        new Medium12Label { HorizontalOptions = LayoutOptions.End }
                            .Bind(
                                Medium12Label.TextProperty,
                                nameof(viewModel.ApplicantMessageLength),
                                BindingMode.TwoWay,
                                stringFormat: "{0}/110 characters"
                            )
                            .Margins(0, 5, 0, 0),
                        new MediumButton { Text = "Request to Join" }
                            .BindCommand(nameof(viewModel.ApplyCommand))
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
                            .Margins(20, 20, 20, 0),
                    },
                }
                    .AppThemeBinding(
                        VerticalStackLayout.BackgroundProperty,
                        Colors.White,
                        Color.FromArgb("#23262A")
                    )
                    .Row(BodyRow.second)
                    .ColumnSpan(2)
            );
        }
        catch (Exception ex)
        {
            Shell.Current.DisplayAlert("Error", ex.Message, "Ok");
        }
    }
}
