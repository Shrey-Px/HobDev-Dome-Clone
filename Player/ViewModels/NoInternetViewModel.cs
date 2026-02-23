namespace Player.ViewModels
{
    public partial class NoInternetViewModel : ObservableObject
    {
        private readonly IConnectivity connectivity;
        private readonly INavigationService navigationService;
        PeriodicTimer? timer;

        public NoInternetViewModel(IConnectivity connectivity, INavigationService navigationService)
        {
            this.connectivity = connectivity;
            this.navigationService = navigationService;
        }

        public async Task StartTimer()
        {
            try
            {
                timer = new PeriodicTimer(TimeSpan.FromSeconds(5));

                while (await timer.WaitForNextTickAsync())
                {
                    await CheckConnectivity();
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        public async Task StopTimer()
        {
            try
            {
                timer?.Dispose();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        public async Task CheckConnectivity()
        {
            try
            {
                if (connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    await navigationService.PopAsync();
                    timer.Dispose();
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}
