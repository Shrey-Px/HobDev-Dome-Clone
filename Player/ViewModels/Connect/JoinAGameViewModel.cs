using Dome.Shared.Synced;

namespace Player.ViewModels.Connect
{
    public partial class JoinAGameViewModel : ObservableObject
    {
        [ObservableProperty]
        ObservableCollection<PlannedBooking> plannedBookings;

        [ObservableProperty]
        ObservableCollection<PlannedBooking> othersPlannedBookings;

        [ObservableProperty]
        ObservableCollection<PlannedBooking> othersFilteredPlannedBookings;

        [ObservableProperty]
        ObservableCollection<Game>? onboardedGames;

        [ObservableProperty]
        Game? selectedGame;

        [ObservableProperty]
        ObservableCollection<string> venueLocations;

        [ObservableProperty]
        string selectedLocation;

        [ObservableProperty]
        PlannedBooking selectedBooking;

        AppTheme currentTheme;

        Realm realm;

        VenueUser? Player;

        IDisposable bookingToken;

        private readonly IRealmService realmService;
        private readonly INavigationService navigationService;

        private readonly IPopupNavigation popupNavigation;

        public JoinAGameViewModel(
            IRealmService realmService,
            INavigationService navigationService,
            IPopupNavigation popupNavigation
        )
        {
            try
            {
                plannedBookings = new ObservableCollection<PlannedBooking>();
                OthersPlannedBookings = new ObservableCollection<PlannedBooking>();
                OthersFilteredPlannedBookings = new ObservableCollection<PlannedBooking>();
                OnboardedGames = new ObservableCollection<Game>();
                VenueLocations = new ObservableCollection<string>();

                this.realmService = realmService;
                this.navigationService = navigationService;
                this.popupNavigation = popupNavigation;

                realm = Realm.GetInstance(realmService.Config);

                // Observe collection notifications
                bookingToken = realm
                    .All<PlannedBooking>()
                    .SubscribeForNotifications(
                        async (sender, changes) =>
                        {
                            if (changes == null)
                            {
                                // This is the case when the notification is called
                                // for the first time.
                                // Populate tableview/listview with all the items
                                // from `collection`

                                return;
                            }
                            //Handle individual changes

                            foreach (int i in changes.DeletedIndices)
                            {
                                // ... handle deletions ...

                                await FilterBookings();

                                return;
                            }
                            foreach (int i in changes.InsertedIndices)
                            {
                                // ... handle insertions ...

                                await FilterBookings();
                                return;
                            }
                            foreach (int i in changes.NewModifiedIndices)
                            {
                                // ... handle modifications ...
                                await FilterBookings();
                                return;
                            }
                        }
                    );

                Application.Current.RequestedThemeChanged += Current_RequestedThemeChanged;
            }
            catch (Exception ex)
            {
                Shell.Current.DisplayAlert("Error", ex.Message, "Okay");
            }
        }

        public async Task InitializeAsync()
        {
            try
            {
                Player = realm
                    .All<VenueUser>()
                    .Where(n => n.OwnerId == realmService.RealmUser.Id)
                    .FirstOrDefault();

                await GetGames();

                SelectedGame = OnboardedGames.FirstOrDefault();

                this.OnPropertyChanged(nameof(SelectedGame));

                List<Venue>? ActiveVendors = realm
                    .All<Venue>()
                    .Where(v => v.IsActive == true)
                    .ToList();

                List<string> cities = ActiveVendors.Select(v => v.Address.City).Distinct().ToList();

                VenueLocations.Clear();
                VenueLocations.Add("All Locations");
                foreach (string city in cities)
                {
                    VenueLocations.Add($"{city}, Ontario");
                }
                SelectedLocation = VenueLocations.FirstOrDefault();

                await FilterBookings();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "Okay");
            }
        }

        private async Task GetGames()
        {
            try
            {
                List<Venue>? ActiveVendors = realm
                    .All<Venue>()
                    .Where(v => v.IsActive == true)
                    .ToList();

                List<AvailableGame>? Games = ActiveVendors
                    ?.SelectMany(m => m.AvailableGames)
                    .DistinctBy(x => x.Name)
                    .ToList();

                OnboardedGames.Clear();

                foreach (AvailableGame availableGame in Games)
                {
                    Game? game = realm
                        .All<Game>()
                        .Where(g => g.GameName == availableGame.Name)
                        .FirstOrDefault();
                    if (game != null)
                    {
                        OnboardedGames.Add(game);
                    }
                }

                currentTheme = Application.Current.RequestedTheme;
                ChangeGameIcons();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "Okay");
            }
        }

        private void Current_RequestedThemeChanged(object? sender, AppThemeChangedEventArgs e)
        {
            currentTheme = e.RequestedTheme;
            ChangeGameIcons();
        }

        private void ChangeGameIcons()
        {
            if (currentTheme == AppTheme.Dark)
            {
                foreach (Game game in OnboardedGames)
                {
                    game.GameIcon = game.DarkModeGameIcon;
                }
            }
            else
            {
                foreach (Game game in OnboardedGames)
                {
                    if (game.IsSelected)
                    {
                        game.GameIcon = game.DarkModeGameIcon;
                    }
                    else
                    {
                        game.GameIcon = game.LightModeGameIcon;
                    }
                }
            }
        }

        async Task FilterBookings()
        {
            try
            {
                PlannedBookings.Clear();
                OthersPlannedBookings.Clear();
                OthersFilteredPlannedBookings.Clear();

                PlannedBookings = realm.All<PlannedBooking>().ToObservableCollection();

                foreach (PlannedBooking booking in PlannedBookings)
                {
                    if (booking?.Host?.Id != Player?.Id)
                    {
                        bool joined = false;
                        List<VenueUser?> applicants = booking
                            .JoinRequests.Select(j => j.AppliedBy)
                            .ToList();
                        foreach (VenueUser user in applicants)
                        {
                            if (user?.Id == Player?.Id)
                            {
                                joined = true;
                                break;
                            }
                        }
                        if (!joined)
                        {
                            OthersPlannedBookings.Add(booking);
                        }
                    }
                }
                await Search();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "Okay");
            }
        }

        [RelayCommand]
        async Task JoinHostedGame()
        {
            try
            {
                //  int approvedRequests= SelectedBooking.JoinRequests.Where(t => t.IsApproved == true).Count();
                // 1 is added to the accepted requests to include the host in the team status
                // if (approvedRequests+1 == SelectedBooking.NumberOfPlayers)
                //     {
                //         await Shell.Current.DisplayAlert("Game full", "This game is full", "Okay");

                //     }
                if (SelectedBooking.JoinRequests.Any(j => j.AppliedBy.Id == Player?.Id))
                {
                    await Shell.Current.DisplayAlert(
                        "Applied",
                        "You have already applied for this game",
                        "Okay"
                    );
                }
                else
                {
                    await navigationService.NavigateToAsync(
                        nameof(ReviewGameBeforeApplyingView),
                        new ShellNavigationQueryParameters { { "Id", SelectedBooking.Id } }
                    );
                }

                SelectedBooking = null;
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "Okay");
            }
        }

        protected override async void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            try
            {
                base.OnPropertyChanged(e);
                if (e.PropertyName == nameof(SelectedGame))
                {
                    OnboardedGames.ToList().ForEach(g => g.IsSelected = false);
                    Game? choosenGame = OnboardedGames
                        .Where(g => g.GameName == SelectedGame.GameName)
                        .FirstOrDefault();
                    choosenGame.IsSelected = true;
                    SelectedGame.IsSelected = true;

                    ChangeGameIcons();

                    await Search();
                }
                else if (e.PropertyName == nameof(SelectedLocation))
                {
                    await Search();
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async Task Search()
        {
            try
            {
                OthersFilteredPlannedBookings.Clear();

                if (SelectedGame != null && SelectedLocation != null)
                {
                    string? city = SelectedLocation?.Split(',')[0].Trim();
                    if (city == "All Locations")
                    {
                        OthersFilteredPlannedBookings = OthersPlannedBookings
                            .Where(x => x.GameName == SelectedGame.GameName)
                            .OrderByDescending(x => x.PlannedDate)
                            .ToObservableCollection();
                    }
                    else
                    {
                        OthersFilteredPlannedBookings = OthersPlannedBookings
                            .Where(x => x.City == city && x.GameName == SelectedGame.GameName)
                            .OrderByDescending(x => x.PlannedDate)
                            .ToObservableCollection();
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
