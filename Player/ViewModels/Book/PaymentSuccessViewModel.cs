namespace Player.ViewModels
{
    public partial class PaymentSuccessViewModel : ObservableObject
    {
        public ICommand CloseBookingCommand { get; set; }

        private readonly INavigationService navigationService;

        public PaymentSuccessViewModel(INavigationService navigationService)
        {
            try
            {
                this.navigationService = navigationService;
                CloseBookingCommand = new Command(async () => await CloseBooking());
            }
            catch (Exception ex)
            {
                Shell.Current.DisplayAlert("Error", ex.Message, "Ok");
            }
        }

        private async Task CloseBooking()
        {
            try
            {
                await navigationService.NavigateToAsync($"///LoggedInBar/{nameof(DashboardView)}");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "Ok");
            }
        }
    }
}
