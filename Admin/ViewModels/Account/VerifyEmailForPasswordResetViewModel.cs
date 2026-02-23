namespace Admin.ViewModels.Account
{
    public partial class VerifyEmailForPasswordResetViewModel : ObservableObject, IQueryAttributable
    {
        [ObservableProperty]
        string oneTimePasscode = string.Empty;

        [ObservableProperty]
        string userEmail = string.Empty;

        [ObservableProperty]
        string newPassword = string.Empty;

        [ObservableProperty]
        bool timerStarted;

        [ObservableProperty]
        int timeLeft;

        [ObservableProperty]
        Color sendButtonColor;

        private readonly IRealmService realmService;
        private readonly INavigationService navigationService;
        private readonly ISettingsService settingsService;
        private readonly ITwilioService twilioService;
        private readonly IConnectivity connectivity;
        private readonly IPopupNavigation popupNavigation;

        public VerifyEmailForPasswordResetViewModel(
            IRealmService realmService,
            INavigationService navigationService,
            ISettingsService settingsService,
            ITwilioService twilioService,
            IConnectivity connectivity,
            IPopupNavigation popupNavigation
        )
        {
            try
            {
                this.realmService = realmService;
                this.navigationService = navigationService;
                this.settingsService = settingsService;
                this.twilioService = twilioService;
                this.connectivity = connectivity;
                this.popupNavigation = popupNavigation;

                SendButtonColor = Color.FromArgb("#EF2F50");
            }
            catch (Exception)
            {
                Shell.Current.DisplayAlert("Error", "An error occurred", "OK");
            }
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            try
            {
                if (query.TryGetValue(nameof(UserEmail), out object? emailValue))
                {
                    string? email = emailValue.ToString();
                    if (email != null)
                    {
                        UserEmail = email;
                    }
                }

                if (query.TryGetValue(nameof(NewPassword), out object? passwordValue))
                {
                    string? password = passwordValue.ToString();
                    if (password != null)
                    {
                        NewPassword = password;
                    }
                }
            }
            catch (Exception ex)
            {
                AppShell.Current.DisplayAlert("error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async Task SendEmailOTP()
        {
            try
            {
                await twilioService.SendEmailOTP(UserEmail);
                await Shell.Current.DisplayAlert(
                    "OTP sent",
                    "OTP successfully sent to registered e-mail",
                    "OK"
                );

                TimeLeft = 60;
                TimerStarted = true;
                SendButtonColor = Color.FromArgb("#D3D3D3");

                PeriodicTimer timer = new PeriodicTimer(TimeSpan.FromSeconds(1));

                while (await timer.WaitForNextTickAsync())
                {
                    TimeLeft--;
                    if (TimeLeft == 0)
                    {
                        SendButtonColor = Color.FromArgb("#EF2F50");
                        TimerStarted = false;
                        timer.Dispose();
                    }
                }
                ;
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async Task VerifyEmailOTP()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(OneTimePasscode) || OneTimePasscode.Length < 6)
                {
                    await Shell.Current.DisplayAlert("Alert", "Please enter 6 digit OTP", "OK");
                    return;
                }
                else
                {
                    await popupNavigation.PushAsync(new BusyMopup());
                    string status = await twilioService.VerifyEmailOTP(UserEmail, OneTimePasscode);

                    if (status == "approved")
                    {
                        await AlertHelper.ShowSnackBar("OTP Verified");
                        await App.RealmApp.EmailPasswordAuth.CallResetPasswordFunctionAsync(
                            UserEmail,
                            NewPassword
                        );
                        User? realmUser = App.RealmApp.CurrentUser;
                        if (realmUser != null)
                        {
                            await realmUser.LogOutAsync();
                        }
                        await AlertHelper.ShowSnackBar("The new password has been reset");
                        await navigationService.NavigateToAsync($"///Login");
                        await popupNavigation.PopAsync();
                    }
                    else if (status == "pending")
                    {
                        await popupNavigation.PopAsync();
                        await Shell.Current.DisplayAlert(
                            "Invalid",
                            "Invalid OTP. Please try again",
                            "OK"
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                await popupNavigation.PopAsync();
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}
