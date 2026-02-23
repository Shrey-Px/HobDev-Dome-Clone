namespace Admin.ViewModels
{
    public partial class AppShellViewModel : ObservableObject
    {
       

        [RelayCommand]
        async Task Logout()
        {
            try
            {
                IConnectivity? connectivity =
                    Shell.Current.Handler?.MauiContext?.Services.GetRequiredService<IConnectivity>();
                INavigationService? navigationService =
                    Shell.Current.Handler?.MauiContext?.Services.GetRequiredService<INavigationService>();
                if (connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await Shell.Current.DisplayAlert("Alert", "No Network Connection", "OK");
                    return;
                }

                var result = await Shell.Current.DisplayAlert(
                    "Logout",
                    "Are you sure you want to logout?",
                    "Yes",
                    "No"
                );
                if (result)
                {
                    User? realmUser = App.RealmApp?.CurrentUser;
                    if (realmUser != null)
                    {
                        await realmUser.LogOutAsync();
                    }
                    await navigationService.NavigateToAsync("///Login");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}
