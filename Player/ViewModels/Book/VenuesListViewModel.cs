using Dome.Shared.Synced;

namespace Player.ViewModels
{
    public partial class VenuesListViewModel : BaseViewModel, IQueryAttributable
    {
        [ObservableProperty]
        byte[]? playerImage;

        [ObservableProperty]
        string? selectedGameName;

        [ObservableProperty]
        byte[]? selectedGameImage;

        [ObservableProperty]
        byte[]? lightGameImage;

        [ObservableProperty]
        byte[]? darkGameImage;

        [ObservableProperty]
        string? selectedCityName;

        [ObservableProperty]
        Venue? choosenVendor;

        [ObservableProperty]
        string? searchQuery;

        [ObservableProperty]
        ObservableCollection<Venue>? filteredVendors;

        VenueUser? venueUser;

        private readonly INavigationService? navigationService;
        private readonly IRealmService? realmService;
        private readonly IConnectivity? connectivity;
        private readonly ILocationService? locationService;
        private readonly IImageService? imageService;

        private readonly Realm? realm;

        readonly IDisposable? venuesToken;

        public VenuesListViewModel(
            ILocationService locationService,
            IRealmService realmService,
            INavigationService navigationService,
            IConnectivity connectivity,
            IImageService imageService
        )
            : base(navigationService, realmService, connectivity)
        {
            try
            {
                this.realmService = realmService;
                this.navigationService = navigationService;
                this.connectivity = connectivity;
                this.locationService = locationService;
                this.imageService = imageService;

                realm = Realm.GetInstance(realmService.Config);
                FilteredVendors = new ObservableCollection<Venue>();

                venueUser = realm
                    .All<VenueUser>()
                    .Where(n => n.OwnerId == realmService.RealmUser.Id)
                    .FirstOrDefault();

                // Observe collection notifications
                venuesToken = realm
                    .All<Venue>()
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

                                await FilterVenues();
                                return;
                            }
                            foreach (int i in changes.InsertedIndices)
                            {
                                // ... handle insertions ...

                                await FilterVenues();
                                return;
                            }
                            foreach (int i in changes.NewModifiedIndices)
                            {
                                // ... handle modifications ...
                                await FilterVenues();
                                return;
                            }
                        }
                    );

                App.Current.RequestedThemeChanged += Current_RequestedThemeChanged;
            }
            catch (Exception ex)
            {
                Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async void Current_RequestedThemeChanged(object? sender, AppThemeChangedEventArgs e)
        {
            await SetImages();
        }

        partial void OnSearchQueryChanged(string? value)
        {
            try
            {
                FilterVenues();
            }
            catch (Exception ex)
            {
                Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            try
            {
                if (query.TryGetValue(nameof(SelectedGameName), out object? gameName))
                {
                    if (gameName != null)
                    {
                        if (gameName is string)
                        {
                            SelectedGameName = gameName.ToString();
                            await FilterVenues();
                        }
                    }
                }

                if (query.TryGetValue(nameof(SelectedCityName), out object? cityName))
                {
                    if (cityName != null)
                    {
                        if (cityName is string)
                        {
                            SelectedCityName = cityName.ToString();
                            await FilterVenues();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async Task SetImages()
        {
            AppTheme Theme = App.Current.RequestedTheme;
            if (Theme == AppTheme.Dark)
            {
                SelectedGameImage = DarkGameImage;
            }
            else
            {
                SelectedGameImage = LightGameImage;
            }
        }

        public async Task LoadData()
        {
            PlayerImage = venueUser?.ProfileImage;
            Game? game = realm
                ?.All<Game>()
                .Where(n => n.GameName == SelectedGameName)
                .FirstOrDefault();
            if (game != null)
            {
                LightGameImage = game.LightModeGameIcon;
                DarkGameImage = game.DarkModeGameIcon;
                await SetImages();
            }
        }

        async Task FilterVenues()
        {
            try
            {
                IEnumerable<Venue> AllVendors = realm.All<Venue>();
                if (SelectedCityName == null && SelectedGameName != null)
                {
                    FilteredVendors = AllVendors
                        ?.Where(a => a.AvailableGames.Any(n => n.Name == SelectedGameName))
                        .ToObservableCollection();
                    if (!string.IsNullOrWhiteSpace(SearchQuery))
                    {
                        FilteredVendors = FilteredVendors
                            ?.Where(a =>
                                a.FullName.Contains(
                                    SearchQuery,
                                    StringComparison.CurrentCultureIgnoreCase
                                )
                                || a.Address.City.Contains(
                                    SearchQuery,
                                    StringComparison.CurrentCultureIgnoreCase
                                )
                            )
                            .ToObservableCollection();
                    }
                }
                else if (
                    SelectedGameName != null
                    && SelectedCityName != null
                    && string.IsNullOrWhiteSpace(SearchQuery)
                )
                {
                    FilteredVendors = AllVendors
                        ?.Where(a => a.AvailableGames.Any(n => n.Name == SelectedGameName))
                        .OrderBy(i => i.IsPromoted)
                        .ToObservableCollection();
                    FilteredVendors = FilteredVendors
                        ?.Where(a => a.Address?.City == SelectedCityName)
                        .OrderBy(i => i.IsPromoted)
                        .ToObservableCollection();
                }

                foreach (Venue venue in FilteredVendors)
                {
                    foreach (AvailableGame availableGame in venue.AvailableGames)
                    {
                        if (availableGame.Name == SelectedGameName)
                        {
                            string? imageName = availableGame.FirstImageName;
                            if (!string.IsNullOrEmpty(imageName))
                            {
                                byte[]? imageBytes = await imageService.GetImageFromLocalStorageAsync(
                                    imageName
                                );
                                if (imageBytes == null)
                                {
                                    await imageService.DownloadImageFromAWSS3Async(imageName);
                                    imageBytes = await imageService.GetImageFromLocalStorageAsync(
                                        imageName
                                    );
                                }
                                venue.SelectedGamesImage = imageBytes;
                            }
                            venue.SelectedGamesName = availableGame.Name;
                            this.OnPropertyChanged(nameof(venue));
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async Task VenueDetails(Venue venue)
        {
            try
            {
                // the parameter ChoosenVendor is coming from the SelectableItemsView.SelectedItemProperty in the VenuesListView.cs
                if (ChoosenVendor != null)
                {
                    await navigationService.NavigateToAsync(
                        nameof(VenueDetailsView),
                        new ShellNavigationQueryParameters
                        {
                            { "gameName", SelectedGameName },
                            { "venueId", ChoosenVendor.Id },
                        }
                    );
                    ChoosenVendor = null;
                }

                // the parameter venue is coming from the Image Button in the VenuesListView.cs
                if (venue != null)
                {
                    await navigationService.NavigateToAsync(
                        nameof(VenueDetailsView),
                        new ShellNavigationQueryParameters
                        {
                            { "gameName", SelectedGameName },
                            { "venueId", venue.Id },
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
