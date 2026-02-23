namespace Player.Services.Implementations
{
    public class SettingsService : ISettingsService
    {
        // keys
        private const string IsLoginAuthenticationPendingKey = "isLoginAuthenticationPending";
        private const string IsDarkThemeWantedByUserKey = "isDarkThemeWantedByUser";

        //Dafault Values
        private readonly bool IsLoginAuthenticationPendingDefault = false;
        private readonly bool IsDarkThemeWantedByUserDefault = true;

        //Properties
        public bool IsLoginAuthenticationPending
        {
            get =>
                Preferences.Get(
                    IsLoginAuthenticationPendingKey,
                    IsLoginAuthenticationPendingDefault
                );
            set => Preferences.Set(IsLoginAuthenticationPendingKey, value);
        }

        public bool IsDarkThemeWantedByUser
        {
            get => Preferences.Get(IsDarkThemeWantedByUserKey, IsDarkThemeWantedByUserDefault);
            set => Preferences.Set(IsDarkThemeWantedByUserKey, value);
        }
    }
}
