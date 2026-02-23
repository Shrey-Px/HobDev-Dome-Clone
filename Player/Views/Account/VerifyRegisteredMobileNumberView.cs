namespace Player.Views.Account;

public class VerifyRegisteredMobileNumberView : AccountBaseView
{
    private readonly VerifyRegisteredMobileNumberViewModel? viewModel;

    public VerifyRegisteredMobileNumberView(VerifyRegisteredMobileNumberViewModel viewModel)
        : base(viewModel)
    {
        try
        {
            this.viewModel = viewModel;
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
                    (BodyRow.eleventh, Auto)
                ),

                Children =
                {
                    new AccountTopControl(),
                    new Image { }
                        .AppThemeBinding(
                            Image.SourceProperty,
                            "verify_email_light_theme.png",
                            "verify_email_dark_theme.png"
                        )
                        .Row(BodyRow.second),
                    new ExtraBold28Label
                    {
                        Text = "Verify Mobile Number",
                        HorizontalOptions = LayoutOptions.Center,
                    }
                        .Row(BodyRow.third)
                        .Margins(0, 10, 0, 0),
                    new Regular12Label { HorizontalOptions = LayoutOptions.Center }
                        .FormattedText(
                            new[]
                            {
                                new Span
                                {
                                    Text = "You will get OTP via mobile to ",
                                    FontFamily = "InterRegular",
                                    FontSize = 12,
                                }.AppThemeBinding(
                                    Span.TextColorProperty,
                                    Color.FromArgb("#626262"),
                                    Color.FromArgb("#A2A2A2")
                                ),
                                new Span { FontFamily = "InterBold", FontSize = 12 }
                                    .AppThemeBinding(
                                        Span.TextColorProperty,
                                        Colors.Black,
                                        Color.FromArgb("#626262")
                                    )
                                    .Bind(Span.TextProperty, nameof(viewModel.UserMobileNumber)),
                            }
                        )
                        .Row(BodyRow.fourth)
                        .Margins(0, 5, 0, 0),
                    new Label { HorizontalOptions = LayoutOptions.Center }
                        .AppThemeBinding(
                            Regular12Label.TextColorProperty,
                            Color.FromArgb("#626262"),
                            Color.FromArgb("#A2A2A2")
                        )
                        .Margins(0, 0, 0, 0)
                        .Row(BodyRow.sixth)
                        .FormattedText(
                            new[]
                            {
                                new Span
                                {
                                    Text = "You can resend the OTP in ",
                                    FontFamily = "InterBold",
                                    FontSize = 12,
                                },
                                new Span
                                {
                                    TextColor = Colors.Red,
                                    FontFamily = "InterBold",
                                    FontSize = 12,
                                }.Bind(Span.TextProperty, nameof(viewModel.TimeLeft)),
                            }
                        ),
                    new BorderedEntry { HorizontalOptions = LayoutOptions.Fill }
                        .Row(BodyRow.seventh)
                        .Bind(BorderedEntry.EntryTextProperty, nameof(viewModel.OneTimePasscode), BindingMode.TwoWay, source: viewModel)
                        .Row(BodyRow.seventh)
                        .Margins(30, 20, 30, 0),
                    new MediumButton { Text = "CONFIRM", WidthRequest = 257 }
                        .Row(BodyRow.eighth)
                        .BindCommand(nameof(viewModel.VerifySMSOTPCommand), source: viewModel)
                        .Margins(0, 25, 0, 0),
                    new MediumButton
                    {
                        Text = "CANCEL",
                        WidthRequest = 257,
                        Background = Color.FromArgb("#272727"),
                    }
                        .BindCommand(nameof(viewModel.CancelCommand))
                        .Row(BodyRow.ninth)
                        .Margins(0, 10, 0, 0),
                    new HorizontalStackLayout
                    {
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center,
                        Spacing = 5,
                        Children =
                        {
                            new Bold14Label
                            {
                                Text = "Didn't receive the OTP?",
                                TextColor = Color.FromArgb("#A2A2A2"),
                            },
                            new Button
                            {
                                Text = "Resend OTP",
                                Background = Colors.Transparent,
                                TextColor = Color.FromArgb("#EF2F50"),
                                FontFamily = "InterBold",
                                FontSize = 14,
                                Padding = 0,
                            }.BindCommand(nameof(viewModel.SendSMSOTPCommand)),
                        },
                    }
                        .Row(BodyRow.tenth)
                        .Margins(0, 15, 0, 0),
                    new HorizontalStackLayout
                    {
                        HorizontalOptions = LayoutOptions.Center,

                        Spacing = 5,
                        Children =
                        {
                            new Bold14Label
                            {
                                Text = "Not the correct mobile number?",
                                TextColor = Color.FromArgb("#A2A2A2"),
                            },
                            new Button
                            {
                                Text = "Change",
                                Background = Colors.Transparent,
                                TextColor = Color.FromArgb("#EF2F50"),
                                FontFamily = "InterBold",
                                FontSize = 14,
                                Padding = 0,
                            }.BindCommand(nameof(viewModel.ChangeMobileNumberCommand)),
                        },
                    }
                        .Row(BodyRow.eleventh)
                        .Margins(0, 5, 0, 0),
                },
            };

            Loaded += VerifyRegisteredMobileNumberView_Loaded;
        }
        catch (Exception ex)
        {
            Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async void VerifyRegisteredMobileNumberView_Loaded(object? sender, EventArgs e)
    {
        if (viewModel != null)
        {
            await viewModel.SendSMSOTPCommand.ExecuteAsync(null);
        }
    }
}
