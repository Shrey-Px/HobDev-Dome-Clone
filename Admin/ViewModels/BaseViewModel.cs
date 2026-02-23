namespace Admin.ViewModels
{
    public partial class BaseViewModel : ObservableValidator
    {
        public readonly IRealmService? realmService;
        public readonly INavigationService? navigationService;
        public readonly IConnectivity? connectivity;

        public BaseViewModel(
            IRealmService realmService,
            INavigationService navigationService,
            IConnectivity connectivity
        )
        {
            try
            {
                this.realmService = realmService;
                this.navigationService = navigationService;
                this.connectivity = connectivity;
            }
            catch (Exception ex)
            {
                Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}
