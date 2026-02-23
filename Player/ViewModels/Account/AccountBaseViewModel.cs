namespace Player.ViewModels.Account
{
    public partial class AccountBaseViewModel : ObservableValidator
    {
        private readonly INavigationService? navigationService;
        private readonly IConnectivity? connectivity;
        private readonly ISettingsService? settingsService;

        public AccountBaseViewModel(
            INavigationService navigationService,
            IConnectivity connectivity,
            ISettingsService settingsService
        )
        {
            this.navigationService = navigationService;
            this.connectivity = connectivity;
            this.settingsService = settingsService;
        }
    }
}
