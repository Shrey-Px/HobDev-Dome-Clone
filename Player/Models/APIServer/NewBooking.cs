

namespace Player.Models.APIServer
{
    public class NewBooking
    {
        public NewBooking(
            string _id,
            string venue,
            string ownerId,
            string bookingStatus,
            string fieldName,
            string gameName,
            decimal price,
            decimal pricePerHour,
            decimal totalPrice,
            decimal convenienceFeePercentage,
            decimal taxPercentage,
            decimal convenienceFee,
            decimal tax,
            decimal priceAfterAddingConvenienceFee,
            decimal discountPercentage,
            decimal discount,
            decimal priceAfterDeductingDiscount,
            decimal duration,
            DateTimeOffset startTime,
            DateTimeOffset endTime,
            DateTimeOffset bookingDate,
            string currency,
            string paymentIntentStatus,
            string cartId,
            int peoplePerGame,
            bool isHosted,
            int numberOfPlayersRequiredForHosting,
            bool isBookedByVendor,
            bool costShared,
            bool bringYourOwnEquipment,
            string playerCompetancyLevel,
            string player,
            string customerName,
            string customerEmail,
            string customerPhone,
            string source,
            string notes,
            bool isFirstBookingCouponApplied,
            decimal nonPeakHoursDuration,
            decimal peakHoursDuration,
            decimal totalDuration,
            decimal nonPeakPricePerHour,
            decimal peakPricePerHour
        )
        {
            Id = _id;
            Venue = venue;
            OwnerId = ownerId;
            BookingStatus = bookingStatus;
            FieldName = fieldName;
            GameName = gameName;
            Price = price;
            PricePerHour = pricePerHour;
            TotalPrice = totalPrice;
            ConvenienceFeePercentage = convenienceFeePercentage;
            TaxPercentage = taxPercentage;
            ConvenienceFee = convenienceFee;
            Tax = tax;
            PriceAfterAddingConvenienceFee = priceAfterAddingConvenienceFee;
            DiscountPercentage = discountPercentage;
            Discount = discount;
            PriceAfterDeductingDiscount = priceAfterDeductingDiscount;
            Duration = duration;
            StartTime = startTime;
            EndTime = endTime;
            BookingDate = bookingDate;
            Currency = currency;
            PaymentIntentStatus = paymentIntentStatus;
            CartId = cartId;
            PeoplePerGame = peoplePerGame;
            IsHosted = isHosted;
            NumberOfPlayersRequiredForHosting = numberOfPlayersRequiredForHosting;
            IsBookedByVendor = isBookedByVendor;
            CostShared = costShared;
            BringYourOwnEquipment = bringYourOwnEquipment;
            PlayerCompetancyLevel = playerCompetancyLevel;
            Player = player;
            CustomerName = customerName;
            CustomerEmail = customerEmail;
            CustomerPhone = customerPhone;
            Source = source;
            Notes = notes;
            IsFirstBookingCouponApplied = isFirstBookingCouponApplied;
            NonPeakHoursDuration = nonPeakHoursDuration;
            PeakHoursDuration = peakHoursDuration;
            TotalDuration = totalDuration;
            NonPeakPricePerHour = nonPeakPricePerHour;
            PeakPricePerHour = peakPricePerHour;
        }

        [JsonPropertyName("_id")]
        public string Id { get; set; }

        [JsonPropertyName("venue")]
        public string Venue { get; set; }

        // the venue id of the venue where the slot is booked
        [JsonPropertyName("owner_id")]
        public string OwnerId { get; set; }

        [JsonPropertyName("bookingStatus")]
        public string BookingStatus { get; set; }

        [JsonPropertyName("fieldName")]
        public string FieldName { get; set; }

        [JsonPropertyName("gameName")]
        public string GameName { get; set; }

        [JsonPropertyName("price")]
        public decimal Price { get; set; } = 0.0M;

        [JsonPropertyName("pricePerHour")]
        public decimal PricePerHour { get; set; } = 0.0M;

        [JsonPropertyName("totalPrice")]
        public decimal TotalPrice { get; set; }

        [JsonPropertyName("convenienceFeePercentage")]
        public decimal ConvenienceFeePercentage { get; set; }

        [JsonPropertyName("taxPercentage")]
        public decimal TaxPercentage { get; set; }

        [JsonPropertyName("convenienceFee")]
        public decimal ConvenienceFee { get; set; }

        [JsonPropertyName("tax")]
        public decimal Tax { get; set; }

        [JsonPropertyName("priceAfterAddingConvenienceFee")]
        public decimal? PriceAfterAddingConvenienceFee { get; set; }

        [JsonPropertyName("discountPercentage")]
        public decimal DiscountPercentage { get; set; }

        [JsonPropertyName("discount")]
        public decimal Discount { get; set; }

        [JsonPropertyName("priceAfterDeductingDiscount")]
        public decimal PriceAfterDeductingDiscount { get; set; }

        [JsonPropertyName("duration")]
        public decimal Duration { get; set; }

        //the date and time at which the activity will start
        [JsonPropertyName("startTime")]
        public DateTimeOffset StartTime { get; set; }

        //the date and time at which the activity will finish
        [JsonPropertyName("endTime")]
        public DateTimeOffset EndTime { get; set; }

        //the date on which the slot is booked. the time value should be zero
        [JsonPropertyName("bookingDate")]
        public DateTimeOffset BookingDate { get; set; }

        // the currency of the payment
        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("paymentIntentStatus")]
        public string PaymentIntentStatus { get; set; }

        // the cartId recognise the the batch of bookings made together. this will be used to connect the booking with the payment. In stripe the value of the cartId will be assigned to the transfer_group
        [JsonPropertyName("cartId")]
        public string CartId { get; set; }

        [JsonPropertyName("peoplePerGame")]
        public int PeoplePerGame { get; set; }

        [JsonPropertyName("isHosted")]
        public bool IsHosted { get; set; }

        /// <summary>
        /// this value is taken from the Planned booking that is connected to the shared booking.
        /// </summary>
        [JsonPropertyName("numberOfPlayersRequiredForHosting")]
        public int NumberOfPlayersRequiredForHosting { get; set; }

        [JsonPropertyName("isBookedByVendor")]
        public bool IsBookedByVendor { get; set; }

        [JsonPropertyName("costShared")]
        public bool CostShared { get; set; } = true;

        [JsonPropertyName("bringYourOwnEquipment")]
        public bool BringYourOwnEquipment { get; set; } = true;

        [JsonPropertyName("playerCompetancyLevel")]
        public string PlayerCompetancyLevel { get; set; } = PlayerCompetancy.Beginner;

        [JsonPropertyName("player")]
        public string Player { get; set; }

        [JsonPropertyName("customerName")]
        public string CustomerName { get; set; }

        [JsonPropertyName("customerEmail")]
        public string CustomerEmail { get; set; }

        [JsonPropertyName("customerPhone")]
        public string CustomerPhone { get; set; }

        [JsonPropertyName("source")]
        public string Source { get; set; }

        [JsonPropertyName("notes")]
        public string Notes { get; set; }

        [JsonPropertyName("isFirstBookingCouponApplied")]
        public bool IsFirstBookingCouponApplied { get; set; }

        [JsonPropertyName("nonPeakHoursDuration")]
        public decimal NonPeakHoursDuration { get; set; }

        //time duration of the activity in peak hours
        [JsonPropertyName("peakHoursDuration")]
        public decimal PeakHoursDuration { get; set; }

        [JsonPropertyName("totalDuration")]
        public decimal TotalDuration { get; set; }

        // the price per hour during non-peak hours
        [JsonPropertyName("nonPeakPricePerHour")]
        public decimal NonPeakPricePerHour { get; set; }

        [JsonPropertyName("peakPricePerHour")]
        public decimal PeakPricePerHour { get; set; }

    }
}
