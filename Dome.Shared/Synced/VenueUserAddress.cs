namespace Dome.Shared.Synced
{
    /// <summary>
    /// the player address embedded in the player document
    /// </summary>
    public partial class VenueUserAddress : IEmbeddedObject
    {
        private VenueUserAddress() { }

        public VenueUserAddress(string country, string province, string city)
        {
            Country = country;
            Province = province;
            City = city;
        }

        [MapTo("country")]
        public string Country { get; set; }

        [MapTo("province")]
        public string Province { get; set; }

        [MapTo("city")]
        public string City { get; set; }

        public string CompleteAddress => $"{City}, {Province}, {Country}";
    }
}
