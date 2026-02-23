using Player.Models.Chat;
using Stripe;

namespace Player.ViewModels.Account
{
    public partial class NewProfileViewModel : AccountBaseViewModel
    {
        [ObservableProperty]
        byte[]? playerImage;

        [ObservableProperty]
        VenueUser? venueUser;

        [ObservableProperty]
        List<Province>? allProvinces;

        [ObservableProperty]
        [Required(ErrorMessage = "Province is Empty")]
        Province? selectedProvince;

        [ObservableProperty]
        string provinceError = string.Empty;

        [ObservableProperty]
        ObservableCollection<City>? cities;

        [ObservableProperty]
        [Required(ErrorMessage = "City is Empty")]
        City? selectedCity;

        [ObservableProperty]
        string cityError = string.Empty;

        [ObservableProperty]
        [Required(ErrorMessage = "First name is Empty")]
        string firstName = String.Empty;

        [ObservableProperty]
        [Required(ErrorMessage = "Last name is Empty")]
        string lastName = String.Empty;

        [ObservableProperty]
        string firstNameError = string.Empty;

        [ObservableProperty]
        string lastNameError = string.Empty;

        [ObservableProperty]
        [Required(ErrorMessage = "Number is Empty")]
        [Phone(ErrorMessage = "Not correct Number")]
        string mobileNumber = String.Empty;

        [ObservableProperty]
        string mobileNumberError = string.Empty;

        [ObservableProperty]
        Country? selectedCountry;

        Realm? realmInstance;

        private readonly INavigationService? navigationService;
        private readonly IRealmService? realmService;
        private readonly IConnectivity? connectivity;
        private readonly IImageService? imageService;
        private readonly ILocationService? locationService;
        private readonly IPopupNavigation? popupNavigation;
        private readonly ISecretsService? secretsService;
        private readonly IStripeService? stripeService;

        private readonly IChatService? chatService;

        public NewProfileViewModel(
            INavigationService navigationService,
            IRealmService realmService,
            IConnectivity connectivity,
            ISettingsService settingsService,
            IImageService imageService,
            ILocationService locationService,
            IPopupNavigation? popupNavigation,
            ISecretsService? secretsService,
            IStripeService? stripeService,
            IChatService chatService
        )
            : base(navigationService, connectivity, settingsService)
        {
            try
            {
                this.navigationService = navigationService;
                this.realmService = realmService;
                this.connectivity = connectivity;
                this.imageService = imageService;
                this.locationService = locationService;
                this.popupNavigation = popupNavigation;
                this.secretsService = secretsService;
                this.stripeService = stripeService;
                this.chatService = chatService;

                // at the moment only Canada is supported
                SelectedCountry = new Country(countryName: "Canada", code: "+1", flagImage: "🇨🇦");

                AllProvinces = new List<Province>();
                Cities = new ObservableCollection<City>();
            }
            catch (Exception ex)
            {
                Shell.Current.DisplayAlert("Error", ex.Message, "Ok");
            }
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            try
            {
                base.OnPropertyChanged(e);
                if (e.PropertyName == null)
                {
                    return;
                }
                if (e.PropertyName.Contains("Error"))
                    return;
                ClearErrors(e.PropertyName);
                ValidateAllProperties();
                IEnumerable<ValidationResult> propertyErrors = GetErrors(e.PropertyName);
                if (propertyErrors.Any())
                {
                    string? errorMessage = propertyErrors?.FirstOrDefault()?.ErrorMessage;
                    if (errorMessage != null)
                    {
                        SetErrorValue(e.PropertyName, errorMessage);
                    }
                    else
                    {
                        SetErrorValue(e.PropertyName, "error");
                    }
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
                    case nameof(FirstName):
                        FirstNameError = errorMessage;
                        break;
                    case nameof(LastName):
                        LastNameError = errorMessage;
                        break;
                    case nameof(MobileNumber):
                        MobileNumberError = errorMessage;
                        break;
                    case nameof(SelectedProvince):
                        ProvinceError = errorMessage;
                        break;
                    case nameof(SelectedCity):
                        CityError = errorMessage;
                        break;
                }
            }
            catch (Exception ex)
            {
                Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        public async Task InitializeAsync()
        {
            try
            {
                realmInstance = await Realm.GetInstanceAsync(realmService.Config);
                AllProvinces = realmInstance.All<Province>().ToList();

                SelectedProvince = AllProvinces.FirstOrDefault();
                if (AllProvinces.Count > 0)
                {
                    Cities = SelectedProvince
                        ?.Cities.OrderBy(c => c.CityName)
                        .ToObservableCollection();
                    SelectedCity = Cities?.FirstOrDefault();
                }

                if (AllProvinces.Count > 0)
                {
                    await GetLocation();
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        /// <summary>
        /// set province and city based on user location
        /// </summary>
        /// <returns></returns>
        async Task GetLocation()
        {
            try
            {
                PermissionStatus status = await CheckAndRequestLocationPermission();
                if (status != PermissionStatus.Granted)
                {
                    return;
                }
                Placemark? placemark = await locationService.GetCurrentLocation();

                if (placemark != null)
                {
                    string? currentCity = placemark.Locality;
                    if (currentCity != null)
                    {
                        Province? currentProvince = AllProvinces?.FirstOrDefault(p =>
                            p.Cities.Any(c => c.CityName == currentCity)
                        );
                        if (currentProvince != null)
                        {
                            SelectedProvince = currentProvince;
                            SelectedCity = Cities?.FirstOrDefault(c => c.CityName == currentCity);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        public async Task<PermissionStatus> CheckAndRequestLocationPermission()
        {
            PermissionStatus status =
                await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

            if (status == PermissionStatus.Granted)
                return status;

            if (status == PermissionStatus.Denied && DeviceInfo.Platform == DevicePlatform.iOS)
            {
                // Prompt the user to turn on in settings
                // On iOS once a permission has been denied it may not be requested again from the application
                return status;
            }

            if (Permissions.ShouldShowRationale<Permissions.LocationWhenInUse>())
            {
                // Prompt the user with additional information as to why the permission is needed
            }

            status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

            return status;
        }

        [RelayCommand]
        async Task AddImage()
        {
            try
            {
                if (imageService == null)
                {
                    throw new NullReferenceException(nameof(imageService));
                }
                else
                {
                    Dictionary<string, object>? imageResult = await imageService.PickImageAsync();

                    if (imageResult != null)
                    {
                        byte[]? imageBytes = (byte[])imageResult["image"];
                        if (imageBytes != null)
                        {
                            PlayerImage = imageBytes;
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

                string email = App.RealmApp?.CurrentUser?.Profile?.Email;

                // add stripe customer
                string? customerId = string.Empty;
                CustomerCreateRequest customerCreateRequest = new CustomerCreateRequest(
                    customerName: $"{FirstName} {LastName}",
                    customerEmail: email,
                    environment: AppConstants.Environment
                );
                CustomerCreateResponse? apiResponse = await stripeService.CreateCustomer(
                    customerCreateRequest
                );
                if (apiResponse?.ClientId != null)
                {
                    customerId = apiResponse?.ClientId;
                }
                else
                {
                    await popupNavigation.PopAsync();
                    await Shell.Current.DisplayAlert("API Error", apiResponse?.Error, "OK");
                    return;
                }

                // add twilio conversation participant
                ParticipantCreateResponse participantCreateResponse =
                    await chatService.CreateParticipant(email);

                VenueUserAddress Address = new VenueUserAddress(
                    country: SelectedCountry.CountryName,
                    province: SelectedProvince.ProvinceName,
                    city: SelectedCity.CityName
                );

                VenueUser venueUser = new VenueUser(
                    id: ObjectId.GenerateNewId(),
                    ownerId: realmService.RealmUser.Id,
                    email: email,
                    firstName: FirstName,
                    lastName: LastName,
                    mobileNumber: MobileNumber,
                    phoneCode: SelectedCountry.Code,
                    country: SelectedCountry.CountryName,
                    address: Address,
                    stripeCustomerId: customerId
                );

                if (PlayerImage != null)
                {
                    venueUser.ProfileImage = PlayerImage;
                }

                await realmInstance.WriteAsync(() =>
                {
                    realmInstance.Add(venueUser);
                });

                await navigationService.NavigateToAsync(nameof(NewAgeAndInterestView));

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
