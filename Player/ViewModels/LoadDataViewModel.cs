using CommunityToolkit.Maui.Core;

namespace Player.ViewModels
{
    public partial class LoadDataViewModel : ObservableObject
    {
        Realm realmInstance;

        IRealmService realmService;
        INavigationService navigationService;
        IConnectivity connectivity;

        public LoadDataViewModel(
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

                if (App.Current.UserAppTheme == AppTheme.Dark)
                {
                    CommunityToolkit.Maui.Core.Platform.StatusBar.SetColor(
                        Color.FromArgb("#23262A")
                    );
                    CommunityToolkit.Maui.Core.Platform.StatusBar.SetStyle(
                        StatusBarStyle.LightContent
                    );
                    Shell.SetTabBarUnselectedColor(Shell.Current, Colors.White);
                }
                else if (App.Current.UserAppTheme == AppTheme.Light)
                {
                    CommunityToolkit.Maui.Core.Platform.StatusBar.SetColor(Colors.White);
                    CommunityToolkit.Maui.Core.Platform.StatusBar.SetStyle(
                        StatusBarStyle.DarkContent
                    );
                    Shell.SetTabBarUnselectedColor(Shell.Current, Colors.Black);
                }
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
                VenueUser? Player = realmInstance
                    .All<VenueUser>()
                    .Where(n => n.OwnerId == realmService.RealmUser.Id)
                    .FirstOrDefault();
                if (
                    Player == null
                    || string.IsNullOrWhiteSpace(Player.FirstName)
                    || string.IsNullOrWhiteSpace(Player.LastName)
                    || string.IsNullOrWhiteSpace(Player.Email)
                    || string.IsNullOrWhiteSpace(Player.MobileNumber)
                    || string.IsNullOrWhiteSpace(Player?.Address?.Province)
                    || string.IsNullOrWhiteSpace(Player.Address.City)
                )
                {
                    await navigationService.NavigateToAsync(nameof(NewProfileView));
                }
                else if (Player.AgeGroup == null || Player.FavouriteGames.Count == 0)
                {
                    await navigationService.NavigateToAsync(nameof(NewAgeAndInterestView));
                }
                else
                {
                    await navigationService.NavigateToAsync("///LoggedInBar");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}
