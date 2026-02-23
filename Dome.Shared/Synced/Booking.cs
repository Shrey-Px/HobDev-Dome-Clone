using Dome.Shared.Synced.Player;

namespace Dome.Shared.Synced
{
    /// <summary>
    /// all the bookings made by the players will be stored in this collection. The booking can be made by the player or by the vendor. It have the details of the player, field, venue, booking date, start time, end time, price, number of players, field type, game name, booking status, cancellation reason, invoice id, is hosted, number of players required for hosting, is booked by vendor, unregistered player name, unregistered player phone, unregistered player email, field number, duration, total price
    /// </summary>
    public partial class Booking : IRealmObject
    {
        #region constructors
        private Booking() { }

        public Booking(
            ObjectId _id,
            string ownerId,
            Venue venue,
            string gameName,
            DateTimeOffset bookingDate,
            DateTimeOffset startTime,
            DateTimeOffset endTime,
            decimal nonPeakPricePerHour,
            decimal peakPricePerHour,
            int peoplePerGame,
            string fieldName,
            bool isBookedByVendor,
            decimal discountPercentage,
            string bookingStatus,
            decimal nonPeakHoursDuration,
            decimal peakHoursDuration,
            decimal totalDuration,
            decimal price,
            decimal convenienceFeePercentage,
            decimal taxPercentage,
            string currency,
            string cartId,
            bool isHosted


        // forced by backend
        )
        {
            Venue = venue;
            Id = _id;
            OwnerId = ownerId;
            GameName = gameName;
            BookingDate = bookingDate;
            StartTime = startTime;
            EndTime = endTime;
            NonPeakPricePerHour = nonPeakPricePerHour;
            PeakPricePerHour = peakPricePerHour;
            PeoplePerGame = peoplePerGame;
            FieldName = fieldName;
            IsBookedByVendor = isBookedByVendor;
            DiscountPercentage = discountPercentage;
            BookingStatus = bookingStatus;
            NonPeakHoursDuration = nonPeakHoursDuration;
            PeakHoursDuration = peakHoursDuration;
            TotalDuration = totalDuration;
            Price = price;
            ConvenienceFeePercentage = convenienceFeePercentage;
            TaxPercentage = taxPercentage;
            Currency = currency;
            CartId = cartId;
            IsHosted = isHosted;


        }

        #endregion


        [PrimaryKey]
        [MapTo("_id")]
        public ObjectId Id { get; set; }

        // the venue id of the venue where the slot is booked
        [MapTo("owner_id")]
        public string OwnerId { get; set; }

        // the playerId of the player who booked the slot. if the booking is made by the vendor, the playerId will be null. this is required for write permission in the realm cloud
        [Indexed]
        [MapTo("playerId")]
        public string? PlayerId { get; set; }

        //if the player is not registered in the mobile app, the unregistered player details will be stored while booking from vendor app,
        [MapTo("unregisteredPlayerName")]
        public string? UnregisteredPlayerName { get; set; }

        [MapTo("unregisteredPlayerPhone")]
        public string? UnregisteredPlayerMobileNumber { get; set; }

        [MapTo("unregisteredPlayerEmail")]
        public string? UnregisteredPlayerEmailId { get; set; }

        [MapTo("fieldName")]
        public string FieldName { get; set; }

        [MapTo("gameName")]
        public string GameName { get; set; }

        //the date on which the slot is booked. the time value should be zero
        [Indexed]
        [MapTo("bookingDate")]
        public DateTimeOffset BookingDate { get; set; }

        //the date and time at which the activity will start
        [Indexed]
        [MapTo("startTime")]
        public DateTimeOffset StartTime { get; set; }

        //the date and time at which the activity will finish
        [Indexed]
        [MapTo("endTime")]
        public DateTimeOffset EndTime { get; set; }

        [MapTo("nonPeakHoursDuration")]
        public decimal NonPeakHoursDuration { get; set; }

        [MapTo("peakHoursDuration")]
        public decimal PeakHoursDuration { get; set; }

        // the total duration of the booking calculated by adding nonPeakHoursDuration and peakHoursDuration
        [MapTo("totalDuration")]
        public decimal TotalDuration { get; set; }

        [MapTo("peoplePerGame")]
        public int PeoplePerGame { get; set; }

        [Indexed]
        // five types of booking status:reserved, booked, cancelled, started and completed
        [MapTo("bookingStatus")]
        public string BookingStatus { get; set; }

        [MapTo("isBookedByVendor")]
        public bool IsBookedByVendor { get; set; }

        #region payment

        [MapTo("nonPeakPricePerHour")]
        public decimal NonPeakPricePerHour { get; set; }

        [MapTo("peakPricePerHour")]
        public decimal PeakPricePerHour { get; set; }

        // calculated by adding (nonPeakPricePerHour * nonPeakHoursDuration) and (peakPricePerHour * peakHoursDuration)
        [MapTo("price")]
        public decimal Price { get; set; }

        // the discount percentage applied on price
        [MapTo("discountPercentage")]
        public decimal? DiscountPercentage { get; set; }

        // the discount amount on price
        [MapTo("discount")]
        public decimal? Discount { get; set; }

        // calculated by deducting discount from Price. this amount is refunded on cancellation without convenience fee.
        [MapTo("priceAfterDeductingDiscount")]
        public decimal? PriceAfterDeductingDiscount { get; set; }

        // the convenience fee percentage applied on totalPriceAfterDiscount
        [MapTo("convenienceFeePercentage")]
        public decimal ConvenienceFeePercentage { get; set; }

        // the convenience fee is based on PriceAfterDeductingDiscount
        [MapTo("convenienceFee")]
        public decimal? ConvenienceFee { get; set; }

        // the amount after adding convenience fee to the PriceAfterDeductingDiscount. this amount is refunded on cancellation with convenience fee.
        [MapTo("priceAfterAddingConvenienceFee")]
        public decimal? PriceAfterAddingConvenienceFee { get; set; }

        // the tax percentage applied on the PriceAfterAddingConvenienceFee
        [MapTo("taxPercentage")]
        public decimal TaxPercentage { get; set; }

        // the tax amount on the PriceAfterAddingConvenienceFee
        [MapTo("tax")]
        public decimal? Tax { get; set; }

        // the total price after adding tax to the PriceAfterAddingConvenienceFee
        [MapTo("totalPrice")]
        public decimal? TotalPrice { get; set; }

        [MapTo("appliedCoupon")]
        public Coupon? AppliedCoupon { get; set; }

        // this coupon is applied by default when the user books for the first time
        [MapTo("isFirstBookingCouponApplied")]
        public bool IsFirstBookingCouponApplied { get; set; }

        // the currency of the payment
        [MapTo("currency")]
        public string Currency { get; set; }

        // the cartId recognise the the batch of bookings made together. this will be used to connect the booking with the payment. In stripe the value of the cartId will be assigned to the transfer_group
        [MapTo("cartId")]
        public string CartId { get; set; }

        // the paymentIntentId is used to connect the booking with the payment. this is required for the refund process
        [MapTo("paymentIntentId")]
        public string? PaymentIntentId { get; set; }

        // The time when the paymentIntent is created and after that when the status of the paymentIntent is updated
        [MapTo("paymentIntentUpdated")]
        public DateTimeOffset? PaymentIntentUpdated { get; set; }

        // the status of the payment Intent. It could be succeeded, requires_payment_method, requires_confirmation, requires_action, processing, canceled, or failed
        [MapTo("paymentIntentStatus")]
        public string? PaymentIntentStatus { get; set; }

        [MapTo("customerName")]
        public string? CustomerName { get; set; }

        [Indexed]
        [MapTo("customerEmail")]
        public string? CustomerEmail { get; set; }

        [MapTo("customerPhone")]
        public string? CustomerPhone { get; set; }
        #endregion


        #region backlinks

        [MapTo("venue")]
        public Venue? Venue { get; set; }

        [MapTo("player")]
        public VenueUser? Player { get; set; }

        #endregion


        #region sharing

        [MapTo("isHosted")]
        [Indexed]
        public bool IsHosted { get; set; }

        /// <summary>
        /// this value is taken from the Planned booking that is connected to the shared booking.
        /// </summary>
        [MapTo("numberOfPlayersRequiredForHosting")]
        public int? NumberOfPlayersRequiredForHosting { get; set; }

        /// <summary>
        /// this value is taken from the Planned booking that is connected to the shared booking.
        /// </summary>
        [MapTo("playerCompetancyLevel")]
        public string? PlayerCompetancyLevel { get; set; } = PlayerCompetancy.Beginner;

        /// <summary>
        /// the list of players who joined the shared booking. This list will be updated when the player joins the shared booking from the Chat group by clicking on Join button.
        /// </summary>
        [MapTo("joinedPlayers")]
        public IList<VenueUser> JoinedPlayers { get; }

        [MapTo("bringYourOwnEquipment")]
        public bool? BringYourOwnEquipment { get; set; } = true;

        [MapTo("costShared")]
        public bool? CostShared { get; set; } = true;

        /// <summary>
        ///  conversation id is the id of the chat group associated with the planned booking and the Shared Purchased booking
        /// </summary>
        [MapTo("conversationId")]
        public string? ConversationId { get; set; }

        /// <summary>
        /// The user can connect the Shared Purchased booking with the planned booking.
        /// </summary>
        [MapTo("connectedPlannedBooking")]
        public PlannedBooking? ConnectedPlannedBooking { get; set; }

        #endregion



        #region local and ignored properties
        private byte[] gameImage;

        // use to display the game image in the venues list page in the mobile app
        public byte[] GameImage
        {
            get => gameImage;
            set
            {
                if (value != gameImage)
                {
                    gameImage = value;
                    RaisePropertyChanged();
                }
            }
        }

        // use to dislay the status of the shared booking in the mobile app. 1 is added to the count of joined players for host.
        [Ignored]
        // public string TeamStatus => $"{JoinedPlayers.Count+1}/{NumberOfPlayersRequiredForHosting} Going";

        // until we set the NumberOfPlayersRequiredForHosting to 1 while booking, we are only showing the joinedPlayers count in the team status because the intial value of NumberOfPlayersRequiredForHosting is 0  which result in team status of 1/0
        public string TeamStatus => $"{JoinedPlayers.Count + 1} Going";

        bool isJoined;

        [Ignored]
        // use to show whether the hosted game is joined by the logged in user.
        public bool IsJoined
        {
            get => isJoined;
            set
            {
                if (value != isJoined)
                {
                    isJoined = value;
                    RaisePropertyChanged();
                }
            }
        }

        [Ignored]
        public bool IsHostedbyLoggedInUser => !IsJoined && IsHosted;

        [Ignored]
        public bool NotHostedbyLoggedInUser => !IsJoined && !IsHosted;

        // while booking a slot in vendor app if the player is not registered in the mobile app, the unregistered player name will be assigned to this property. If the player is registered in the mobile app, the player name will be assigned to this property
        [Ignored]
        public string? PlayerName => Player == null ? UnregisteredPlayerName : Player.FirstName;

        // this used in AvailableGamesView to display the venue name
        [Ignored]
        public string? VenueName => Venue?.FullName;

        [Ignored]
        public string? BookingInfo =>
            $"{StartTime:dd MMMM}, {StartTime:HH:mm} | {TotalDuration} hours | {FieldName}";

        #endregion
    }
}

