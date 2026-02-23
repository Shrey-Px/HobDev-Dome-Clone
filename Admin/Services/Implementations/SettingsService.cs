namespace Admin.Services.Implementations
{
    public class SettingsService : ISettingsService
    {
        // keys
        private const string IsEmailVerificationPendingKey = "isEmailVerificationPending";

        //Dafault Values
        private readonly bool IsEmailVerificationPendingDefault = false;

        //Properties
        public bool IsEmailVerificationPending
        {
            get =>
                Preferences.Get(IsEmailVerificationPendingKey, IsEmailVerificationPendingDefault);
            set => Preferences.Set(IsEmailVerificationPendingKey, value);
        }
    }
}
