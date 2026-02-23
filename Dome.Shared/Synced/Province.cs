namespace Dome.Shared.Synced
{
    /// <summary>
    /// the province is a class that represents the province where the Sport Venue is located. The provinces are pre stored in the database and the admin select the province where the venue is located from all the stored provinces while creating the venue.
    /// </summary>
    public partial class Province : IRealmObject
    {
        private Province() { }

        public Province(ObjectId id, string provinceName)
        {
            Id = id;
            ProvinceName = provinceName;
        }

        [PrimaryKey]
        [MapTo("_id")]
        public ObjectId Id { get; set; }

        [MapTo("provinceName")]
        public string ProvinceName { get; set; }

        //embedded
        [MapTo("cities")]
        public IList<City> Cities { get; }
    }
}
