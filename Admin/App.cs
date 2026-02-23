namespace Admin
{
    public class App : Application
    {
        private string? appId;

        public static Realms.Sync.App? RealmApp { get; private set; }

        private readonly ISecretsService? secretsService;
        private readonly ISettingsService? settingsService;

        public App(ISettingsService settingsService, ISecretsService secretsService)
        {
            try
            {
                this.secretsService = secretsService;
                this.settingsService = settingsService;
                SetLanguage();
                Task.Run(async () => await secretsService.LoadAllSecrets())
                    .GetAwaiter()
                    .GetResult();
                string? appId = secretsService.RealmAdminAppId;
                if (appId == null)
                {
                    throw new Exception("appId  is null");
                }
                AppConfiguration appConfiguration = new(appId)
                {
#if WINDOWS
                    BaseFilePath = Environment.GetFolderPath(
                        Environment.SpecialFolder.ApplicationData
                    ),
#endif
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
                Shell.Current.DisplayAlert("Error", ex.Message, "OK");
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

        protected override Window CreateWindow(IActivationState? activationState)
        {
            User? realmUser = RealmApp.CurrentUser;

            if (
                realmUser == null
                || realmUser.State == UserState.LoggedOut
                || realmUser.State == UserState.Removed
            )
            {
                return new Window(new AppShell("Login", new AppShellViewModel()));
            }
            else
            {
                return new Window(new AppShell(null, new AppShellViewModel()));
            }
        }
    }
}
