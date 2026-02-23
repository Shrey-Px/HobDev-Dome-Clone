using Dome.Shared.Synced.Player;

namespace Admin.ViewModels
{
    public partial class EditVendorViewModel : ObservableValidator, IQueryAttributable
    {
        [ObservableProperty]
        ObjectId? userId;

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
        string businessNameError = string.Empty;

        [ObservableProperty]
        string streetError = string.Empty;

        [ObservableProperty]
        string postCodeError = string.Empty;

        [ObservableProperty]
        string provinceError = string.Empty;

        [ObservableProperty]
        string cityError = string.Empty;

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
        ObservableCollection<Amenity>? allAmenities;

        [ObservableProperty]
        [Required]
        [MinLength(1, ErrorMessage = "Amenity is required")]
        ObservableCollection<object>? selectedAmenities;

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

        [ObservableProperty]
        string selectedGamesError = string.Empty;

        [ObservableProperty]
        string selectedAmenitiesError = string.Empty;

        [ObservableProperty]
        ObservableCollection<Game>? allGames;

        [ObservableProperty]
        ObservableCollection<Field>? fieldsInGame;

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
        byte[]? gameFirstImage;

        [ObservableProperty]
        string? gameFirstImageExtension;

        [ObservableProperty]
        byte[]? gameSecondImage;

        [ObservableProperty]
        string? gameSecondImageExtension;

        [ObservableProperty]
        string gameImageError = string.Empty;

        [ObservableProperty]
        [Required]
        [MinLength(1, ErrorMessage = "Game is required")]
        ObservableCollection<AvailableGame>? addedGames;

        [ObservableProperty]
        string addedGamesError = string.Empty;

        [ObservableProperty]
        Venue? vendor;

        Realm? realmInstance;

        private readonly IRealmService? realmService;
        private readonly IConnectivity? connectivity;
        private readonly INavigationService? navigationService;
        private readonly IImageService? imageService;
        private readonly IPopupNavigation? popupNavigation;

        public EditVendorViewModel(
            IRealmService realmService,
            IConnectivity connectivity,
            INavigationService navigationService,
            IImageService imageService,
            IPopupNavigation popupNavigation
        )
        {
            try
            {
                this.realmService = realmService;
                this.connectivity = connectivity;
                this.navigationService = navigationService;
                this.imageService = imageService;
                this.popupNavigation = popupNavigation;

                AddedGames = new ObservableCollection<AvailableGame>();
                SelectedAmenities = new ObservableCollection<object>();
                FieldsInGame = new ObservableCollection<Field>();
                AllGames = new ObservableCollection<Game>();
                AllAmenities = new ObservableCollection<Amenity>();

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

        protected override async void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            try
            {
                base.OnPropertyChanged(e);
                if (e.PropertyName == nameof(SelectedProvince))
                {
                    await realmInstance.WriteAsync(() =>
                    {
                        Vendor.Address.Province = SelectedProvince.ProvinceName;
                    });
                }
                else if (e.PropertyName == nameof(SelectedCity))
                {
                    await realmInstance.WriteAsync(() =>
                    {
                        Vendor.Address.City = SelectedCity.CityName;
                    });
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            try
            {
                if (query.TryGetValue(nameof(UserId), out object? userIdValue))
                {
                    string? id = userIdValue.ToString();
                    if (id != null)
                    {
                        UserId = ObjectId.Parse(id);
                    }
                }
            }
            catch (Exception ex)
            {
                AppShell.Current.DisplayAlert("error", ex.Message, "OK");
            }
        }

        public async Task EditExistingVendor()
        {
            try
            {
                realmInstance = await Realm.GetInstanceAsync(realmService.Config);

                Provinces = realmInstance.All<Province>().ToList();
                AllGames = realmInstance.All<Game>().ToObservableCollection();
                AllAmenities = realmInstance.All<Amenity>().ToObservableCollection();

                Venue? savedVenue = realmInstance.Find<Venue>(UserId);
                if (savedVenue == null)
                {
                    throw new NullReferenceException(nameof(savedVenue));
                }
                if (savedVenue.Address == null)
                {
                    throw new NullReferenceException(nameof(savedVenue.Address));
                }
                // if (savedVenue.Timing == null)
                // {
                //     throw new NullReferenceException(nameof(savedVenue.Timing));
                // }

                Vendor = new Venue(
                    id: savedVenue.Id,
                    fullName: savedVenue.FullName,
                    about: savedVenue.About,
                    email: savedVenue.Email,
                    phoneCode: savedVenue.PhoneCode,
                    mobileNumber: savedVenue.MobileNumber,
                    stripeUserId: savedVenue.StripeUserId
                );

                Vendor.Address = new VenueAddress(
                    street: savedVenue.Address.Street,
                    postCode: savedVenue.Address.PostCode,
                    country: savedVenue.Address.Country,
                    province: savedVenue.Address.Province,
                    city: savedVenue.Address.City,
                    latitude: savedVenue.Address.Latitude,
                    longitude: savedVenue.Address.Longitude
                );

                Vendor.IsActive = savedVenue.IsActive;
                Vendor.IsPromoted = savedVenue.IsPromoted;

                if (Provinces.Count > 0)
                {
                    SelectedProvince = Provinces
                        .Where(n => n.ProvinceName == savedVenue.Address.Province)
                        .FirstOrDefault();
                    Cities = SelectedProvince
                        .Cities.OrderBy(c => c.CityName)
                        .ToObservableCollection();
                    if (Cities.Count > 0)
                    {
                        SelectedCity = Cities
                            .Where(n => n.CityName == savedVenue.Address.City)
                            .FirstOrDefault();
                    }
                }
                IList<Holiday> holidays = savedVenue.Holidays;
                if (holidays != null)
                {
                    foreach (Holiday holiday in holidays)
                    {
                        // Holiday is an embedded object; clone it so we don't attach a managed embedded
                        // object instance to an unmanaged Venue (which can lead to the list being cleared).
                        Vendor.Holidays.Add(new Holiday(holiday.Date, holiday.Description));
                    }
                }

                SelectedAmenities = savedVenue.Amenities.ToObservableCollection<object>();

                // AvailableGame and Field are now embedded objects; clone them so we don't
                // attach managed embedded instances to an unmanaged Venue.
                foreach (AvailableGame savedGame in savedVenue.AvailableGames)
                {
                    AvailableGame clonedGame = new AvailableGame(
                        name: savedGame.Name,
                        peoplePerGame: savedGame.PeoplePerGame,
                        fieldType: savedGame.FieldType,
                        fieldsCount: savedGame.FieldsCount,
                        weekdayHourlyRate: savedGame.WeekdayHourlyRate,
                        weekdayPeakHoursHourlyRate: savedGame.WeekdayPeakHoursHourlyRate,
                        weekendHourlyRate: savedGame.WeekendHourlyRate,
                        weekendPeakHoursHourlyRate: savedGame.WeekendPeakHoursHourlyRate
                    );
                    clonedGame.FirstImageName = savedGame.FirstImageName;
                    clonedGame.SecondImageName = savedGame.SecondImageName;
                    if (savedGame.Timing != null)
                    {
                        clonedGame.Timing = new GameTiming(
                            weekdayOpenTime: savedGame.Timing.WeekdayOpenTime,
                            weekdayCloseTime: savedGame.Timing.WeekdayCloseTime,
                            weekdayPeakHoursStartTime: savedGame.Timing.WeekdayPeakHoursStartTime,
                            weekdayPeakHoursEndTime: savedGame.Timing.WeekdayPeakHoursEndTime,
                            weekendOpenTime: savedGame.Timing.WeekendOpenTime,
                            weekendCloseTime: savedGame.Timing.WeekendCloseTime,
                            weekendPeakHoursStartTime: savedGame.Timing.WeekendPeakHoursStartTime,
                            weekendPeakHoursEndTime: savedGame.Timing.WeekendPeakHoursEndTime
                        );
                    }
                    // Clone embedded fields
                    foreach (Field savedField in savedGame.Fields)
                    {
                        clonedGame.Fields.Add(
                            new Field(savedField.FieldNumber, savedField.FieldType)
                        );
                    }
                    AddedGames.Add(clonedGame);
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

                    AvailableGame game = new AvailableGame(
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
                        Field field = new Field(fieldNumber: i, fieldType: SelectedGame.FieldType);
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
                    // Remove the game from AddedGames.
                    // Since AvailableGame is now an embedded object, it will be removed
                    // from the venue when the venue is updated.
                    AddedGames.Remove(availableGame);
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

                await EditVendor();
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
                if (Vendor == null || Vendor.Address == null)
                {
                    return true;
                }

                BusinessNameError = string.IsNullOrWhiteSpace(Vendor.FullName)
                    ? "Business name is required"
                    : string.Empty;
                StreetError = string.IsNullOrWhiteSpace(Vendor.Address.Street)
                    ? "Street is required"
                    : string.Empty;
                PostCodeError = string.IsNullOrWhiteSpace(Vendor.Address.PostCode)
                    ? "PostCode is required"
                    : string.Empty;
                ProvinceError = string.IsNullOrWhiteSpace(Vendor.Address.Province)
                    ? "Province is required"
                    : string.Empty;
                CityError = string.IsNullOrWhiteSpace(Vendor.Address.City)
                    ? "City is required"
                    : string.Empty;
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

        private async Task EditVendor()
        {
            try
            {
                await popupNavigation.PushAsync(new BusyMopup());

                // Preserve holidays: this screen doesn't edit holidays, but Realm update semantics can
                // clear list properties if the incoming object has an empty list.
                List<Holiday> holidaysToPersist = new();
                if (Vendor?.Holidays?.Any() == true)
                {
                    holidaysToPersist.AddRange(
                        Vendor.Holidays.Select(h => new Holiday(h.Date, h.Description))
                    );
                }
                else if (Vendor != null)
                {
                    Venue? existingVenue = realmInstance
                        ?.All<Venue>()
                        .FirstOrDefault(v => v.Id == Vendor.Id);

                    if (existingVenue?.Holidays?.Any() == true)
                    {
                        holidaysToPersist.AddRange(
                            existingVenue.Holidays.Select(h => new Holiday(h.Date, h.Description))
                        );
                    }
                }

                Vendor?.Amenities.Clear();

                foreach (Amenity amenity in SelectedAmenities.Cast<Amenity>())
                {
                    Vendor?.Amenities.Add(amenity);
                }

                // Upload images for games that have new image bytes
                foreach (AvailableGame game in AddedGames.Where(g => g.FirstImageBytes != null))
                {
                    string? imageName =
                        $"{Vendor.FullName}-{game.Name}-FirstImage{game.FirstImageExtension}";
                    byte[]? imageBytes = game.FirstImageBytes;
                    await AddImageToAWSS(imageBytes, imageName);

                    game.FirstImageName = imageName;

                    if (game.SecondImageBytes != null)
                    {
                        string? secondImageName =
                            $"{Vendor.FullName}-{game.Name}-SecondImage{game.SecondImageExtension}";
                        imageBytes = game.SecondImageBytes;
                        await AddImageToAWSS(imageBytes, secondImageName);

                        game.SecondImageName = secondImageName;
                    }
                }

                await realmInstance.WriteAsync(() =>
                {
                    if (Vendor == null)
                    {
                        throw new InvalidOperationException("Vendor cannot be null");
                    }

                    // Set image names on newly added games before persisting
                    foreach (AvailableGame game in AddedGames)
                    {
                        if (
                            game.FirstImageBytes != null
                            && string.IsNullOrEmpty(game.FirstImageName)
                        )
                        {
                            game.FirstImageName =
                                $"{Vendor.FullName}-{game.Name}-FirstImage{game.FirstImageExtension}";
                        }

                        if (
                            game.SecondImageBytes != null
                            && string.IsNullOrEmpty(game.SecondImageName)
                        )
                        {
                            game.SecondImageName =
                                $"{Vendor.FullName}-{game.Name}-SecondImage{game.SecondImageExtension}";
                        }
                    }

                    // Rebuild the embedded AvailableGames list on the venue
                    Vendor.AvailableGames.Clear();
                    foreach (AvailableGame game in AddedGames)
                    {
                        Vendor.AvailableGames.Add(game);
                    }

                    // Add/update the Vendor (embedded games and fields are persisted automatically)
                    Venue managedVendor = realmInstance.Add(Vendor, true);

                    // Restore holidays explicitly (screen doesn't edit them)
                    if (holidaysToPersist.Count > 0)
                    {
                        managedVendor.Holidays.Clear();
                        foreach (Holiday holiday in holidaysToPersist)
                        {
                            managedVendor.Holidays.Add(holiday);
                        }
                    }
                });

                // UI operations must be outside the Realm write transaction to avoid deadlocks/hangs.
                if (popupNavigation.PopupStack.Any())
                {
                    await popupNavigation.PopAsync();
                }

                await Shell.Current.DisplayAlert("Success", "Vendor updated successfully", "OK");
                await navigationService.PopAsync();
            }
            catch (Exception ex)
            {
                try
                {
                    if (popupNavigation.PopupStack.Any())
                    {
                        await popupNavigation.PopAsync();
                    }
                }
                catch
                {
                    // ignore pop failures
                }
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

                if (SelectedAmenities.Any())
                {
                    SelectedAmenitiesError = string.Empty;
                    foreach (Amenity amenity in SelectedAmenities.Cast<Amenity>())
                    {
                        Amenity? localAmenity = AllAmenities
                            .Where(n => n.AmenityName == amenity.AmenityName)
                            .FirstOrDefault();
                        if (localAmenity != null)
                            localAmenity.IsSelected = true;
                        amenity.IsSelected = true;
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
            try
            {
                await navigationService.NavigateToAsync("..");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}
