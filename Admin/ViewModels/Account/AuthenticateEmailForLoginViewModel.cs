namespace Admin.ViewModels.Account
{
    public partial class AuthenticateEmailForLoginViewModel : ObservableObject
    {
        [ObservableProperty]
        string oneTimePasscode = string.Empty;

        [ObservableProperty]
        string? userEmail;

        [ObservableProperty]
        bool timerStarted;

        [ObservableProperty]
        int timeLeft;

        [ObservableProperty]
        Color sendButtonColor;

        private readonly INavigationService navigationService;
        private readonly ISettingsService settingsService;
        private readonly ITwilioService twilioService;
        private readonly IPopupNavigation popupNavigation;

        public AuthenticateEmailForLoginViewModel(
            INavigationService navigationService,
            ISettingsService settingsService,
            ITwilioService twilioService,
            IPopupNavigation popupNavigation
        )
        {
            try
            {
                this.navigationService = navigationService;
                this.settingsService = settingsService;
                this.twilioService = twilioService;
                this.popupNavigation = popupNavigation;

                SendButtonColor = Color.FromArgb("#EF2F50");

                UserEmail = App.RealmApp?.CurrentUser?.Profile.Email;
            }
            catch (Exception ex)
            {
                Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        public async Task SendEmailOTP()
        {
            try
            {
                if (UserEmail == null)
                {
                    await Shell.Current.DisplayAlert("Error", "User email is Empty", "OK");
                    return;
                }
                await twilioService.SendEmailOTP(UserEmail);

                await Shell.Current.DisplayAlert(
                    "OTP sent",
                    "OTP successfully sent to registered e-mail",
                    "OK"
                );

                TimeLeft = 60;
                TimerStarted = true;
                SendButtonColor = Color.FromArgb("#D3D3D3");

                PeriodicTimer timer = new(TimeSpan.FromSeconds(1));

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
                else if (string.IsNullOrWhiteSpace(UserEmail))
                {
                    await Shell.Current.DisplayAlert("Alert", "User email is Empty", "OK");
                    return;
                }
                else
                {
                    await popupNavigation.PushAsync(new BusyMopup());
                    string status = await twilioService.VerifyEmailOTP(UserEmail, OneTimePasscode);
                    if (status != null)
                    {
                        if (status == "approved")
                        {
                            await AlertHelper.ShowSnackBar("OTP Verified");
                            await navigationService.NavigateToAsync("///VendorYard");
                            settingsService.IsEmailVerificationPending = false;
                        }
                    }
                    else if (status == "pending")
                    {
                        await AlertHelper.ShowSnackBar("OTP Verification Failed");
                    }
                }
                await popupNavigation.PopAsync();
            }
            catch (Exception ex)
            {
                await popupNavigation.PopAsync();
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}
