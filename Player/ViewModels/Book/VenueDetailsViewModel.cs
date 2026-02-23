namespace Player.ViewModels
{
    public partial class VenueDetailsViewModel : BaseViewModel, IQueryAttributable
    {
        [ObservableProperty]
        byte[]? playerImage;

        [ObservableProperty]
        ObjectId selectedVenueId;

        [ObservableProperty]
        string? gameName;

        [ObservableProperty]
        AvailableGame selectedGame;

        Realm? realm;

        VenueUser? venueUser;

        [ObservableProperty]
        Venue? selectedVenue;

        [ObservableProperty]
        ObservableCollection<Amenity>? amenities;

        [ObservableProperty]
        string aboutVenue = string.Empty;

        private readonly IRealmService? realmService;
        private readonly INavigationService? navigationService;
        private readonly IConnectivity? connectivity;
        private readonly IImageService? imageService;

        public VenueDetailsViewModel(
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
                this.imageService = imageService;

                Amenities = [];
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
                realm = Realm.GetInstance(realmService.Config);
                venueUser = realm
                    .All<VenueUser>()
                    .Where(n => n.OwnerId == realmService.RealmUser.Id)
                    .FirstOrDefault();
                PlayerImage = venueUser?.ProfileImage;
                if (query.TryGetValue("venueId", out object? venueId))
                {
                    if (venueId != null)
                    {
                        if (venueId is ObjectId)
                        {
                            SelectedVenueId = ObjectId.Parse(venueId.ToString());
                            SelectedVenue = realm.Find<Venue>(SelectedVenueId);
                            foreach (Amenity amenity in SelectedVenue.Amenities)
                            {
                                Amenities.Add(amenity);
                            }
                            // the about property has \n in it, so we need to replace it with new line
                            AboutVenue = SelectedVenue.About.Replace("\\n", "\n");
                        }
                    }
                }

                if (query.TryGetValue("gameName", out object? gameName))
                {
                    if (gameName != null)
                    {
                        if (gameName is string)
                        {
                            GameName = gameName.ToString();
                            await SetGameNameAndImage(GameName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async Task SetGameNameAndImage(string GameName)
        {
            try
            {
                foreach (AvailableGame availableGame in SelectedVenue.AvailableGames)
                {
                    if (availableGame.Name == GameName)
                    {
                        string? firstImageName = availableGame.FirstImageName;
                        if (!string.IsNullOrEmpty(firstImageName))
                        {
                            byte[]? firstImageBytes = await imageService.GetImageFromLocalStorageAsync(
                                firstImageName
                            );
                            if (firstImageBytes == null)
                            {
                                await imageService.DownloadImageFromAWSS3Async(firstImageName);
                                firstImageBytes = await imageService.GetImageFromLocalStorageAsync(
                                    firstImageName
                                );
                            }
                            if (firstImageBytes != null)
                                SelectedVenue.SelectedGamesImages = new ObservableCollection<byte[]>
                                {
                                    firstImageBytes,
                                };
                        }

                        string secondImageName = availableGame.SecondImageName;
                        if (!string.IsNullOrEmpty(secondImageName))
                        {
                            byte[]? secondImageBytes =
                                await imageService.GetImageFromLocalStorageAsync(secondImageName);
                            if (secondImageBytes == null)
                            {
                                await imageService.DownloadImageFromAWSS3Async(secondImageName);
                                secondImageBytes = await imageService.GetImageFromLocalStorageAsync(
                                    secondImageName
                                );
                            }
                            if (secondImageBytes != null)
                                SelectedVenue.SelectedGamesImages.Add(secondImageBytes);
                        }

                        SelectedVenue.SelectedGamesName = availableGame.Name;
                        this.OnPropertyChanged(nameof(SelectedVenue));
                        SelectedGame = availableGame;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async Task BookNow()
        {
            try
            {
                IEnumerable<Booking> onHoldAndReservedBookings = venueUser.Bookings.Where(n =>
                    n.BookingStatus == Dome.Shared.Constants.AppConstants.Created
                    || n.BookingStatus == Dome.Shared.Constants.AppConstants.Reserved
                );
                // if the user has any on hold or reserved bookings, then navigate to cart view to complete the booking
                if (onHoldAndReservedBookings.Any())
                {
                    await navigationService.NavigateToAsync(nameof(PaymentView));
                }
                else
                {
                    await navigationService.NavigateToAsync(
                        nameof(BookingTimingView),
                        new ShellNavigationQueryParameters
                        {
                            { "gameName", GameName },
                            { "venueId", SelectedVenueId },
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
