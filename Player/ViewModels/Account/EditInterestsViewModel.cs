namespace Player.ViewModels.Account
{
    public partial class EditInterestsViewModel : AccountBaseViewModel
    {
        public List<AgeGroup>? AllAgeGroups { get; }

        [ObservableProperty]
        [Required(ErrorMessage = "Age group not selected")]
        AgeGroup? selectedAgeGroup;

        public List<FavouriteGame>? AllGames { get; }

        [ObservableProperty]
        [Required]
        [MinLength(1, ErrorMessage = "Atleast 1 interest is required")]
        ObservableCollection<object>? selectedGames;

        [ObservableProperty]
        string selectedAgeGroupError = string.Empty;

        [ObservableProperty]
        string selectedGamesError = string.Empty;

        Realm? realmInstance;

        [ObservableProperty]
        VenueUser? venueUser;

        private readonly INavigationService? navigationService;
        private readonly IRealmService? realmService;

        public EditInterestsViewModel(
            INavigationService navigationService,
            IRealmService realmService,
            IConnectivity connectivity,
            ISettingsService settingsService,
            IAppStaticData appStaticData
        )
            : base(navigationService, connectivity, settingsService)
        {
            try
            {
                this.navigationService = navigationService;
                this.realmService = realmService;

                realmInstance = Realm.GetInstance(realmService.Config);
                VenueUser = realmInstance
                    .All<VenueUser>()
                    .Where(n => n.OwnerId == realmService.RealmUser.Id)
                    .FirstOrDefault();

                AllAgeGroups = appStaticData.GetAgeGroupsForUser().Result;
                AllGames = appStaticData.GetGamesForUser().Result;

                SelectedGames = new ObservableCollection<object>();
            }
            catch (Exception ex)
            {
                Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        public async Task AddGamesAndAge()
        {
            try
            {
                SelectedGames!.Clear();
                //SelectedAgeGroup = new AgeGroup();
                if (AllGames == null)
                {
                    await Shell.Current.DisplayAlert("Error", "AllGames is null", "OK");
                    return;
                }
                else if (AllAgeGroups == null)
                {
                    await Shell.Current.DisplayAlert("Error", "AllAgeGroups is null", "OK");
                    return;
                }
                if (VenueUser?.FavouriteGames.Count > 0)
                {
                    IList<string> favouriteGames = VenueUser.FavouriteGames;
                    foreach (string game in favouriteGames)
                    {
                        FavouriteGame? favouriteGame = AllGames
                            .Where(x => x.GameName == game)
                            .FirstOrDefault();
                        if (favouriteGame != null)
                        {
                            favouriteGame.IsSelected = true;
                            SelectedGames.Add(favouriteGame);
                        }
                    }
                }
                if (!string.IsNullOrWhiteSpace(VenueUser?.AgeGroup))
                {
                    AgeGroup? ageGroup = AllAgeGroups
                        .Where(x => x.AgeGroupName == VenueUser.AgeGroup)
                        .FirstOrDefault();
                    if (ageGroup != null)
                    {
                        ageGroup.IsSelected = true;
                        SelectedAgeGroup = ageGroup;
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            try
            {
                base.OnPropertyChanged(e);
                if (e.PropertyName.Contains("Error") || e.PropertyName == nameof(SelectedGames))
                    return;
                ClearErrors(e.PropertyName);
                ValidateAllProperties();
                IEnumerable<ValidationResult> propertyErrors = GetErrors(e.PropertyName);
                if (propertyErrors.Any())
                {
                    string errorMessage = propertyErrors.FirstOrDefault().ErrorMessage;
                    SetErrorValue(e.PropertyName, errorMessage);
                }
                else
                {
                    string errorMessage = string.Empty;
                    SetErrorValue(e.PropertyName, errorMessage);
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
                    case "SelectedAgeGroup":
                        SelectedAgeGroupError = errorMessage;
                        break;
                    //case "SelectedGames":
                    //    SelectedGamesError = errorMessage;
                    //    break;
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
                if (HasErrors || SelectedGames!.Count() == 0)
                {
                    IEnumerable<ValidationResult> propertyErrors = GetErrors();
                    if (propertyErrors.Any())
                    {
                        foreach (ValidationResult result in propertyErrors)
                        {
                            SetErrorValue(result.MemberNames.ElementAt(0), result.ErrorMessage);
                        }
                        return;
                    }
                }
                else
                {
                    await realmInstance.WriteAsync(() =>
                    {
                        VenueUser.FavouriteGames.Clear();
                        foreach (FavouriteGame game in SelectedGames.Cast<FavouriteGame>())
                        {
                            VenueUser.FavouriteGames.Add(game.GameName);
                        }
                        VenueUser.AgeGroup = SelectedAgeGroup.AgeGroupName;
                    });

                    await navigationService.PopAsync();
                }
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
                if (SelectedGames.Any())
                {
                    SelectedGamesError = "";
                    AllGames.ForEach(game => game.IsSelected = false);
                    foreach (FavouriteGame favouriteGame in SelectedGames.Cast<FavouriteGame>())
                    {
                        favouriteGame.IsSelected = true;
                    }
                }
                else if (!SelectedGames.Any())
                {
                    AllGames.ForEach(game => game.IsSelected = false);
                    SelectedGamesError = "Atleast 1 interest is required";
                    this.OnPropertyChanged(nameof(AllGames));
                }
            }
            catch (Exception ex)
            {
                Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async Task Cancel()
        {
            await navigationService.PopAsync();
        }
    }
}
