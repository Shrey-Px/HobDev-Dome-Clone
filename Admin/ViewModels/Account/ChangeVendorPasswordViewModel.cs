namespace Admin.ViewModels.Account
{
    public partial class ChangeVendorPasswordViewModel : ObservableValidator
    {
        [ObservableProperty]
        public string email = string.Empty;

        [ObservableProperty]
        [Required]
        [MinLength(8, ErrorMessage = "The password should have atleast 8 characters")]
        [MaxLength(14, ErrorMessage = "The password should not be more than 14 characters")]
        [NotifyPropertyChangedFor(nameof(ConfirmPassword))]
        string password = string.Empty;

        string confirmPassword = string.Empty;

        [Required]
        [Compare(nameof(Password), ErrorMessage = "The confirm password is not equal to password")]
        public string ConfirmPassword
        {
            get => confirmPassword;
            set => SetProperty(ref confirmPassword, value, true);
        }

        [ObservableProperty]
        string passwordError = string.Empty;

        [ObservableProperty]
        string confirmPasswordError = string.Empty;

        private readonly IConnectivity? connectivity;
        private readonly IPopupNavigation? popupNavigation;

        public ChangeVendorPasswordViewModel(
            IConnectivity connectivity,
            IPopupNavigation popupNavigation
        )
        {
            try
            {
                this.connectivity = connectivity;
                this.popupNavigation = popupNavigation;
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

                if (e == null || e.PropertyName == null || e.PropertyName.Contains("Error"))
                {
                    return;
                }
                ClearErrors(e.PropertyName);
                ValidateAllProperties();
                IEnumerable<ValidationResult> propertyErrors = GetErrors(e.PropertyName);
                if (propertyErrors.Any())
                {
                    string? errorMessage = propertyErrors.First().ErrorMessage ?? "error";
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
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async void SetErrorValue(string propertyName, string errorMessage)
        {
            try
            {
                switch (propertyName)
                {
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
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        private async Task ResetPassword()
        {
            try
            {
                ValidateAllProperties();
                if (HasErrors)
                {
                    IEnumerable<ValidationResult> propertyErrors = GetErrors();
                    if (propertyErrors.Any())
                    {
                        foreach (ValidationResult validationResult in propertyErrors)
                        {
                            SetErrorValue(
                                validationResult.MemberNames.ElementAt(0),
                                validationResult.ErrorMessage
                            );
                        }
                        return;
                    }
                }

                if (connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await Shell.Current.DisplayAlert("Alert", "No Network Connection", "OK");
                    return;
                }

                await popupNavigation.PushAsync(new BusyMopup());

                await ResetVendorPasswordAsync();

                await AlertHelper.ShowSnackBar("The new password has been reset");

                await popupNavigation.PopAsync();

                Email = string.Empty;
            }
            catch (Exception ex)
            {
                await popupNavigation.PopAsync();
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async Task ResetVendorPasswordAsync()
        {
            AppConfiguration? appConfiguration = null;

            string environment = AppConstants.Environment;
            if (environment == "development")
            {
                appConfiguration = new AppConfiguration("vendor_dev-bmltl");
            }
            else if (environment == "production")
            {
                appConfiguration = new AppConfiguration("vendor_prod-iozmv");
            }

            Realms.Sync.App AdminRealmApp = Realms.Sync.App.Create(appConfiguration);
            // reset password using function
            await AdminRealmApp.EmailPasswordAuth.CallResetPasswordFunctionAsync(Email, Password);
        }
    }
}
