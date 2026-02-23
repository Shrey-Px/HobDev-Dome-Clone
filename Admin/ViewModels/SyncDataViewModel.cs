using System.Reflection;
using Dome.Shared.Services.Implementations;
using Dome.Shared.Synced;
using Realms;

namespace Admin.ViewModels
{
    public class SyncDataViewModel : ObservableObject
    {
        Realm realmInstance;

        IRealmService realmService;
        INavigationService navigationService;
        IConnectivity connectivity;
        ISettingsService settingsService;

        public SyncDataViewModel(
            IRealmService realmService,
            INavigationService navigationService,
            IConnectivity connectivity,
            ISettingsService settingsService
        )
        {
            try
            {
                this.realmService = realmService;
                this.navigationService = navigationService;
                this.connectivity = connectivity;
                this.settingsService = settingsService;
            }
            catch (Exception ex)
            {
                Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        public async Task LoadData()
        {
            try
            {
                if (connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    // await  StartTimer();
                }
                realmInstance = await Realm.GetInstanceAsync(realmService.Config);
                await realmInstance.Subscriptions.WaitForSynchronizationAsync();

                await FindNextView();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async Task FindNextView()
        {
            try
            {
                string? email = realmService?.RealmUser?.Profile.Email;
                // the tester@store.com is login Id for store reviewer. It should by pass the email and sms verification.
                if (email == "tester@store.com")
                {
                    await navigationService.NavigateToAsync("///VendorYard");
                }
                else
                {
                    if (settingsService.IsEmailVerificationPending)
                    {
                        await navigationService.NavigateToAsync(
                            $"/{nameof(AuthenticateEmailForLoginView)}"
                        );
                    }
                    else
                    {
                        await navigationService.NavigateToAsync("///VendorYard");
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}
