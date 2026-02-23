using Player.Views.OnBoarding;

namespace Player.ViewModels.Account
{
    public partial class LoginViewModel(
        INavigationService navigationService,
        IConnectivity connectivity,
        ISettingsService settingsService,
        IPopupNavigation popupNavigation
    ) : AccountBaseViewModel(navigationService, connectivity, settingsService)
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
                if (e.PropertyName == null)
                {
                    return;
                }
                if (e.PropertyName.Contains("Error") || e.PropertyName == "VisibilityImage")
                {
                    return;
                }
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
                        string? errorMessage = validationResult.ErrorMessage ?? "error";
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

                Email = Email.ToLower();
                User RealmUser = await App.RealmApp.LogInAsync(
                    Credentials.EmailPassword(Email, Password)
                );
                if (RealmUser != null)
                {
                    // check if the user is a store tester. If the user is a tester, navigate to LoadDataView, else navigate to AuthenticateEmailForLoginView.
                    if (Email == "tester@store.com")
                    {
                        await navigationService.NavigateToAsync($"///{nameof(LoadDataView)}");
                    }
                    else
                    {
                        settingsService.IsLoginAuthenticationPending = true;
                        await navigationService.NavigateToAsync(
                            $"///{nameof(AuthenticateEmailForLoginView)}"
                        );
                    }

                    await popupNavigation.PopAsync();
                    Email = string.Empty;
                    Password = string.Empty;
                }
                else
                {
                    await popupNavigation.PopAsync();
                    await AlertHelper.ShowSnackBar("Sign in fail! please try again");
                }
            }
            catch (Exception ex)
            {
                await popupNavigation.PopAsync();

                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async Task ForgotPassword()
        {
            try
            {
                await navigationService.NavigateToAsync(nameof(ForgotPasswordView));
                Email = string.Empty;
                Password = string.Empty;
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async Task SignUp()
        {
            try
            {
                await navigationService.NavigateToAsync(nameof(OnBoardPage1));

                Email = string.Empty;
                Password = string.Empty;
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}
