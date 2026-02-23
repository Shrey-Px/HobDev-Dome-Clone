namespace Player.ViewModels
{
    public partial class BaseViewModel : ObservableObject
    {
        private readonly INavigationService navigationService;
        private readonly IRealmService realmService;
        private readonly IConnectivity connectivity;

        public BaseViewModel(
            INavigationService navigationService,
            IRealmService realmService,
            IConnectivity connectivity
        )
        {
            this.navigationService = navigationService;
            this.realmService = realmService;
            this.connectivity = connectivity;
        }

        [RelayCommand]
        public async Task Notification() =>
            await navigationService.NavigateToAsync(nameof(NotificationView));
    }
}
