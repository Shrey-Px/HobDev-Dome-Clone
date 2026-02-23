namespace Player.ViewModels.Account
{
    public partial class ChangeMobileNumberViewModel : AccountBaseViewModel
    {
        [ObservableProperty]
        [Required(ErrorMessage = "Number is Empty")]
        [Phone(ErrorMessage = "Not correct Number")]
        string mobileNumber = String.Empty;

        [ObservableProperty]
        string mobileNumberError = string.Empty;

        [ObservableProperty]
        Country? selectedCountry;

        [ObservableProperty]
        VenueUser? player;

        Realm realm;

        private readonly INavigationService navigationService;
        private readonly IRealmService realmService;
        private readonly IConnectivity connectivity;
        private readonly ISettingsService settingsService;

        public ChangeMobileNumberViewModel(
            INavigationService navigationService,
            IRealmService realmService,
            IConnectivity connectivity,
            ISettingsService settingsService
        )
            : base(navigationService, connectivity, settingsService)
        {
            this.navigationService = navigationService;
            this.realmService = realmService;
            this.connectivity = connectivity;
            this.settingsService = settingsService;

            realm = Realm.GetInstance(realmService.Config);
            Player = realm
                .All<VenueUser>()
                .Where(n => n.OwnerId == realmService.RealmUser.Id)
                .FirstOrDefault();

            // at the moment only Canada is supported
            SelectedCountry = new Country(countryName: "Canada", code: "+1", flagImage: "🇨🇦");
        }

        [RelayCommand]
        async Task ChangeMobileNumber()
        {
            try
            {
                await realm.WriteAsync(() =>
                {
                    Player.MobileNumber = MobileNumber;
                });

                await navigationService.PopAsync();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "Okay");
            }
        }

        [RelayCommand]
        async Task Cancel()
        {
            await navigationService.PopAsync();
        }
    }
}
