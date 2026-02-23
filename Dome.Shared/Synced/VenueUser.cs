using Dome.Shared.Synced.Player;

namespace Dome.Shared.Synced
{
    /// <summary>
    /// the player class represents the registered user of the dome mobile app.
    /// </summary>
    public partial class VenueUser : IRealmObject
    {
        private VenueUser() { }

        public VenueUser(
            ObjectId id,
            string ownerId,
            string email,
            string firstName,
            string lastName,
            string mobileNumber,
            string phoneCode,
            string country,
            VenueUserAddress address,
            string stripeCustomerId
        )
        {
            Id = id;
            OwnerId = ownerId;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            MobileNumber = mobileNumber;
            PhoneCode = phoneCode;
            Country = country;
            Address = address;
            StripeCustomerId = stripeCustomerId;
        }

        [PrimaryKey]
        [MapTo("_id")]
        public ObjectId Id { get; set; }

        [MapTo("owner_id")]
        [Indexed]
        public string OwnerId { get; set; }

        [MapTo("isActive")]
        public bool IsActive { get; set; } = true;

        // this is the id of the stripe customer object that is created when the user registers in the app
        [MapTo("stripeCustomerId")]
        public string StripeCustomerId { get; set; }

        [MapTo("firstName")]
        public string FirstName { get; set; }

        [MapTo("lastName")]
        public string LastName { get; set; }

        [MapTo("eMail")]
        public string Email { get; set; }

        [MapTo("mobileNumber")]
        public string MobileNumber { get; set; }

        [MapTo("phoneCode")]
        public string PhoneCode { get; set; }

        [MapTo("country")]
        public string Country { get; set; }

        [MapTo("isMobileNumberVerified")]
        public bool IsMobileNumberVerified { get; set; }

        // this is an optional attribute
        [MapTo("profileImage")]
        public byte[]? ProfileImage { get; set; }

        [MapTo("ageGroup")]
        public string? AgeGroup { get; set; }

        [MapTo("favouriteGames")]
        public IList<string> FavouriteGames { get; }

        // embedded objects
        [MapTo("address")]
        public VenueUserAddress? Address { get; set; }

        //backlinks
        /// <summary>
        /// The list of Bookings that player has purchased.
        /// </summary>
        [Backlink(nameof(Booking.Player))]
        [MapTo("bookings")]
        public IQueryable<Booking> Bookings { get; }

        /// <summary>
        /// The list of planned bookings that the player has created from the Host Game Page
        /// </summary>
        [Backlink(nameof(PlannedBooking.Host))]
        [MapTo("plannedBookings")]
        public IQueryable<PlannedBooking> PlannedBookings { get; }

        /// <summary>
        /// The list of join requests that the player has applied for in the bookings planned by other players.
        /// </summary>
        [Backlink(nameof(JoinRequest.AppliedBy))]
        [MapTo("appliedBookings")]
        public IQueryable<JoinRequest> AppliedBookings { get; }

        /// <summary>
        /// The list of join requests that the player has got approval for in the bookings planned by other players.
        /// </summary>
        [Backlink(nameof(Booking.JoinedPlayers))]
        [MapTo("joinedBookings")]
        public IQueryable<Booking> JoinedBookings { get; }

        // List of venues that the user is a member of. Members get special discounts and offers from the venue. This is a many to many relationship with Venue.Members
        public IList<Venue> MemberOf { get; }

        // local

        [Ignored]
        [MapTo("fullName")]
        public string FullName => $"{FirstName} {LastName}";
    }
}
