namespace Player.Views.Account
{
    public class AccountBaseView : ContentPage
    {
        public AccountBaseView(AccountBaseViewModel viewModel)
        {
            try
            {
                this.AppThemeBinding(
                    ContentPage.BackgroundProperty,
                    Colors.White,
                    Colors.Black
                );

                BindingContext = viewModel;
            }
            catch (Exception ex)
            {
                Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}
