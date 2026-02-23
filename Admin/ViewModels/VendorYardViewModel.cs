using CommunityToolkit.Maui.Extensions;

namespace Admin.ViewModels
{
    public partial class VendorYardViewModel : BaseViewModel
    {
        [ObservableProperty]
        ObservableCollection<Venue> activeVendors;

        [ObservableProperty]
        ObservableCollection<Venue>? dormantVendors;

        [ObservableProperty]
        ObservableCollection<Venue>? filteredVendors;

        [ObservableProperty]
        ObservableCollection<string>? gameNames;

        [ObservableProperty]
        ObservableCollection<string>? cityNames;

        [ObservableProperty]
        string? selectedGame;

        [ObservableProperty]
        string? selectedCity;

        [ObservableProperty]
        Venue? selectedVendor;

        Realm? realm;

        IDisposable? vendorsToken;

        public VendorYardViewModel(
            IRealmService realmService,
            INavigationService navigationService,
            IConnectivity connectivity
        )
            : base(realmService, navigationService, connectivity)
        {
            try
            {
                ActiveVendors = new ObservableCollection<Venue>();
                FilteredVendors = new ObservableCollection<Venue>();
                DormantVendors = new ObservableCollection<Venue>();
                GameNames = new ObservableCollection<string>();
                CityNames = new ObservableCollection<string>();
            }
            catch (Exception ex)
            {
                Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        public async Task Initialize()
        {
            try
            {
                realm = Realm.GetInstance(realmService.Config);

                ActiveVendors = realm
                    .All<Venue>()
                    .Where(c => c.IsActive == true)
                    .ToObservableCollection();

                DormantVendors = realm
                    .All<Venue>()
                    .Where(c => c.IsActive == false)
                    .ToObservableCollection();
                // select all the games from the ActiveVendors
                List<AvailableGame> Games = ActiveVendors
                    .SelectMany(m => m.AvailableGames)
                    .ToList();
                GameNames = Games
                    .DistinctBy(m => m.Name)
                    .Select(m => m.Name)
                    .ToObservableCollection();
                CityNames = ActiveVendors
                    .DistinctBy(m => m.Address.City)
                    .Select(n => n.Address.City)
                    .ToObservableCollection();

                if (SelectedCity != null)
                {
                    await SearchByRegion();
                }
                else if (SelectedGame != null)
                {
                    await SearchByGame();
                }
                else if (SelectedVendor != null)
                {
                    await SearchByVendor();
                }
                else
                {
                    FilteredVendors = ActiveVendors;
                }

                // Observe collection notifications
                vendorsToken = realm
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

                                await Initialize();

                                return;
                            }
                            foreach (int i in changes.InsertedIndices)
                            {
                                // ... handle insertions ...

                                await Initialize();
                                return;
                            }
                            foreach (int i in changes.NewModifiedIndices)
                            {
                                // ... handle modifications ...
                                await Initialize();
                                return;
                            }
                        }
                    );
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        protected override async void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            try
            {
                base.OnPropertyChanged(e);
                if (e.PropertyName == nameof(SelectedGame))
                {
                    await SearchByGame();
                }
                else if (e.PropertyName == nameof(SelectedCity))
                {
                    await SearchByRegion();
                }
                else if (e.PropertyName == nameof(SelectedVendor))
                {
                    await SearchByVendor();
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async Task ActivateVendor(Venue venue)
        {
            try
            {
                await realm.WriteAsync(() =>
                {
                    venue.IsActive = true;
                });
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async Task DeactivateVendor(Venue venue)
        {
            try
            {
                await realm.WriteAsync(() =>
                {
                    venue.IsActive = false;
                });
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async Task PromoteVendor(Venue venue)
        {
            try
            {
                await realm.WriteAsync(() =>
                {
                    venue.IsPromoted = true;
                });
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async Task UnPromoteVendor(Venue venue)
        {
            try
            {
                await realm.WriteAsync(() =>
                {
                    venue.IsPromoted = false;
                });
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async Task AddVendor()
        {
            try
            {
                await navigationService.NavigateToAsync(nameof(OnboardVendorView));
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async Task EditVendor(Venue venue)
        {
            try
            {
                await navigationService.NavigateToAsync(
                    nameof(EditVendorView),
                    new ShellNavigationQueryParameters { { "UserId", venue.Id.ToString() } }
                );
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        

        [RelayCommand]
        async Task ClearSearch()
        {
            try
            {
                SelectedCity = null;
                SelectedGame = null;
                SelectedVendor = null;
                FilteredVendors = ActiveVendors;
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        async Task SearchByGame()
        {
            try
            {
                if (SelectedGame != null)
                {
                    SelectedCity = null;
                    SelectedVendor = null;
                    // select all the vendors that have the selected game
                    FilteredVendors = ActiveVendors
                        .Where(m => m.AvailableGames.Any(g => g.Name == SelectedGame))
                        .ToObservableCollection();
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        async Task SearchByRegion()
        {
            try
            {
                if (SelectedCity != null)
                {
                    SelectedGame = null;
                    SelectedVendor = null;
                    FilteredVendors = ActiveVendors
                        .Where(m => m.Address.City == SelectedCity)
                        .ToObservableCollection();
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        async Task SearchByVendor()
        {
            try
            {
                if (SelectedVendor != null)
                {
                    SelectedCity = null;
                    SelectedGame = null;
                    FilteredVendors = ActiveVendors
                        .Where(m => m.Id == SelectedVendor.Id)
                        .ToObservableCollection();
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}
