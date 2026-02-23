namespace Dome.Shared.Synced
{
    /// <summary>
    /// the city is a class that represents the city where the Sport Venue is located. The cities are pre stored in the database and the admin select the city where the venue is located from all the stored cities while creating the venue.
    /// </summary>
    public partial class City : IEmbeddedObject
    {
        private City() { }

        public City(string cityName)
        {
            CityName = cityName;
        }

        [MapTo("cityName")]
        public string CityName { get; set; }
    }
}
