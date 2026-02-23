namespace Player.ViewModels
{
    public partial class CoachViewModel : BaseViewModel
    {
        [ObservableProperty]
        byte[]? playerImage;

        [ObservableProperty]
        int selectedIndex;

        [ObservableProperty]
        string? userName;

        [ObservableProperty]
        byte[] gameImage;

        [ObservableProperty]
        ObservableCollection<Coach>? coachesList;

        [ObservableProperty]
        ObservableCollection<Coach>? filteredCoaches;

        [ObservableProperty]
        ObservableCollection<Game> games;

        [ObservableProperty]
        Game? choosenGame;

        VenueUser? Player;

        readonly IRealmService realmService;
        readonly INavigationService navigationService;
        Realm realm;

        public CoachViewModel(
            IRealmService realmService,
            INavigationService navigationService,
            IConnectivity connectivity
        )
            : base(navigationService, realmService, connectivity)
        {
            try
            {
                this.realmService = realmService;
                this.navigationService = navigationService;

                FilteredCoaches = new ObservableCollection<Coach>();
                Games = new ObservableCollection<Game>();
                CoachesList = new ObservableCollection<Coach>();

                realm = Realm.GetInstance(realmService.Config);
                Player = realm
                    .All<VenueUser>()
                    .Where(n => n.OwnerId == realmService.RealmUser.Id)
                    .FirstOrDefault();

                App.Current.RequestedThemeChanged += Current_RequestedThemeChanged;
            }
            catch (Exception ex)
            {
                Shell.Current.DisplayAlert("Error", ex.Message, "Ok");
            }
        }

        public async Task LoadData()
        {
            try
            {
                UserName = Player?.FirstName;
                PlayerImage = Player?.ProfileImage;

                Games = realm.All<Game>().ToObservableCollection();
                CoachesList = realm.All<Coach>().ToObservableCollection();
                FilteredCoaches = new ObservableCollection<Coach>(
                    CoachesList.Where(n => n.GameCategory == "Pickleball")
                );
                SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "Ok");
            }
        }

        private async void Current_RequestedThemeChanged(object? sender, AppThemeChangedEventArgs e)
        {
            await SetImages();
        }

        private async Task SetImages()
        {
            AppTheme Theme = App.Current.RequestedTheme;
            if (Theme == AppTheme.Dark)
            {
                GameImage = ChoosenGame.DarkModeGameIcon;
            }
            else
            {
                GameImage = ChoosenGame.LightModeGameIcon;
            }
        }

        partial void OnSelectedIndexChanged(int value)
        {
            try
            {
                if (CoachesList == null)
                {
                    return;
                }
                if (SelectedIndex == 0)
                {
                    FilteredCoaches = new ObservableCollection<Coach>(
                        CoachesList.Where(n => n.GameCategory == "Pickleball")
                    );
                    ChoosenGame = Games.Where(n => n.GameName == "Pickleball").FirstOrDefault();
                }
                else if (SelectedIndex == 1)
                {
                    FilteredCoaches = new ObservableCollection<Coach>(
                        CoachesList.Where(n => n.GameCategory == "Badminton")
                    );
                    ChoosenGame = Games.Where(n => n.GameName == "Badminton").FirstOrDefault();
                }
                else if (SelectedIndex == 2)
                {
                    FilteredCoaches = new ObservableCollection<Coach>(
                        CoachesList.Where(n => n.GameCategory == "Volleyball")
                    );
                    ChoosenGame = Games.Where(n => n.GameName == "Volleyball").FirstOrDefault();
                }
                else if (SelectedIndex == 3)
                {
                    FilteredCoaches = new ObservableCollection<Coach>(
                        CoachesList.Where(n => n.GameCategory == "Basketball")
                    );
                    ChoosenGame = Games.Where(n => n.GameName == "Basketball").FirstOrDefault();
                }
                else if (SelectedIndex == 4)
                {
                    FilteredCoaches = new ObservableCollection<Coach>(
                        CoachesList.Where(n => n.GameCategory == "Archery")
                    );
                    ChoosenGame = Games.Where(n => n.GameName == "Archery").FirstOrDefault();
                }
                else if (SelectedIndex == 5)
                {
                    FilteredCoaches = new ObservableCollection<Coach>(
                        CoachesList.Where(n => n.GameCategory == "Bowling")
                    );
                    ChoosenGame = Games.Where(n => n.GameName == "Bowling").FirstOrDefault();
                }
                else if (SelectedIndex == 6)
                {
                    FilteredCoaches = new ObservableCollection<Coach>(
                        CoachesList.Where(n => n.GameCategory == "Golf")
                    );
                    ChoosenGame = Games.Where(n => n.GameName == "Golf").FirstOrDefault();
                }
                else if (SelectedIndex == 7)
                {
                    FilteredCoaches = new ObservableCollection<Coach>(
                        CoachesList.Where(n => n.GameCategory == "Escaperoom")
                    );
                    ChoosenGame = Games.Where(n => n.GameName == "Escaperoom").FirstOrDefault();
                }
                else if (SelectedIndex == 8)
                {
                    FilteredCoaches = new ObservableCollection<Coach>(
                        CoachesList.Where(n => n.GameCategory == "Karting")
                    );
                    ChoosenGame = Games.Where(n => n.GameName == "Karting").FirstOrDefault();
                }
                else if (SelectedIndex == 9)
                {
                    FilteredCoaches = new ObservableCollection<Coach>(
                        CoachesList.Where(n => n.GameCategory == "Snooker")
                    );
                    ChoosenGame = Games.Where(n => n.GameName == "Snooker").FirstOrDefault();
                }
                else if (SelectedIndex == 10)
                {
                    FilteredCoaches = new ObservableCollection<Coach>(
                        CoachesList.Where(n => n.GameCategory == "Table tennis")
                    );
                    ChoosenGame = Games.Where(n => n.GameName == "Table tennis").FirstOrDefault();
                }
                SetImages();
            }
            catch (Exception ex)
            {
                Shell.Current.DisplayAlert("Error", ex.Message, "Ok");
            }
        }

        [RelayCommand]
        async Task NavigateBack()
        {
            try
            {
                await navigationService.PopAsync();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}
