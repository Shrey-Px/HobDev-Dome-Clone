namespace Admin.View.Account;

public class AuthenticateEmailForLoginView : ContentPage
{
    private readonly AuthenticateEmailForLoginViewModel viewModel;

    public AuthenticateEmailForLoginView(AuthenticateEmailForLoginViewModel viewModel)
    {
        try
        {
            Title = "Authenticate Email";
            this.AppThemeBinding(
                ContentPage.BackgroundProperty,
                Color.FromArgb("#F1F3F2"),
                Colors.Black
            );
            this.viewModel = viewModel;
            Shell.SetFlyoutBehavior(this, FlyoutBehavior.Disabled);
            Content = new VerticalStackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(20, 10, 20, 10),
                Children =
                {
                    new BorderedEntry
                    {
                        PlaceholderText = "Enter OTP here",
                        WidthRequest = 500,
                        HeightRequest = 70,
                    }
                        .Bind(BorderedEntry.EntryTextProperty, nameof(viewModel.OneTimePasscode), BindingMode.TwoWay, source: viewModel)
                        .Margins(0, 20, 0, 0),
                    new Regular16Label { }
                        .FormattedText(
                            new[]
                            {
                                new Span { Text = "You can resend the OTP after " },
                                new Span { TextColor = Colors.Red }.Bind(
                                    Span.TextProperty,
                                    static (AuthenticateEmailForLoginViewModel vm) => vm.TimeLeft,
                                    source: viewModel
                                ),
                                new Span { Text = " seconds" },
                            }
                        )
                        .Bind(IsVisibleProperty, static (AuthenticateEmailForLoginViewModel vm) => vm.TimerStarted, source: viewModel),
                    new RegularButton
                    {
                        Text = "Send OTP",
                        WidthRequest = 300,
                        HeightRequest = 50,
                        FontSize = 25,
                    }
                        .Bind(
                            RegularButton.BackgroundProperty,
                            static (AuthenticateEmailForLoginViewModel vm) => vm.SendButtonColor,
                            source: viewModel
                        )
                        .BindCommand(static (AuthenticateEmailForLoginViewModel vm) => vm.SendEmailOTPCommand, source: viewModel)
                        .Margins(0, 20, 0, 0),
                    new RegularButton
                    {
                        Text = "Authenticate Email",
                        WidthRequest = 300,
                        HeightRequest = 50,
                        FontSize = 25,
                    }
                        .BindCommand(static (AuthenticateEmailForLoginViewModel vm) => vm.VerifyEmailOTPCommand, source: viewModel)
                        .Margins(0, 20, 0, 0),
                },
            };

            BindingContext = viewModel;

            Loaded += AuthenticateEmailForLoginView_Loaded;
        }
        catch (Exception ex)
        {
            Shell.Current.DisplayAlert("Error", ex.Message, "Ok");
        }
    }

    private async void AuthenticateEmailForLoginView_Loaded(object? sender, EventArgs e)
    {
        viewModel.SendEmailOTPCommand.Execute(null);
    }
}
