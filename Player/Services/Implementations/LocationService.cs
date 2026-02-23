namespace Player.Services.Implementations
{
    public class LocationService : ILocationService
    {
        private readonly IGeolocation geolocation;
        private readonly IGeocoding geocoding;

        public LocationService(IGeolocation geolocation, IGeocoding geocoding)
        {
            this.geolocation = geolocation;
            this.geocoding = geocoding;
        }

        public async Task<Placemark?> GetCurrentLocation()
        {
            Location? location = await geolocation.GetLastKnownLocationAsync();
            if (location == null)
            {
                location = await geolocation.GetLocationAsync(
                    new GeolocationRequest
                    {
                        DesiredAccuracy = GeolocationAccuracy.Medium,
                        Timeout = TimeSpan.FromSeconds(30),
                    }
                );
            }
            if (location != null)
            {
                IEnumerable<Placemark>? placemarks = await geocoding.GetPlacemarksAsync(
                    location.Latitude,
                    location.Longitude
                );
                return placemarks?.FirstOrDefault();
            }
            return null;
        }
    }
}
