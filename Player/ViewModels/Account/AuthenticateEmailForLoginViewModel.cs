namespace Player.ViewModels.Account
{
    public partial class AuthenticateEmailForLoginViewModel : AccountBaseViewModel
    {
        [ObservableProperty]
        public string oneTimePasscode = string.Empty;

        [ObservableProperty]
        string? userEmail;

        [ObservableProperty]
        string timeLeft;

        private readonly INavigationService navigationService;
        private readonly IConnectivity connectivity;
        private readonly ISettingsService settingsService;
        private readonly ITwilioService twilioService;
        private readonly IPopupNavigation popupNavigation;

        public AuthenticateEmailForLoginViewModel(
            INavigationService navigationService,
            IConnectivity connectivity,
            ISettingsService settingsService,
            ITwilioService twilioService,
            IPopupNavigation popupNavigation
        )
            : base(navigationService, connectivity, settingsService)
        {
            this.navigationService = navigationService;
            this.connectivity = connectivity;
            this.settingsService = settingsService;
            this.twilioService = twilioService;
            this.popupNavigation = popupNavigation;

            UserEmail = App.RealmApp.CurrentUser?.Profile.Email;
        }

        [RelayCommand]
        async Task SendEmailOTP()
        {
            try
            {
                if (connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await Shell.Current.DisplayAlert("Alert", "No internet connection", "OK");
                    return;
                }
                else if (UserEmail == null)
                {
                    await Shell.Current.DisplayAlert("Alert", "No email found", "OK");
                    return;
                }
                await twilioService.SendEmailOTP(UserEmail);

                TimeSpan timeSpan = new TimeSpan(0, 2, 0);

                TimeLeft = timeSpan.ToString(@"mm\:ss");

                PeriodicTimer timer = new(TimeSpan.FromSeconds(1));

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
                if (connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await Shell.Current.DisplayAlert("Alert", "No internet connection", "OK");
                    return;
                }
                else if (string.IsNullOrWhiteSpace(OneTimePasscode) || OneTimePasscode.Length < 6)
                {
                    await Shell.Current.DisplayAlert("Alert", "Please enter 6 digit OTP", "OK");
                    return;
                }
                else if (UserEmail == null)
                {
                    await Shell.Current.DisplayAlert("Alert", "No email found", "OK");
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
                            await navigationService.NavigateToAsync($"///{nameof(LoadDataView)}");
                            await popupNavigation.PopAsync();
                            settingsService.IsLoginAuthenticationPending = false;
                            OneTimePasscode = string.Empty;
                        }
                        else if (status == "pending")
                        {
                            await popupNavigation.PopAsync();
                            await AlertHelper.ShowSnackBar("OTP Verification Failed");
                        }
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
                User? realmUser = App.RealmApp.CurrentUser;
                if (realmUser != null)
                {
                    await App.RealmApp.RemoveUserAsync(realmUser);
                    // closing the app to clean up all the views, viewmodels and services. As the app is not designed to handle multiple users.
                    System.Environment.Exit(0);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}
