using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Player.ViewModels
{
    public partial class DashboardViewModel : BaseViewModel
    {
        [ObservableProperty]
        byte[]? playerImage;

        [ObservableProperty]
        string? userName;

        Realm? realm;

        [ObservableProperty]
        string? currentVersion;

        private readonly INavigationService navigationService;
        private readonly IRealmService realmService;

        public DashboardViewModel(
            INavigationService navigationService,
            IRealmService realmService,
            IConnectivity connectivity,
            IVersionTracking versionTracking
        )
            : base(navigationService, realmService, connectivity)
        {
            this.navigationService = navigationService;
            this.realmService = realmService;

            currentVersion = versionTracking.CurrentVersion.ToString();
        }

        public async Task OnPageLoad()
        {
            try
            {
                realm = Realm.GetInstance(realmService.Config);

                VenueUser? user = null;
                if (realm != null && realmService.RealmUser?.Id != null)
                {
                    user = realm
                        .All<VenueUser>()
                        .Where(n => n.OwnerId == realmService.RealmUser.Id)
                        .FirstOrDefault();
                }

                UserName = user?.FirstName;

                PlayerImage = user?.ProfileImage;
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async Task NavigateToPlay()
        {
            try
            {
                await navigationService.NavigateToAsync($"///{nameof(AvailableGamesView)}");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async Task NavigateToConnectView()
        {
            try
            {
                await navigationService.NavigateToAsync($"///{nameof(ConnectView)}");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async Task NavigateToMyBookingsView()
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

        [RelayCommand]
        async Task NavigateToCoachView()
        {
            try
            {
                await navigationService.NavigateToAsync(nameof(CoachView));
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async Task NavigateToLearnView()
        {
            try
            {
                await navigationService.NavigateToAsync(nameof(LearnView));
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }


    }
}
