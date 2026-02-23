namespace Player.ViewModels
{
    public partial class NotificationViewModel : ObservableObject
    {
        private readonly INavigationService navigationService;
        private readonly IRealmService realmService;
        private readonly IConnectivity connectivity;

        public NotificationViewModel(
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
        public async Task NavigateBack()
        {
            await navigationService.PopAsync();
        }
    }
}
