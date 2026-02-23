namespace Player.Services.Interfaces
{
    public interface ILocationService
    {
        Task<Placemark?> GetCurrentLocation();
    }
}
