namespace Dome.Shared.Synced
{
    /// <summary>
    /// amenities provided by the venues
    /// </summary>
    public partial class Amenity : IRealmObject
    {
        private Amenity() { }

        public Amenity(ObjectId id, string amenityName)
        {
            Id = id;
            AmenityName = amenityName;
        }

        [PrimaryKey]
        [MapTo("_id")]
        public ObjectId Id { get; set; }

        [MapTo("amenityName")]
        public string AmenityName { get; set; }

        [MapTo("amenityImage")]
        public byte[] AmenityImage { get; set; }

        //local

        private bool isSelected;

        // being used in admin app for highlighting the selected amenities while creating and editing a venue
        public bool IsSelected
        {
            get => isSelected;
            set
            {
                if (value != isSelected)
                {
                    isSelected = value;
                    RaisePropertyChanged();
                }
            }
        }
    }
}
