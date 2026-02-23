namespace Player.ViewModels.Account
{
    public partial class RegisterViewModel : AccountBaseViewModel
    {
        [ObservableProperty]
        [Required(ErrorMessage = "Email is Empty")]
        [EmailAddress(ErrorMessage = "This email is not a valid email address")]
        string email = String.Empty;

        [ObservableProperty]
        [Required]
        [MinLength(8, ErrorMessage = "The password should have atleast 8 characters")]
        [MaxLength(14, ErrorMessage = "The password should not be more than 14 characters")]
        [NotifyPropertyChangedFor(nameof(ConfirmPassword))]
        string password = String.Empty;

        string confirmPassword = String.Empty;

        [Required]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword
        {
            get => confirmPassword;
            set => SetProperty(ref confirmPassword, value, true);
        }

        [ObservableProperty]
        string emailError = string.Empty;

        [ObservableProperty]
        string passwordError = string.Empty;

        [ObservableProperty]
        string confirmPasswordError = string.Empty;

        private readonly INavigationService navigationService;
        private readonly IConnectivity connectivity;
        private readonly IPopupNavigation? popupNavigation;

        public RegisterViewModel(
            INavigationService navigationService,
            IConnectivity connectivity,
            ISettingsService settingsService,
            IPopupNavigation? popupNavigation
        )
            : base(navigationService, connectivity, settingsService)
        {
            try
            {
                this.navigationService = navigationService;
                this.connectivity = connectivity;
                this.popupNavigation = popupNavigation;
            }
            catch (Exception ex)
            {
                Shell.Current.DisplayAlert("Error", ex.Message, "OK");
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
                    string? errorMessage = propertyErrors.FirstOrDefault()?.ErrorMessage;
                    if (errorMessage != null)
                        SetErrorValue(e.PropertyName, errorMessage);
                    else
                        SetErrorValue(e.PropertyName, "error");
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
                    case nameof(Email):
                        EmailError = errorMessage;
                        break;
                    case nameof(Password):
                        PasswordError = errorMessage;
                        break;
                    case nameof(ConfirmPassword):
                        ConfirmPasswordError = errorMessage;
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
        async Task RegisterUser()
        {
            try
            {
                await popupNavigation.PushAsync(new BusyMopup());
                ValidateAllProperties();
                if (HasErrors)
                {
                    IEnumerable<ValidationResult> propertyErrors = GetErrors();

                    foreach (ValidationResult result in propertyErrors)
                    {
                        if (result.ErrorMessage != null)
                            SetErrorValue(result.MemberNames.ElementAt(0), result.ErrorMessage);
                        else
                            SetErrorValue(result.MemberNames.ElementAt(0), "error");
                    }
                }
                else if (connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await Shell.Current.DisplayAlert("Alert", "No Network Connection", "OK");
                }
                else
                {
                    Email = Email.ToLower();
                    await navigationService.NavigateToAsync(
                        nameof(ConfirmEmailToRegisterItView),
                        new ShellNavigationQueryParameters
                        {
                            { "UserEmail", Email },
                            { "Password", Password },
                        }
                    );
                    Email = string.Empty;
                    Password = string.Empty;
                    ConfirmPassword = string.Empty;
                }

                await popupNavigation.PopAsync();
            }
            catch (Exception ex)
            {
                await popupNavigation.PopAsync();
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async Task SignIn()
        {
            try
            {
                await navigationService.NavigateToAsync($"///{nameof(LoginView)}");
                Email = string.Empty;
                Password = string.Empty;
                ConfirmPassword = string.Empty;
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}
