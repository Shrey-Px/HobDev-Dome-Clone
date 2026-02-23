namespace Player.ViewModels.Account
{
    [QueryProperty(nameof(UserEmail), nameof(UserEmail))]
    [QueryProperty(nameof(Password), nameof(Password))]
    public partial class ConfirmEmailToRegisterItViewModel : AccountBaseViewModel
    {
        [ObservableProperty]
        string oneTimePasscode = string.Empty;

        [ObservableProperty]
        string? userEmail;

        [ObservableProperty]
        string? password;

        [ObservableProperty]
        string timeLeft;

        private readonly INavigationService navigationService;
        private readonly ISettingsService settingsService;
        private readonly ITwilioService twilioService;
        private readonly IConnectivity connectivity;
        private readonly IPopupNavigation popupNavigation;

        public ConfirmEmailToRegisterItViewModel(
            INavigationService navigationService,
            ISettingsService settingsService,
            ITwilioService twilioService,
            IConnectivity connectivity,
            IPopupNavigation popupNavigation
        )
            : base(navigationService, connectivity, settingsService)
        {
            this.navigationService = navigationService;
            this.settingsService = settingsService;
            this.twilioService = twilioService;
            this.connectivity = connectivity;
            this.popupNavigation = popupNavigation;
        }

        [RelayCommand]
        async Task SendEmailOTP()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(UserEmail))
                {
                    await Shell.Current.DisplayAlert("Error", "email is missing", "OK");
                }
                else if (string.IsNullOrWhiteSpace(Password))
                {
                    await Shell.Current.DisplayAlert("Error", "Password is missing", "OK");
                }
                else if (connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await Shell.Current.DisplayAlert("Error", "No internet connection", "OK");
                }
                else
                {
                    await twilioService.SendEmailOTP(UserEmail);
                    await Shell.Current.DisplayAlert(
                        "OTP sent",
                        $"OTP successfully sent to the {UserEmail}",
                        "OK"
                    );

                    TimeSpan timeSpan = new TimeSpan(0, 2, 0);
                    TimeLeft = timeSpan.ToString(@"mm\:ss");

                    PeriodicTimer timer = new PeriodicTimer(TimeSpan.FromSeconds(1));

                    while (await timer.WaitForNextTickAsync())
                    {
                        TimeSpan oneSecond = new TimeSpan(0, 0, 01);
                        timeSpan = timeSpan - oneSecond;
                        TimeLeft = timeSpan.ToString(@"mm\:ss");
                        if (timeSpan == TimeSpan.Zero)
                        {
                            timer.Dispose();
                        }
                    }
                    ;
                }
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
                    await Shell.Current.DisplayAlert("Error", "email is missing", "OK");
                }
                else if (string.IsNullOrWhiteSpace(Password))
                {
                    await Shell.Current.DisplayAlert("Error", "Password is missing", "OK");
                }
                else if (connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await Shell.Current.DisplayAlert("Error", "No internet connection", "OK");
                }
                else
                {
                    await popupNavigation.PushAsync(new BusyMopup());
                    string status = await twilioService.VerifyEmailOTP(UserEmail, OneTimePasscode);

                    if (status == "approved")
                    {
                        await AlertHelper.ShowSnackBar("OTP Verified");
                        await App.RealmApp.EmailPasswordAuth.RegisterUserAsync(UserEmail, Password);
                        await App.RealmApp.LogInAsync(
                            Credentials.EmailPassword(UserEmail, Password)
                        );
                        await AlertHelper.ShowSnackBar(
                            $"The new email {UserEmail} is successfully registered"
                        );

                        await navigationService.NavigateToAsync($"///{nameof(LoadDataView)}");
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

        [RelayCommand]
        async Task GoBack()
        {
            try
            {
                await navigationService.PopAsync();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}
