using Dome.Shared.Local;
using Dome.Shared.Synced;

namespace Player.ViewModels.Connect
{
    public partial class HostingSuccessViewModel : ObservableObject
    {
        private readonly INavigationService navigationService;

        public HostingSuccessViewModel(INavigationService navigationService)
        {
            try
            {
                this.navigationService = navigationService;
            }
            catch (Exception ex)
            {
                Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async Task Complete()
        {
            try
            {
                await navigationService.NavigateToAsync($"///{nameof(MyBookingsView)}");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}
