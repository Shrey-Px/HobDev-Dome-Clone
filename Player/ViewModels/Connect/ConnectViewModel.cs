using Realms;

namespace Player.ViewModels.Connect
{
    public partial class ConnectViewModel : BaseViewModel
    {
        [ObservableProperty]
        byte[]? playerImage;

        [ObservableProperty]
        string? userName;

        private readonly IRealmService realmService;
        private readonly INavigationService navigationService;
        private readonly IConnectivity connectivity;

        public ConnectViewModel(
            INavigationService navigationService,
            IRealmService realmService,
            IConnectivity connectivity
        )
            : base(navigationService, realmService, connectivity)
        {
            this.connectivity = connectivity;
            this.realmService = realmService;
            this.navigationService = navigationService;
        }

        public async Task LoadData()
        {
            try
            {
                Realm realm = Realm.GetInstance(realmService.Config);
                VenueUser? Player = realm
                    .All<VenueUser>()
                    .Where(n => n.OwnerId == realmService.RealmUser.Id)
                    .FirstOrDefault();
                UserName = Player?.FirstName;
                PlayerImage = Player?.ProfileImage;
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "Ok");
            }
        }
    }
}
