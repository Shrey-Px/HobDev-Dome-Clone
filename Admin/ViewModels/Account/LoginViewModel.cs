using Admin.View;

namespace Admin.ViewModels.Account
{
    public partial class LoginViewModel(
        INavigationService navigationService,
        IConnectivity connectivity,
        ISettingsService settingsService,
        IPopupNavigation popupNavigation
    ) : ObservableValidator
    {
        [Required(ErrorMessage = "Email is Empty")]
        [EmailAddress(ErrorMessage = "This email is not a valid email address")]
        [ObservableProperty]
        string email = String.Empty;

        [Required(ErrorMessage = "Password is Empty")]
        [ObservableProperty]
        string password = String.Empty;

        [ObservableProperty]
        string emailError = string.Empty;

        [ObservableProperty]
        string passwordError = string.Empty;

        private readonly INavigationService navigationService = navigationService;
        private readonly IConnectivity connectivity = connectivity;
        private readonly ISettingsService settingsService = settingsService;
        private readonly IPopupNavigation popupNavigation = popupNavigation;

        protected override async void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            try
            {
                base.OnPropertyChanged(e);
                if (e.PropertyName == null || e.PropertyName.Contains("Error"))
                {
                    return;
                }
                ValidateAllProperties();
                IEnumerable<ValidationResult> propertyErrors = GetErrors(e.PropertyName);
                if (propertyErrors.Any())
                {
                    string errorMessage = propertyErrors.First().ErrorMessage ?? "error";
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
                    case "Password":
                        PasswordError = errorMessage;
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
        private async Task Login()
        {
            try
            {
                ValidateAllProperties();
                if (HasErrors)
                {
                    IEnumerable<ValidationResult> propertyErrors = GetErrors();

                    foreach (ValidationResult validationResult in propertyErrors)
                    {
                        string? errorMessage = validationResult.ErrorMessage ?? string.Empty;
                        SetErrorValue(validationResult.MemberNames.ElementAt(0), errorMessage);
                    }
                    return;
                }

                if (connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await Shell.Current.DisplayAlert("Alert", "No Network Connection", "OK");
                    return;
                }

                await popupNavigation.PushAsync(new BusyMopup());
                string email = Email.ToLower().Trim();
                string password = Password.Trim();

                if (App.RealmApp == null)
                {
                    await Shell.Current.DisplayAlert("Error", "Please re-start the app", "OK");
                }
                else
                {
                    User realmUser = await App.RealmApp.LogInAsync(
                        Credentials.EmailPassword(Email, Password)
                    );

                    if (realmUser != null)
                    {
                        settingsService.IsEmailVerificationPending = true;
                        await DownloadDataAndNavigate();
                    }
                    else
                    {
                        await popupNavigation.PopAsync();
                        await AlertHelper.ShowSnackBar("Sign In fail! please try again");
                    }
                }
            }
            catch (Exception ex)
            {
                await popupNavigation.PopAsync();
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async Task DownloadDataAndNavigate()
        {
            try
            {
                CancellationTokenSource cts = new();
                CancellationToken cancellationToken = cts.Token;
                cts.CancelAfter(TimeSpan.FromMinutes(1));

                // the tester@storereview.com is store reviewer account which will not require authentication and Verification
                if (Email == "tester@store.com")
                {
                    settingsService.IsEmailVerificationPending = false;
                }

                await navigationService.NavigateToAsync($"///{nameof(SyncDataView)}");

                await popupNavigation.PopAsync();
                Email = string.Empty;
                Password = string.Empty;
            }
            catch (Exception ex)
            {
                await popupNavigation.PopAsync();
                if (ex.Message == "A task was canceled.")
                {
                    await Shell.Current.DisplayAlert(
                        "Error in downloading data",
                        "Try again after some time. If error persist contact the developer",
                        "OK"
                    );
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                }
            }
        }

        [RelayCommand]
        async Task ForgotPassword()
        {
            try
            {
                if (connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await Shell.Current.DisplayAlert("Alert", "No Network Connection", "OK");
                    return;
                }
                await navigationService.NavigateToAsync($"/{nameof(ForgotPasswordView)}");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}
