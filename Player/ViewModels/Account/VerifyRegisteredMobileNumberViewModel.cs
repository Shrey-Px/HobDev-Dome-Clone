namespace Player.ViewModels.Account
{
    public partial class VerifyRegisteredMobileNumberViewModel : AccountBaseViewModel
    {
        [ObservableProperty]
        public string oneTimePasscode = string.Empty;

        [ObservableProperty]
        string? userMobileNumber;

        [ObservableProperty]
        string timeLeft;

        [ObservableProperty]
        Color sendButtonColor;

        [ObservableProperty]
        VenueUser? player;

        Realm realm;

        private readonly INavigationService navigationService;
        private readonly IRealmService realmService;
        private readonly IConnectivity connectivity;
        private readonly ISettingsService settingsService;
        private readonly ITwilioService twilioService;

        public VerifyRegisteredMobileNumberViewModel(
            INavigationService navigationService,
            IRealmService realmService,
            IConnectivity connectivity,
            ISettingsService settingsService,
            ITwilioService twilioService
        )
            : base(navigationService, connectivity, settingsService)
        {
            this.navigationService = navigationService;
            this.realmService = realmService;
            this.connectivity = connectivity;
            this.settingsService = settingsService;
            this.twilioService = twilioService;

            SendButtonColor = Color.FromArgb("#EF2F50");

            realm = Realm.GetInstance(realmService.Config);
        }

        [RelayCommand]
        async Task SendSMSOTP()
        {
            try
            {
                Player = realm
                    .All<VenueUser>()
                    .Where(n => n.OwnerId == realmService.RealmUser.Id)
                    .FirstOrDefault();
                UserMobileNumber = $"{Player?.PhoneCode}{Player?.MobileNumber}";
                if (connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await Shell.Current.DisplayAlert(
                        "No Internet",
                        "Please check your internet connection",
                        "OK"
                    );
                    return;
                }
                else if (string.IsNullOrWhiteSpace(UserMobileNumber))
                {
                    await Shell.Current.DisplayAlert("Alert", "no email found", "OK");
                }
                else
                {
                    await twilioService.SendSMSOTP(UserMobileNumber);

                    TimeSpan timeSpan = new TimeSpan(0, 2, 0);
                    TimeLeft = timeSpan.ToString(@"mm\:ss");
                    SendButtonColor = Color.FromArgb("#D3D3D3");

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
        async Task VerifySMSOTP()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(OneTimePasscode) || OneTimePasscode.Length < 6)
                {
                    await Shell.Current.DisplayAlert("Alert", "Please enter 6 digit OTP", "OK");
                    return;
                }
                if (connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await Shell.Current.DisplayAlert(
                        "No Internet",
                        "Please check your internet connection",
                        "OK"
                    );
                    return;
                }
                else if (string.IsNullOrWhiteSpace(UserMobileNumber))
                {
                    await Shell.Current.DisplayAlert("Alert", "no email found", "OK");
                }
                else
                {
                    string status = await twilioService.VerifySMSOTP(
                        UserMobileNumber,
                        OneTimePasscode
                    );
                    if (status != null)
                    {
                        if (status == "approved")
                        {
                            await AlertHelper.ShowSnackBar("Mobile Number Verified");
                            await realm.WriteAsync(() =>
                            {
                                Player.IsMobileNumberVerified = true;
                            });

                            await navigationService.NavigateToAsync("///LoggedInBar");

                            realm.Dispose();
                        }
                        else if (status == "pending")
                        {
                            await Shell.Current.DisplayAlert(
                                "Invalid",
                                "Invalid OTP. Please try again",
                                "OK"
                            );
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async Task ChangeMobileNumber()
        {
            await navigationService.NavigateToAsync(nameof(ChangeMobileNumberView));
        }

        [RelayCommand]
        async Task Cancel()
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
