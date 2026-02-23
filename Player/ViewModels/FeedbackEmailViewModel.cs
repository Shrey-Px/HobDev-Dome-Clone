namespace Player.ViewModels
{
    public partial class FeedbackEmailViewModel : BaseViewModel
    {
        [ObservableProperty]
        byte[]? playerImage;

        [ObservableProperty]
        string message;

        private readonly IFeedbackEmailService emailService;
        private readonly IConnectivity connectivity;
        private readonly IRealmService realmService;
        private readonly INavigationService navigationService;
        private readonly IPopupNavigation popupNavigation;

        public FeedbackEmailViewModel(
            INavigationService navigationService,
            IRealmService realmService,
            IConnectivity connectivity,
            IFeedbackEmailService emailService,
            IPopupNavigation popupNavigation
        )
            : base(navigationService, realmService, connectivity)
        {
            this.navigationService = navigationService;
            this.realmService = realmService;
            this.connectivity = connectivity;
            this.emailService = emailService;
            this.popupNavigation = popupNavigation;

            using (Realm realm = Realm.GetInstance(realmService.Config))
            {
                VenueUser? Player = realm
                    .All<VenueUser>()
                    .Where(n => n.OwnerId == realmService.RealmUser.Id)
                    .FirstOrDefault();
                PlayerImage = Player?.ProfileImage;
            }
        }

        [RelayCommand]
        async Task SendEmail()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Message))
                {
                    await Shell.Current.DisplayAlert("Error", "Please enter a message", "Okay");
                }
                else if (connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await Shell.Current.DisplayAlert("Alert", "No Network Connection", "OK");
                }
                else
                {
                    await popupNavigation.PushAsync(new BusyMopup());

                    using (Realm realm = Realm.GetInstance(realmService.Config))
                    {
                        string? email =
                            (
                                realm
                                    .All<VenueUser>()
                                    .Where(n => n.OwnerId == realmService.RealmUser.Id)
                                    .FirstOrDefault()
                                    ?.Email
                            ) ?? string.Empty;
                        await emailService.SendEmail(email, Message);
                        await popupNavigation.PopAsync();
                        await Shell.Current.DisplayAlert(
                            "Success",
                            "Your message has been sent",
                            "Okay"
                        );
                        Message = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                await popupNavigation.PopAsync();
                await Shell.Current.DisplayAlert("Error", ex.Message, "Okay");
            }
        }

        [RelayCommand]
        async Task NavigateBack()
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
