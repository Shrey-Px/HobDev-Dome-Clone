namespace Admin.Services.Interfaces
{
    public interface ISettingsService
    {
        bool IsEmailVerificationPending { get; set; }
    }
}
