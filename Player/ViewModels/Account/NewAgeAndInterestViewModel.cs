namespace Player.ViewModels.Account
{
    public partial class NewAgeAndInterestViewModel : AccountBaseViewModel
    {
        [ObservableProperty]
        List<AgeGroup> allAgeGroups;

        [ObservableProperty]
        [Required(ErrorMessage = "Age group not selected")]
        AgeGroup? selectedAgeGroup;

        [ObservableProperty]
        string selectedAgeGroupError = string.Empty;

        [ObservableProperty]
        List<FavouriteGame> allGames;

        [ObservableProperty]
        [Required]
        [MinLength(1, ErrorMessage = "Atleast 1 interest is required")]
        ObservableCollection<object>? selectedGames;

        [ObservableProperty]
        string selectedGamesError = string.Empty;

        Realm? realmInstance;

        private readonly INavigationService? navigationService;
        private readonly IRealmService? realmService;
        private readonly IConnectivity? connectivity;
        private readonly IPopupNavigation popupNavigation;
        private readonly IAppStaticData appStaticData;

        public NewAgeAndInterestViewModel(
            INavigationService navigationService,
            IRealmService realmService,
            IConnectivity connectivity,
            ISettingsService settingsService,
            IAppStaticData appStaticData,
            IPopupNavigation popupNavigation
        )
            : base(navigationService, connectivity, settingsService)
        {
            this.navigationService = navigationService;
            this.realmService = realmService;
            this.connectivity = connectivity;
            this.popupNavigation = popupNavigation;
            this.appStaticData = appStaticData;

            SelectedGames = new ObservableCollection<object>();
            AllAgeGroups = new List<AgeGroup>();
            AllGames = new List<FavouriteGame>();
        }

        public async Task InitializeAsync()
        {
            try
            {
                AllAgeGroups = await appStaticData.GetAgeGroupsForUser();
                AllGames = await appStaticData.GetGamesForUser();

                realmInstance = Realm.GetInstance(realmService.Config);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        private async Task AgeGroupSelected()
        {
            try
            {
                if (SelectedAgeGroup != null)
                {
                    AllAgeGroups.ForEach(ageGroup => ageGroup.IsSelected = false);
                    SelectedAgeGroup.IsSelected = true;
                    SelectedAgeGroupError = "";
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        private void InterestSelected()
        {
            try
            {
                AllGames.ForEach(game => game.IsSelected = false);
                if (SelectedGames.Any())
                {
                    SelectedGamesError = "";
                    foreach (FavouriteGame favouriteGame in SelectedGames.Cast<FavouriteGame>())
                    {
                        favouriteGame.IsSelected = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private void SetErrorValue(string propertyName, string errorMessage)
        {
            try
            {
                switch (propertyName)
                {
                    case nameof(SelectedAgeGroup):
                        SelectedAgeGroupError = errorMessage;
                        break;
                    case nameof(SelectedGames):
                        SelectedGamesError = errorMessage;
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async Task Save()
        {
            try
            {
                ValidateAllProperties();
                if (HasErrors)
                {
                    IEnumerable<ValidationResult> propertyErrors = GetErrors();
                    if (propertyErrors.Any())
                    {
                        foreach (ValidationResult result in propertyErrors)
                        {
                            if (result.ErrorMessage != null)
                            {
                                SetErrorValue(result.MemberNames.ElementAt(0), result.ErrorMessage);
                            }
                            else
                            {
                                SetErrorValue(result.MemberNames.ElementAt(0), "error");
                            }
                        }
                    }
                }
                else
                {
                    await CreateUserProfile();
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async Task CreateUserProfile()
        {
            try
            {
                await popupNavigation.PushAsync(new BusyMopup());

                VenueUser? AppUser = realmInstance
                    .All<VenueUser>()
                    .Where(n => n.OwnerId == realmService.RealmUser.Id)
                    .FirstOrDefault();

                await realmInstance.WriteAsync(() =>
                {
                    AppUser.AgeGroup = SelectedAgeGroup?.AgeGroupName;
                    foreach (FavouriteGame game in SelectedGames.Cast<FavouriteGame>())
                    {
                        AppUser.FavouriteGames.Add(game.GameName);
                    }
                });

                await navigationService.NavigateToAsync("///LoggedInBar");

                await popupNavigation.PopAsync();
            }
            catch (Exception ex)
            {
                await popupNavigation.PopAsync();
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}
