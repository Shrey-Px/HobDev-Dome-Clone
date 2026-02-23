namespace Admin.ViewModels.Account
{
    public partial class ChangePasswordViewModel : ObservableValidator
    {
        [ObservableProperty]
        public string? email;

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

        private readonly IConnectivity connectivity;
        private readonly IRealmService realmService;
        private readonly INavigationService navigationService;
        private readonly IPopupNavigation popupNavigation;

        public ChangePasswordViewModel(
            IRealmService realmService,
            IConnectivity connectivity,
            INavigationService navigationService,
            IPopupNavigation popupNavigation
        )
        {
            try
            {
                this.realmService = realmService;
                this.connectivity = connectivity;
                this.navigationService = navigationService;
                this.popupNavigation = popupNavigation;

                Email = App.RealmApp?.CurrentUser?.Profile.Email;
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
                if (e.PropertyName.Contains("Error"))
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

                // the tester@store.com is login Id for store reviewer. It should by pass the email & sms verification and authentication.
                if (Email == "tester@store.com")
                {
                    await popupNavigation.PushAsync(new BusyMopup());
                    await App.RealmApp.EmailPasswordAuth.CallResetPasswordFunctionAsync(
                        Email,
                        Password
                    );
                    User? realmUser = App.RealmApp.CurrentUser;
                    if (realmUser != null)
                    {
                        await realmUser.LogOutAsync();
                    }
                    await popupNavigation.PopAsync();
                    await AlertHelper.ShowSnackBar("The new password has been reset");
                    await navigationService.NavigateToAsync($"///Login");
                }
                else
                {
                    await navigationService.NavigateToAsync(
                        nameof(VerifyEmailForPasswordResetView),
                        new ShellNavigationQueryParameters
                        {
                            { "UserEmail", Email },
                            { "NewPassword", Password },
                        }
                    );
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
}
