namespace Admin.ViewModels.Account
{
    public partial class ForgotPasswordViewModel : ObservableValidator
    {
        [Required(ErrorMessage = "Email is Empty")]
        [EmailAddress(ErrorMessage = "This email is not a valid email address")]
        [ObservableProperty]
        string email = String.Empty;

        [ObservableProperty]
        [Required]
        [MinLength(8, ErrorMessage = "The password should have atleast 8 characters")]
        [MaxLength(14, ErrorMessage = "The password should not be more than 14 characters")]
        [NotifyPropertyChangedFor(nameof(ConfirmNewPassword))]
        string newPassword = String.Empty;

        string confirmNewPassword = String.Empty;

        [Required]
        [Compare(
            nameof(NewPassword),
            ErrorMessage = "The confirm password is not equal to password"
        )]
        public string ConfirmNewPassword
        {
            get => confirmNewPassword;
            set => SetProperty(ref confirmNewPassword, value, true);
        }

        [ObservableProperty]
        string emailError;

        [ObservableProperty]
        string newPasswordError;

        [ObservableProperty]
        string confirmNewPasswordError = string.Empty;

        private readonly IConnectivity connectivity;
        private readonly INavigationService navigationService;
        private readonly IRealmService realmService;

        public ForgotPasswordViewModel(
            IRealmService realmService,
            IConnectivity connectivity,
            INavigationService navigationService
        )
        {
            try
            {
                this.realmService = realmService;
                this.connectivity = connectivity;
                this.navigationService = navigationService;
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
                if (
                    e == null
                    || e.PropertyName == null
                    || e.PropertyName.Contains("Error")
                    || e.PropertyName == "VisibilityImage"
                )
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
                    case "Email":
                        EmailError = errorMessage;
                        break;
                    case "NewPassword":
                        NewPasswordError = errorMessage;
                        break;
                    case "ConfirmNewPassword":
                        ConfirmNewPasswordError = errorMessage;
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

                await navigationService.NavigateToAsync(
                    nameof(VerifyEmailForPasswordResetView),
                    new ShellNavigationQueryParameters
                    {
                        { "UserEmail", Email },
                        { "NewPassword", NewPassword },
                    }
                );

                Email = string.Empty;
                NewPassword = string.Empty;
                ConfirmNewPassword = string.Empty;
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async Task CancelPasswordReset()
        {
            try
            {
                await navigationService.NavigateToAsync($"///Login");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}
