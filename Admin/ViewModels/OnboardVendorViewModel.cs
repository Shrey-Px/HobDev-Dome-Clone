namespace Admin.ViewModels
{
    public partial class OnboardVendorViewModel : ObservableValidator
    {
        [ObservableProperty]
        List<Province>? provinces;

        [ObservableProperty]
        Province? selectedProvince;

        [ObservableProperty]
        ObservableCollection<City>? cities;

        [ObservableProperty]
        City? selectedCity;

        [ObservableProperty]
        string countryText = "🇨🇦";

        [ObservableProperty]
        string phoneCode = "+1";

        [ObservableProperty]
        byte[]? gameFirstImage;

        [ObservableProperty]
        string? gameFirstImageExtension;

        [ObservableProperty]
        byte[]? gameSecondImage;

        [ObservableProperty]
        string? gameSecondImageExtension;

        [ObservableProperty]
        DateTimeOffset weekdayOpenTime;

        [ObservableProperty]
        DateTimeOffset weekdayCloseTime;

        [ObservableProperty]
        DateTimeOffset weekdayPeakHoursStartTime;

        [ObservableProperty]
        DateTimeOffset weekdayPeakHoursEndTime;

        [ObservableProperty]
        DateTimeOffset weekendOpenTime;

        [ObservableProperty]
        DateTimeOffset weekendCloseTime;

        [ObservableProperty]
        DateTimeOffset weekendPeakHoursStartTime;

        [ObservableProperty]
        DateTimeOffset weekendPeakHoursEndTime;

        [ObservableProperty]
        decimal weekdayHourlyRate;

        [ObservableProperty]
        decimal weekdayPeakHoursHourlyRate;

        [ObservableProperty]
        decimal weekendHourlyRate;

        [ObservableProperty]
        decimal weekendPeakHoursHourlyRate;

        [ObservableProperty]
        string latitudeError = string.Empty;

        [ObservableProperty]
        string longitudeError = string.Empty;

        [ObservableProperty]
        string mobileNumberError = string.Empty;

        [ObservableProperty]
        string emailError = string.Empty;

        [ObservableProperty]
        string aboutError = string.Empty;

        [ObservableProperty]
        public ObservableCollection<Amenity>? allAmenities;

        [ObservableProperty]
        [Required]
        [MinLength(1, ErrorMessage = "Amenity is required")]
        ObservableCollection<object>? selectedAmenities;

        [ObservableProperty]
        public string weekdayStartTimeError = string.Empty;

        [ObservableProperty]
        public string weekdayEndTimeError = string.Empty;

        [ObservableProperty]
        public string weekdayPeakHoursStartTimeError = string.Empty;

        [ObservableProperty]
        public string weekdayPeakHoursEndTimeError = string.Empty;

        [ObservableProperty]
        public string weekendStartTimeError = string.Empty;

        [ObservableProperty]
        public string weekendEndTimeError = string.Empty;

        [ObservableProperty]
        public string weekendPeakHoursStartTimeError = string.Empty;

        [ObservableProperty]
        public string weekendPeakHoursEndTimeError = string.Empty;

        [ObservableProperty]
        string weekdayHourlyRateError = string.Empty;

        [ObservableProperty]
        public string weekdayPeakHoursHourlyRateError = string.Empty;

        [ObservableProperty]
        string weekendHourlyRateError = string.Empty;

        [ObservableProperty]
        public string weekendPeakHoursHourlyRateError = string.Empty;

        string selectedGamesError = string.Empty;

        [ObservableProperty]
        string selectedAmenitiesError = string.Empty;

        [ObservableProperty]
        public ObservableCollection<Game>? allGames;

        [ObservableProperty]
        public ObservableCollection<Field>? fieldsInGame;

        [ObservableProperty]
        Game? selectedGame;

        [ObservableProperty]
        string selectedGameError = string.Empty;

        [ObservableProperty]
        [Required]
        IList<int>? fieldsAndPlayersCount;

        [ObservableProperty]
        int fieldCount;

        [ObservableProperty]
        string fieldCountError = string.Empty;

        [ObservableProperty]
        int playerCount;

        [ObservableProperty]
        string playerCountError = string.Empty;

        [ObservableProperty]
        string gameImageError = string.Empty;

        [ObservableProperty]
        string email = string.Empty;

        [ObservableProperty]
        [Required]
        [MinLength(1, ErrorMessage = "Game is required")]
        ObservableCollection<AvailableGame>? addedGames;

        [ObservableProperty]
        string addedGamesError = string.Empty;

        [ObservableProperty]
        Venue? vendor;

        private readonly IRealmService? realmService;
        private readonly IConnectivity? connectivity;
        private readonly INavigationService? navigationService;
        private readonly IImageService? imageService;
        private readonly IFileSystem? fileSystem;
        private readonly IPopupNavigation? popupNavigation;

        public OnboardVendorViewModel(
            IRealmService realmService,
            IConnectivity connectivity,
            INavigationService navigationService,
            IImageService imageService,
            IFileSystem fileSystem,
            IPopupNavigation popupNavigation
        )
        {
            try
            {
                this.realmService = realmService;
                this.connectivity = connectivity;
                this.navigationService = navigationService;
                this.imageService = imageService;
                this.fileSystem = fileSystem;
                this.popupNavigation = popupNavigation;

                AddedGames = [];
                SelectedAmenities = [];
                FieldsInGame = [];

                FieldsAndPlayersCount = new List<int>
                {
                    1,
                    2,
                    3,
                    4,
                    5,
                    6,
                    7,
                    8,
                    9,
                    10,
                    11,
                    12,
                    13,
                    14,
                    15,
                    16,
                    17,
                    18,
                    19,
                    20,
                    21,
                    22,
                    23,
                    24,
                    25,
                    26,
                    27,
                    28,
                    29,
                    30,
                };

                WeekdayOpenTime = new DateTimeOffset(2020, 2, 2, 8, 0, 0, new TimeSpan(0, 0, 0));
                WeekdayCloseTime = new DateTimeOffset(2020, 2, 2, 20, 0, 0, new TimeSpan(0, 0, 0));
                WeekdayPeakHoursStartTime = new DateTimeOffset(
                    2020,
                    2,
                    2,
                    8,
                    0,
                    0,
                    new TimeSpan(0, 0, 0)
                );
                WeekdayPeakHoursEndTime = new DateTimeOffset(
                    2020,
                    2,
                    2,
                    20,
                    0,
                    0,
                    new TimeSpan(0, 0, 0)
                );
                WeekendOpenTime = new DateTimeOffset(2020, 2, 2, 6, 0, 0, new TimeSpan(0, 0, 0));
                WeekendCloseTime = new DateTimeOffset(2020, 2, 2, 22, 0, 0, new TimeSpan(0, 0, 0));
                WeekendPeakHoursStartTime = new DateTimeOffset(
                    2020,
                    2,
                    2,
                    6,
                    0,
                    0,
                    new TimeSpan(0, 0, 0)
                );
                WeekendPeakHoursEndTime = new DateTimeOffset(
                    2020,
                    2,
                    2,
                    22,
                    0,
                    0,
                    new TimeSpan(0, 0, 0)
                );
            }
            catch (Exception ex)
            {
                Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.PropertyName == nameof(SelectedProvince))
            {
                if (Vendor?.Address != null && SelectedProvince != null)
                    Vendor.Address.Province = SelectedProvince.ProvinceName;
            }
            else if (e.PropertyName == nameof(SelectedCity))
            {
                if (Vendor?.Address != null && SelectedCity != null)
                    Vendor.Address.City = SelectedCity.CityName;
            }
        }

        public async Task CreateNewVendor()
        {
            try
            {
                Vendor = new Venue(
                    id: ObjectId.GenerateNewId(),
                    fullName: string.Empty,
                    about: string.Empty,
                    email: string.Empty,
                    phoneCode: "+1",
                    mobileNumber: string.Empty,
                    stripeUserId: string.Empty
                );
                Vendor.Address = new VenueAddress(
                    street: string.Empty,
                    postCode: string.Empty,
                    country: "Canada",
                    province: string.Empty,
                    city: string.Empty,
                    latitude: 0,
                    longitude: 0
                );

                Realm? realmInstance = await Realm.GetInstanceAsync(realmService.Config);

                Provinces = realmInstance.All<Province>().ToList();
                AllGames = realmInstance.All<Game>().ToObservableCollection();
                AllAmenities = realmInstance.All<Amenity>().ToObservableCollection();

                if (Provinces.Count > 0)
                {
                    SelectedProvince = Provinces.First();
                    Cities = Provinces[0].Cities.OrderBy(c => c.CityName).ToObservableCollection();
                    if (Cities.Count > 0)
                        SelectedCity = Cities.First();
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async void SetErrorValue(string propertyName, string errorMessage)
        {
            try
            {
                switch (propertyName)
                {
                    case nameof(AddedGames):
                        AddedGamesError = errorMessage;
                        break;
                    case nameof(SelectedAmenities):
                        SelectedAmenitiesError = errorMessage;
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        private async Task AddGame()
        {
            try
            {
                if (SelectedGame == null)
                {
                    SelectedGameError = "select a game";
                }
                if (FieldCount == 0)
                {
                    FieldCountError = "select the available fields";
                }
                if (PlayerCount == 0)
                {
                    PlayerCountError = "select the players allowed";
                }
                if (GameFirstImage == null && GameSecondImage == null)
                {
                    GameImageError = "add image";
                }
                WeekdayHourlyRateError =
                    WeekdayHourlyRate >= 1 ? string.Empty : "more than $1 is required";
                WeekendHourlyRateError =
                    WeekendHourlyRate >= 1 ? string.Empty : "more than $1 is required";
                if (
                    (GameFirstImage == null && GameSecondImage == null)
                    || SelectedGame == null
                    || FieldCount == 0
                    || PlayerCount == 0
                    || WeekdayHourlyRate < 1
                    || WeekendHourlyRate < 1
                )
                {
                    return;
                }
                else
                {
                    if (AddedGames.Any())
                    {
                        foreach (AvailableGame availableGame in AddedGames)
                        {
                            if (availableGame.Name == SelectedGame.GameName)
                            {
                                await Shell.Current.DisplayAlert(
                                    "Already added",
                                    $"{availableGame.Name} is already added",
                                    "OK"
                                );
                                return;
                            }
                        }
                    }
                    SelectedGameError = string.Empty;
                    FieldCountError = string.Empty;
                    PlayerCountError = string.Empty;
                    GameImageError = string.Empty;

                    // if the first image is null and the second image is not null then set the first image to the second image and set the second image to null because in the mobile app the first image is the main image which is shown in the list of games
                    if (GameFirstImage == null && GameSecondImage != null)
                    {
                        GameFirstImage = GameSecondImage;
                        GameSecondImage = null;
                    }

                    AvailableGame? game = new AvailableGame(
                        name: SelectedGame.GameName,
                        peoplePerGame: PlayerCount,
                        fieldType: SelectedGame.FieldType,
                        fieldsCount: FieldCount,
                        weekdayHourlyRate: WeekdayHourlyRate,
                        weekdayPeakHoursHourlyRate: WeekdayPeakHoursHourlyRate,
                        weekendHourlyRate: WeekendHourlyRate,
                        weekendPeakHoursHourlyRate: WeekendPeakHoursHourlyRate
                    );
                    game.Timing = new GameTiming(
                        weekdayOpenTime: new DateTimeOffset(
                            WeekdayOpenTime.Year,
                            WeekdayOpenTime.Month,
                            WeekdayOpenTime.Day,
                            WeekdayOpenTime.Hour,
                            WeekdayOpenTime.Minute,
                            0,
                            new TimeSpan(0, 0, 0)
                        ),
                        weekdayCloseTime: new DateTimeOffset(
                            WeekdayCloseTime.Year,
                            WeekdayCloseTime.Month,
                            WeekdayCloseTime.Day,
                            WeekdayCloseTime.Hour,
                            WeekdayCloseTime.Minute,
                            0,
                            new TimeSpan(0, 0, 0)
                        ),
                        weekdayPeakHoursStartTime: new DateTimeOffset(
                            WeekdayPeakHoursStartTime.Year,
                            WeekdayPeakHoursStartTime.Month,
                            WeekdayPeakHoursStartTime.Day,
                            WeekdayPeakHoursStartTime.Hour,
                            WeekdayPeakHoursStartTime.Minute,
                            0,
                            new TimeSpan(0, 0, 0)
                        ),
                        weekdayPeakHoursEndTime: new DateTimeOffset(
                            WeekdayPeakHoursEndTime.Year,
                            WeekdayPeakHoursEndTime.Month,
                            WeekdayPeakHoursEndTime.Day,
                            WeekdayPeakHoursEndTime.Hour,
                            WeekdayPeakHoursEndTime.Minute,
                            0,
                            new TimeSpan(0, 0, 0)
                        ),
                        weekendOpenTime: new DateTimeOffset(
                            WeekendOpenTime.Year,
                            WeekendOpenTime.Month,
                            WeekendOpenTime.Day,
                            WeekendOpenTime.Hour,
                            WeekendOpenTime.Minute,
                            0,
                            new TimeSpan(0, 0, 0)
                        ),
                        weekendCloseTime: new DateTimeOffset(
                            WeekendCloseTime.Year,
                            WeekendCloseTime.Month,
                            WeekendCloseTime.Day,
                            WeekendCloseTime.Hour,
                            WeekendCloseTime.Minute,
                            0,
                            new TimeSpan(0, 0, 0)
                        ),
                        weekendPeakHoursStartTime: new DateTimeOffset(
                            WeekendPeakHoursStartTime.Year,
                            WeekendPeakHoursStartTime.Month,
                            WeekendPeakHoursStartTime.Day,
                            WeekendPeakHoursStartTime.Hour,
                            WeekendPeakHoursStartTime.Minute,
                            0,
                            new TimeSpan(0, 0, 0)
                        ),
                        weekendPeakHoursEndTime: new DateTimeOffset(
                            WeekendPeakHoursEndTime.Year,
                            WeekendPeakHoursEndTime.Month,
                            WeekendPeakHoursEndTime.Day,
                            WeekendPeakHoursEndTime.Hour,
                            WeekendPeakHoursEndTime.Minute,
                            0,
                            new TimeSpan(0, 0, 0)
                        )
                    );
                    game.FirstImageBytes = GameFirstImage;
                    game.FirstImageExtension = GameFirstImageExtension;
                    if (GameSecondImage != null)
                    {
                        game.SecondImageBytes = GameSecondImage;
                        game.SecondImageExtension = GameSecondImageExtension;
                    }

                    // Create fields and embed them inside the game
                    for (int i = 1; i <= FieldCount; i++)
                    {
                        Field field = new(fieldNumber: i, fieldType: SelectedGame.FieldType);
                        game.Fields.Add(field);
                    }

                    AddedGames.Add(game);

                    SelectedGame = null;
                    FieldCount = 0;
                    PlayerCount = 0;
                    GameFirstImage = null;
                    GameSecondImage = null;
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async Task RemoveGame(AvailableGame availableGame)
        {
            try
            {
                if (availableGame != null)
                {
                    AddedGames?.Remove(availableGame);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async Task PickFirstPhoto()
        {
            try
            {
                Dictionary<string, object>? result = await imageService.PickImageAsync();
                if (result != null)
                {
                    GameFirstImage = (byte[])result["image"];
                    GameFirstImageExtension = (string)result["extension"];
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async Task PickSecondPhoto()
        {
            try
            {
                Dictionary<string, object>? result = await imageService.PickImageAsync();
                if (result != null)
                {
                    GameSecondImage = (byte[])result["image"];
                    GameSecondImageExtension = (string)result["extension"];
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async Task RemoveFirstPhoto()
        {
            try
            {
                GameFirstImage = null;
                GameFirstImageExtension = null;
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async Task RemoveSecondPhoto()
        {
            try
            {
                GameSecondImage = null;
                GameSecondImageExtension = null;
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        private async Task ValidateData()
        {
            try
            {
                ValidateAllProperties();
                if (HasErrors)
                {
                    IEnumerable<ValidationResult> propertyErrors = GetErrors();

                    foreach (ValidationResult validationResult in propertyErrors)
                    {
                        SetErrorValue(
                            validationResult.MemberNames.ElementAt(0),
                            validationResult.ErrorMessage
                        );
                    }
                    bool vendorErrorResult = await GetVendorErrors();

                    return;
                }
                bool result = await GetVendorErrors();
                if (result)
                {
                    return;
                }
                if (string.IsNullOrWhiteSpace(vendor.StripeUserId))
                {
                    await Shell.Current.DisplayAlert(
                        "Alert",
                        "Please connect your stripe account",
                        "OK"
                    );
                    return;
                }

                if (connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await Shell.Current.DisplayAlert("Alert", "No Network Connection", "OK");
                    return;
                }

                await CreateVendor();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async Task<bool> GetVendorErrors()
        {
            try
            {
                LatitudeError = Vendor.Address.Latitude == 0 ? "Value can't be zero" : string.Empty;
                LongitudeError =
                    Vendor.Address.Longitude == 0 ? "Value can't be zero" : string.Empty;
                MobileNumberError = string.IsNullOrWhiteSpace(Vendor.MobileNumber)
                    ? "Mobile number is required"
                    : string.Empty;
                EmailError = string.IsNullOrWhiteSpace(Vendor.Email)
                    ? "Email is required"
                    : string.Empty;
                AboutError = string.IsNullOrWhiteSpace(Vendor.About)
                    ? "About is required"
                    : string.Empty;

                if (
                    string.IsNullOrWhiteSpace(Vendor.FullName)
                    || string.IsNullOrWhiteSpace(Vendor.Address.Street)
                    || string.IsNullOrWhiteSpace(Vendor.Address.PostCode)
                    || string.IsNullOrWhiteSpace(Vendor.Address.Province)
                    || string.IsNullOrWhiteSpace(Vendor.Address.City)
                    || Vendor.Address.Latitude == 0
                    || Vendor.Address.Longitude == 0
                    || string.IsNullOrWhiteSpace(Vendor.MobileNumber)
                    || string.IsNullOrWhiteSpace(Vendor.Email)
                    || string.IsNullOrWhiteSpace(Vendor.About)
                )
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }

            return false;
        }

        private async Task CreateVendor()
        {
            try
            {
                await popupNavigation.PushAsync(new BusyMopup());

                if (Vendor != null)
                {
                    using Realm? realmInstance = await Realm.GetInstanceAsync(realmService.Config);
                    await realmInstance.WriteAsync(() =>
                    {
                        // Set image names for all games and embed them in the venue
                        foreach (AvailableGame availableGame in AddedGames)
                        {
                            string? imageName =
                                $"{Vendor.FullName}-{availableGame.Name}-FirstImage{availableGame.FirstImageExtension}";
                            availableGame.FirstImageName = imageName;

                            if (availableGame.SecondImageBytes != null)
                            {
                                string? secondImageName =
                                    $"{Vendor.FullName}-{availableGame.Name}-SecondImage{availableGame.SecondImageExtension}";
                                availableGame.SecondImageName = secondImageName;
                            }

                            // Embed the game (with its embedded fields) in the venue
                            Vendor.AvailableGames.Add(availableGame);
                        }

                        // Add the venue (embedded games and fields are persisted automatically)
                        realmInstance.Add(Vendor);
                    });

                    // Upload images to AWS after realm transaction
                    foreach (AvailableGame availableGame in AddedGames)
                    {
                        if (!string.IsNullOrEmpty(availableGame.FirstImageName))
                        {
                            byte[]? imageBytes = availableGame.FirstImageBytes;
                            await AddImageToAWSS(imageBytes, availableGame.FirstImageName);
                        }

                        if (
                            availableGame.SecondImageBytes != null
                            && !string.IsNullOrEmpty(availableGame.SecondImageName)
                        )
                        {
                            byte[]? imageBytes = availableGame.SecondImageBytes;
                            await AddImageToAWSS(imageBytes, availableGame.SecondImageName);
                        }
                    }
                }

                await popupNavigation.PopAsync();
                await Shell.Current.DisplayAlert("Success", "Vendor onboarded successfully", "OK");
                await navigationService.PopAsync();
            }
            catch (Exception ex)
            {
                await popupNavigation.PopAsync();
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async Task AddImageToAWSS(byte[] imageBytes, string imageName)
        {
            await imageService.UploadImageToAWSS3Async(imageBytes, imageName);
        }

        [RelayCommand]
        async Task AmenitySelectionChanged()
        {
            try
            {
                AllAmenities
                    .ToList()
                    .ForEach(a =>
                    {
                        a.IsSelected = false;
                    });
                Vendor.Amenities.Clear();
                if (SelectedAmenities.Any())
                {
                    SelectedAmenitiesError = string.Empty;
                    foreach (Amenity amenity in SelectedAmenities.Cast<Amenity>())
                    {
                        amenity.IsSelected = true;
                        Vendor.Amenities.Add(amenity);
                    }
                }
                else
                {
                    SelectedAmenitiesError = "Amenity is required";
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async Task Cancel()
        {
            await navigationService.NavigateToAsync("..");
        }
    }
}
