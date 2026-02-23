namespace Player.ViewModels.Account;

public partial class EditProfileViewModel : AccountBaseViewModel
{
    [ObservableProperty]
    byte[]? playerImage;

    [ObservableProperty]
    List<Province>? allProvinces;

    [ObservableProperty]
    [Required(ErrorMessage = "Province is Empty")]
    Province? selectedProvince;

    [ObservableProperty]
    ObservableCollection<City>? cities;

    [ObservableProperty]
    [Required(ErrorMessage = "City is Empty")]
    City? selectedCity;

    [ObservableProperty]
    [Required(ErrorMessage = "First name is Empty")]
    string firstName = String.Empty;

    [ObservableProperty]
    [Required(ErrorMessage = "Last name is Empty")]
    string lastName = string.Empty;

    [ObservableProperty]
    string firstNameError = string.Empty;

    [ObservableProperty]
    string lastNameError = string.Empty;

    [ObservableProperty]
    string provinceError = string.Empty;

    [ObservableProperty]
    string cityError = string.Empty;

    private readonly CountryPopup? countryPopup;

    VenueUser? thisPlayer;

    Realm? realmInstance;

    private readonly INavigationService? navigationService;
    private readonly IRealmService? realmService;
    private readonly IConnectivity? connectivity;
    private readonly IImageService? imageService;
    private readonly IPopupNavigation? popupNavigation;

    public EditProfileViewModel(
        INavigationService navigationService,
        IRealmService realmService,
        IConnectivity connectivity,
        ISettingsService settingsService,
        IImageService imageService,
        IPopupNavigation? popupNavigation
    )
        : base(navigationService, connectivity, settingsService)
    {
        try
        {
            this.navigationService = navigationService;
            this.realmService = realmService;
            this.connectivity = connectivity;
            this.imageService = imageService;
            this.popupNavigation = popupNavigation;
        }
        catch (Exception ex)
        {
            Shell.Current.DisplayAlert("Error", ex.Message, "Ok");
        }
    }

    public async Task InitializeAsync()
    {
        realmInstance = Realm.GetInstance(realmService.Config);
        AllProvinces = realmInstance.All<Province>().ToList();
        thisPlayer = realmInstance
            .All<VenueUser>()
            .Where(n => n.OwnerId == realmService.RealmUser.Id)
            .FirstOrDefault();
        SelectedProvince = AllProvinces?.FirstOrDefault(p =>
            p.ProvinceName == thisPlayer.Address.Province
        );
        Cities = SelectedProvince.Cities.OrderBy(c => c.CityName).ToObservableCollection();
        SelectedCity = Cities?.FirstOrDefault(c => c.CityName == thisPlayer.Address.City);
        FirstName = thisPlayer.FirstName;
        LastName = thisPlayer.LastName;
        PlayerImage = thisPlayer.ProfileImage;
    }

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        try
        {
            base.OnPropertyChanged(e);
            if (e.PropertyName.Contains("Error"))
                return;
            ClearErrors(e.PropertyName);
            ValidateAllProperties();
            IEnumerable<ValidationResult> propertyErrors = GetErrors(e.PropertyName);
            if (propertyErrors.Any())
            {
                string? errorMessage = propertyErrors.FirstOrDefault()?.ErrorMessage ?? "error";
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
                case nameof(FirstName):
                    FirstNameError = errorMessage;
                    break;
                case nameof(LastName):
                    LastNameError = errorMessage;
                    break;
                case nameof(SelectedProvince):
                    ProvinceError = errorMessage;
                    break;
                case nameof(SelectedCity):
                    CityError = errorMessage;
                    break;
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
                        SetErrorValue(result.MemberNames.ElementAt(0), result.ErrorMessage);
                    }
                }
            }
            else
            {
                await popupNavigation.PushAsync(new BusyMopup());

                realmInstance = Realm.GetInstance(realmInstance.Config);

                await realmInstance.WriteAsync(() =>
                {
                    thisPlayer.FirstName = FirstName;
                    thisPlayer.LastName = LastName;
                    thisPlayer.Address.Province = SelectedProvince.ProvinceName;
                    thisPlayer.Address.City = SelectedCity.CityName;
                    if (PlayerImage != null)
                    {
                        thisPlayer.ProfileImage = PlayerImage;
                    }
                });

                await navigationService.PopAsync();

                await popupNavigation.PopAsync();

                realmInstance.Dispose();
            }
        }
        catch (Exception ex)
        {
            await popupNavigation.PopAsync();
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
    }

    [RelayCommand]
    async Task Cancel()
    {
        await navigationService.PopAsync();
    }
}
