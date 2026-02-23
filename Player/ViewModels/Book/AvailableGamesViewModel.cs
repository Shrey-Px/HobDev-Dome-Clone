namespace Player.ViewModels
{
    public partial class AvailableGamesViewModel : BaseViewModel
    {
        [ObservableProperty]
        byte[]? playerImage;

        [ObservableProperty]
        ObservableCollection<Game>? availableGames;

        [ObservableProperty]
        Game? selectedGame;

        [ObservableProperty]
        string? userName;

        [ObservableProperty]
        Booking? recentBooking;

        Realm? realm;

        private readonly INavigationService? navigationService;
        private readonly IRealmService? realmService;

        public AvailableGamesViewModel(
            INavigationService navigationService,
            IRealmService realmService,
            IConnectivity connectivity
        )
            : base(navigationService, realmService, connectivity)
        {
            try
            {
                this.realmService = realmService;
                this.navigationService = navigationService;
            }
            catch (Exception ex)
            {
                Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        public async Task OnPageLoad()
        {
            try
            {
                realm = Realm.GetInstance(realmService.Config);
                VenueUser? user = realm
                    .All<VenueUser>()
                    .Where(n => n.OwnerId == realmService.RealmUser.Id)
                    .FirstOrDefault();

                UserName = user?.FirstName;

                PlayerImage = user?.ProfileImage;

                RecentBooking = user
                    ?.Bookings.Where(s =>
                        s.BookingStatus == Dome.Shared.Constants.AppConstants.Booked
                        || s.BookingStatus == Dome.Shared.Constants.AppConstants.Completed
                    )
                    .OrderByDescending(b => b.BookingDate)
                    .FirstOrDefault();

                await GetAvailableGames();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        async Task GetAvailableGames()
        {
            try
            {
                AvailableGames = [];

                // AvailableGame is now embedded in Venue, so query through venues
                IEnumerable<AvailableGame> allGames = realm
                    .All<Venue>()
                    .Where(v => v.IsActive == true)
                    .ToList()
                    .SelectMany(v => v.AvailableGames);
                IEnumerable<AvailableGame> favouriteGames = allGames.DistinctBy(x => x.Name);

                foreach (AvailableGame availableGame in favouriteGames)
                {
                    Game? game = realm
                        .All<Game>()
                        .Where(a => a.GameName == availableGame.Name)
                        .FirstOrDefault();
                    if (game != null)
                    {
                        AvailableGames.Add(game);
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async Task NavigateToVenuesList()
        {
            try
            {
                await navigationService.NavigateToAsync(nameof(VenuesListView));
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async Task VenueDetails()
        {
            try
            {
                if (RecentBooking != null)
                {
                    await navigationService.NavigateToAsync(
                        nameof(VenueDetailsView),
                        new ShellNavigationQueryParameters
                        {
                            { "gameName", RecentBooking.GameName },
                            { "venueId", RecentBooking.Venue.Id },
                        }
                    );
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async Task GameChoosen(Game game)
        {
            try
            {
                if (SelectedGame == null)
                {
                    return;
                }
                await navigationService.NavigateToAsync(
                    nameof(VenuesListView),
                    new ShellNavigationQueryParameters
                    {
                        { "SelectedGameName", SelectedGame.GameName },
                    }
                );
                SelectedGame = null;
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}
