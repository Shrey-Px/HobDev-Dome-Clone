namespace Player.Services.Interfaces
{
    public interface ISettingsService
    {
        bool IsLoginAuthenticationPending { get; set; }

        bool IsDarkThemeWantedByUser { get; set; }
    }
}
