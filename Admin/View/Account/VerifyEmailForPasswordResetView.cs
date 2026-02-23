namespace Admin.View.Account;

public class VerifyEmailForPasswordResetView : ContentPage
{
    private readonly VerifyEmailForPasswordResetViewModel viewModel;

    public VerifyEmailForPasswordResetView(VerifyEmailForPasswordResetViewModel viewModel)
    {
        try
        {
            Title = "Verify Email";
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
                                    static (VerifyEmailForPasswordResetViewModel vm) => vm.TimeLeft,
                                    source: viewModel
                                ),
                                new Span { Text = " seconds" },
                            }
                        )
                        .Bind(IsVisibleProperty, static (VerifyEmailForPasswordResetViewModel vm) => vm.TimerStarted, source: viewModel),
                    new RegularButton
                    {
                        Text = "Send OTP",
                        WidthRequest = 300,
                        HeightRequest = 50,
                        FontSize = 25,
                    }
                        .Bind(
                            RegularButton.BackgroundProperty,
                            static (VerifyEmailForPasswordResetViewModel vm) => vm.SendButtonColor,
                            source: viewModel
                        )
                        .BindCommand(static (VerifyEmailForPasswordResetViewModel vm) => vm.SendEmailOTPCommand, source: viewModel)
                        .Margins(0, 20, 0, 0),
                    new RegularButton
                    {
                        Text = "Verify Email",
                        WidthRequest = 300,
                        HeightRequest = 50,
                        FontSize = 25,
                    }
                        .BindCommand(nameof(viewModel.VerifyEmailOTPCommand), source: viewModel)
                        .Margins(0, 20, 0, 0),
                },
            };

            BindingContext = viewModel;
            Loaded += VerifyEmailForPasswordResetView_Loaded;
        }
        catch (Exception ex)
        {
            Shell.Current.DisplayAlert("Error", ex.Message, "Ok");
        }
    }

    private async void VerifyEmailForPasswordResetView_Loaded(object? sender, EventArgs e)
    {
        await viewModel.SendEmailOTPCommand.ExecuteAsync(null);
    }
}
