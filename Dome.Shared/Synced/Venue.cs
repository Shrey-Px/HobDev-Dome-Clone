namespace Dome.Shared.Synced
{
    /// <summary>
    /// the venue class represents the venue entity in the database. Venues are the places where the games are played.
    /// </summary>
    public partial class Venue : IRealmObject
    {
        private Venue() { }

        public Venue(
            ObjectId id,
            string fullName,
            string about,
            string email,
            string phoneCode,
            string mobileNumber,
            string stripeUserId
        )
        {
            Id = id;
            FullName = fullName;
            About = about;
            Email = email;
            PhoneCode = phoneCode;
            MobileNumber = mobileNumber;
            StripeUserId = stripeUserId;
        }

        [PrimaryKey]
        [MapTo("_id")]
        public ObjectId Id { get; set; }

        [MapTo("fullName")]
        public string FullName { get; set; }

        [MapTo("about")]
        public string About { get; set; }

        [MapTo("eMail")]
        public string Email { get; set; }

        [MapTo("phoneCode")]
        public string PhoneCode { get; set; }

        [MapTo("mobileNumber")]
        public string MobileNumber { get; set; }

        [MapTo("isActive")]
        public bool IsActive { get; set; }

        [MapTo("isPromoted")]
        public bool IsPromoted { get; set; }

        [MapTo("stripeUserId")]
        public string StripeUserId { get; set; }

        // embedded objects
        [MapTo("address")]
        public VenueAddress? Address { get; set; }

        [MapTo("holidays")]
        public IList<Holiday> Holidays { get; }

        /// <summary>
        /// Embedded available games at this venue.
        /// </summary>
        [MapTo("availableGames")]
        public IList<AvailableGame> AvailableGames { get; }

        [MapTo("bookings")]
        [Backlink(nameof(Booking.Venue))]
        public IQueryable<Booking> Bookings { get; }

        // the list of members of the venue. The members get special discounts and offers from the venue. This is a many to many relationship with VenueUser.
        [MapTo("members")]
        [Backlink(nameof(VenueUser.MemberOf))]
        public IQueryable<VenueUser> Members { get; }

        // reference
        [MapTo("amenities")]
        public IList<Amenity> Amenities { get; }

        //local

        byte[]? selectedGamesImage;

        [Ignored]
        // use in mobile app to show the image of the selected game
        public byte[]? SelectedGamesImage
        {
            get { return selectedGamesImage; }
            set
            {
                if (value != selectedGamesImage)
                    selectedGamesImage = value;
                RaisePropertyChanged();
            }
        }

        ObservableCollection<byte[]>? selectedGamesImages;

        [Ignored]
        // use in mobile app to show all images of the selected game
        public ObservableCollection<byte[]>? SelectedGamesImages
        {
            get { return selectedGamesImages; }
            set
            {
                if (value != selectedGamesImages)
                    selectedGamesImages = value;
                RaisePropertyChanged();
            }
        }

        // use in mobile app to show the name of the selected game
        [Ignored]
        public string SelectedGamesName { get; set; }

        //use in the mobile app to show the full address of the venue
        string fullAddress;

        [Ignored]
        public string FullAddress
        {
            get
            {
                if (Address != null)
                {
                    fullAddress =
                        $"{Address.Street}, {Address.Province}, {Address.City}, {Address.PostCode}, {Address.Country}";
                }
                return fullAddress;
            }
        }
    }
}
