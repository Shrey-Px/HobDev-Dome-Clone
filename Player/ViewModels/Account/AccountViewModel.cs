using Dome.Shared.Services.Implementations;

namespace Player.ViewModels.Account
{
    public partial class AccountViewModel : AccountBaseViewModel
    {
        [ObservableProperty]
        VenueUser? venueUser;

        [ObservableProperty]
        byte[]? playerImage;

        [ObservableProperty]
        bool? isDarkThemWantedByUser;

        Realm realm;

        private readonly INavigationService navigationService;
        private readonly IConnectivity connectivity;
        private readonly IRealmService realmService;
        private readonly IPopupNavigation popupNavigation;
        private readonly IImageService imageService;
        private readonly ISettingsService settingsService;

        public AccountViewModel(
            INavigationService navigationService,
            IRealmService realmService,
            IConnectivity connectivity,
            IPopupNavigation popupNavigation,
            IImageService imageService,
            ISettingsService settingsService
        )
            : base(navigationService, connectivity, settingsService)
        {
            this.navigationService = navigationService;
            this.realmService = realmService;
            this.connectivity = connectivity;
            this.popupNavigation = popupNavigation;
            this.imageService = imageService;
            this.settingsService = settingsService;

            if (settingsService.IsDarkThemeWantedByUser)
            {
                this.IsDarkThemWantedByUser = true;
            }
            else
            {
                this.IsDarkThemWantedByUser = false;
            }

            realm = Realm.GetInstance(realmService.Config);
            VenueUser = realm
                .All<VenueUser>()
                .Where(u => u.OwnerId == realmService.RealmUser.Id)
                .FirstOrDefault();
            PlayerImage = VenueUser.ProfileImage;
        }

        [RelayCommand]
        async Task AddImage()
        {
            try
            {
                if (imageService == null)
                {
                    throw new NullReferenceException(nameof(imageService));
                }
                else
                {
                    Dictionary<string, object>? imageResult = await imageService.PickImageAsync();

                    if (imageResult != null)
                    {
                        byte[]? imageBytes = (byte[])imageResult["image"];
                        if (imageBytes != null)
                        {
                            await realm.WriteAsync(() =>
                            {
                                VenueUser.ProfileImage = imageBytes;
                            });
                            PlayerImage = VenueUser.ProfileImage;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        partial void OnIsDarkThemWantedByUserChanged(bool? value)
        {
            if (value != null)
            {
                if (value == true)
                {
                    App.Current.UserAppTheme = AppTheme.Dark;
                    settingsService.IsDarkThemeWantedByUser = true;
                    Shell.SetTabBarUnselectedColor(Shell.Current, Colors.White);
                }
                else
                {
                    App.Current.UserAppTheme = AppTheme.Light;
                    settingsService.IsDarkThemeWantedByUser = false;
                    Shell.SetTabBarUnselectedColor(Shell.Current, Colors.Black);
                }
            }
        }

        [RelayCommand]
        async Task EditProfile()
        {
            try
            {
                await navigationService.NavigateToAsync(nameof(EditProfileView));
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async Task EditAgeAndInterests()
        {
            try
            {
                await navigationService.NavigateToAsync(nameof(EditInterestsView));
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async Task ChangeMobileNumber()
        {
            try
            {
                await navigationService.NavigateToAsync(nameof(ChangeMobileNumberView));
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async Task ConfirmMobileNumber()
        {
            try
            {
                if (!VenueUser.IsMobileNumberVerified)
                {
                    await navigationService.NavigateToAsync(
                        nameof(VerifyRegisteredMobileNumberView)
                    );
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async Task Logout()
        {
            try
            {
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
                    await popupNavigation.PushAsync(new BusyMopup());
                    User? realmUser = realmService.RealmUser;
                    if (realmUser != null)
                    {
                        await App.RealmApp.RemoveUserAsync(realmUser);
                    }
                    await popupNavigation.PopAsync();
                    await navigationService.NavigateToAsync($"///{nameof(LoginView)}");
                    // closing the app to clean up all the views, viewmodels and services.
                    System.Environment.Exit(0);
                }
            }
            catch (Exception ex)
            {
                await popupNavigation.PopAsync();
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async Task ChangePassword()
        {
            try
            {
                if (connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await Shell.Current.DisplayAlert("Alert", "No Network Connection", "OK");
                    return;
                }
                await navigationService.NavigateToAsync(nameof(ChangePasswordView));
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async Task DeleteAccount()
        {
            try
            {
                var result = await Shell.Current.DisplayAlert(
                    "Delete account",
                    "Your account and data will be deleted permanently",
                    "Yes",
                    "No"
                );
                if (result)
                {
                    await realm.WriteAsync(() =>
                    {
                        realm.Remove(VenueUser);
                    });
                    await popupNavigation.PushAsync(new BusyMopup());
                    User? user = realmService.RealmUser;
                    await App.RealmApp.DeleteUserFromServerAsync(user);
                    await popupNavigation.PopAsync();
                    await navigationService.NavigateToAsync($"///{nameof(LoginView)}");
                }
            }
            catch (Exception ex)
            {
                await popupNavigation.PopAsync();
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async Task EmailUs()
        {
            try
            {
                //TODO: navigate to send email view and use sendgrid email service to send email
                await navigationService.NavigateToAsync(nameof(FeedbackEmailView));
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}
