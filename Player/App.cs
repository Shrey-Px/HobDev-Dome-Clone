using Dome.Shared.Synced;

namespace Player
{
    public class App : Application
    {
        public static Realms.Sync.App RealmApp { get; set; }

        private readonly ISecretsService secretsService;
        private readonly ISettingsService settingsService;
        private readonly IConnectivity connectivity;

        public App(
            ISettingsService settingsService,
            ISecretsService secretsService,
            IVersionTracking versionTracking,
            IConnectivity connectivity
        )
        {
            try
            {
                SetLanguage();
                this.secretsService = secretsService;
                this.settingsService = settingsService;
                this.connectivity = connectivity;

                // check if the app is launched for the first time. If it is, set the IsFirstTimeAppOpened to true and IsDarkThemeWantedByUser to true. If the app is launched for the first time the user will be presented with Onboarding screen before registration page
                if (versionTracking.IsFirstLaunchEver)
                {
                    settingsService.IsDarkThemeWantedByUser = true;
                }

                // check if the user wants the dark theme. If the user wants the dark theme, set the UserAppTheme to dark, else set it to light. The theme is selected by user from the Settings page
                if (settingsService.IsDarkThemeWantedByUser)
                {
                    UserAppTheme = AppTheme.Dark;
                }
                else
                {
                    UserAppTheme = AppTheme.Light;
                }

                Task.Run(async () => await secretsService.LoadAllSecrets())
                    .GetAwaiter()
                    .GetResult();
                string? appId = secretsService.RealmPlayerAppId;
                if (string.IsNullOrEmpty(appId))
                {
                    throw new InvalidOperationException(
                        "RealmPlayerAppId is missing from appsettings.json. Please ensure the configuration file contains a valid 'realmPlayerAppId' value.");
                }
                AppConfiguration appConfiguration = new Realms.Sync.AppConfiguration(appId)
                {
                    BaseUri = new Uri("https://services.cloud.mongodb.com"),
                    // the user will be logged in automatically if the user is already logged in
                };
                // Always put log before App.Create
#if DEBUG
                // reveal information that is useful for debugging, no longer paying attention to efficiency
                Realms.Logging.RealmLogger.SetLogLevel(Realms.Logging.LogLevel.Debug);
#else

                // log only errors in release mode
                Realms.Logging.RealmLogger.SetLogLevel(Realms.Logging.LogLevel.Error);
#endif

                RealmApp = Realms.Sync.App.Create(appConfiguration);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"App initialization error: {ex}");
                Console.WriteLine($"App initialization error: {ex}");
#if ANDROID
                Android.Util.Log.Error("DomeApp", $"App initialization error: {ex}");
#endif
            }
        }

        private void SetLanguage()
        {
            // the language and culture to be used in the application is set to canadian english
            string language = "en-CA";
            // intended to be used for parsing / formatting stuffs.Date and time, numbers and string comparison etc. Use four letter code separated by -, e.g "de-DE". First two letters are language code and last two letters are country code
            Thread.CurrentThread.CurrentCulture = new CultureInfo(language);
            //Used by the resource manager to look up specific resources at runtime, it's the language used to display text.Use two letter code e.g "de"
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
            //AppResources.Culture = new CultureInfo(language);
        }

        // evaluate this function if this could be used to received deep link from the user
        protected override async void OnAppLinkRequestReceived(Uri uri)
        {
            base.OnAppLinkRequestReceived(uri);

            // Show an alert to test the app link worked

            string Id = uri.Segments[uri.Segments.Length - 1];
            ObjectId bookingId = ObjectId.Parse(Id);

            await this.Dispatcher.DispatchAsync(async () =>
            {
                await Shell.Current.GoToAsync(
                    nameof(ReviewGameBeforeJoiningPopup),
                    new ShellNavigationQueryParameters { { "Id", bookingId } }
                );
            });

            // Console.WriteLine("APP LINK: " + uri.ToString());
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            if (RealmApp == null)
            {
                return new Window(new AppShell("Login"));
            }

            User? realmUser = RealmApp.CurrentUser;

            if (
                realmUser == null
                || realmUser.State == UserState.Removed
                || realmUser.State == UserState.LoggedOut
            )
            {
                return new Window(new AppShell("Login"));
            }
            else if (settingsService.IsLoginAuthenticationPending == true)
            {
                return new Window(new AppShell("EmailAuthentication"));
            }
            else
            {
                return new Window(new AppShell("LoggedIn"));
            }
        }
    }
}
