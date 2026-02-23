namespace Player.ViewModels.Account
{
    public partial class VerifyEmailForPasswordResetViewModel
        : AccountBaseViewModel,
            IQueryAttributable
    {
        [ObservableProperty]
        string? userEmail;

        [ObservableProperty]
        string? newPassword;

        [ObservableProperty]
        public string oneTimePasscode;

        [ObservableProperty]
        public bool oneTimePasscodeError;

        [ObservableProperty]
        string timeLeft;

        private readonly INavigationService navigationService;
        private readonly IRealmService realmService;
        private readonly IConnectivity connectivity;
        private readonly ITwilioService twilioService;
        private readonly IPopupNavigation popupNavigation;

        public VerifyEmailForPasswordResetViewModel(
            INavigationService navigationService,
            IRealmService realmService,
            IConnectivity connectivity,
            ISettingsService settingsService,
            ITwilioService twilioService,
            IPopupNavigation popupNavigation
        )
            : base(navigationService, connectivity, settingsService)
        {
            this.navigationService = navigationService;
            this.realmService = realmService;
            this.connectivity = connectivity;
            this.twilioService = twilioService;
            this.popupNavigation = popupNavigation;
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            try
            {
                query.TryGetValue("Email", out object? email);
                if (email != null)
                {
                    UserEmail = email.ToString();
                }
                query.TryGetValue(nameof(NewPassword), out object? newPassword);
                if (newPassword != null)
                {
                    NewPassword = newPassword.ToString();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [RelayCommand]
        async Task SendEmailOTP()
        {
            try
            {
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
                if (string.IsNullOrWhiteSpace(OneTimePasscode) || OneTimePasscode.Length < 6)
                {
                    await Shell.Current.DisplayAlert("Alert", "Please enter 6 digit OTP", "OK");
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
                            await App.RealmApp.EmailPasswordAuth.CallResetPasswordFunctionAsync(
                                UserEmail,
                                NewPassword
                            );
                            User? realmUser = realmService.RealmUser;
                            if (realmUser != null)
                            {
                                await realmUser.LogOutAsync();
                            }
                            await AlertHelper.ShowSnackBar("The new password has been reset");
                            await navigationService.NavigateToAsync($"///{nameof(LoginView)}");
                        }
                    }
                    else if (status == "pending")
                    {
                        await AlertHelper.ShowSnackBar("OTP Verification Failed");
                    }
                    await popupNavigation.PopAsync();
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
