namespace Dome.Shared.Synced
{
    /// <summary>
    /// the address of a venue embedded in the venue object
    /// </summary>
    public partial class VenueAddress : IEmbeddedObject
    {
        private VenueAddress() { }

        public VenueAddress(
            string street,
            string postCode,
            string country,
            string province,
            string city,
            double latitude,
            double longitude
        )
        {
            Street = street;
            PostCode = postCode;
            Country = country;
            Province = province;
            City = city;
            Latitude = latitude;
            Longitude = longitude;
        }

        [MapTo("street")]
        public string Street { get; set; }

        [MapTo("postCode")]
        public string PostCode { get; set; }

        [MapTo("country")]
        public string Country { get; set; }

        [MapTo("province")]
        public string Province { get; set; }

        [MapTo("city")]
        public string City { get; set; }

        [MapTo("latitude")]
        public double Latitude { get; set; }

        [MapTo("longitude")]
        public double Longitude { get; set; }
    }
}
