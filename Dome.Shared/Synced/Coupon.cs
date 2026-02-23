namespace Dome.Shared.Synced
{
    public partial class Coupon : IRealmObject
    {
        private Coupon() { }

        public Coupon(
            string couponCode,
            decimal discountPercentage,
            DateTimeOffset expiryDate,
            string couponStatus,
            decimal maximumDiscount
        )
        {
            CouponCode = couponCode;
            DiscountPercentage = discountPercentage;
            ExpiryDate = expiryDate;
            MaximumDiscount = maximumDiscount;
        }

        [MapTo("_id")]
        [PrimaryKey]
        public ObjectId Id { get; set; } = ObjectId.GenerateNewId();

        [MapTo("couponCode")]
        public string CouponCode { get; set; }

        [MapTo("discountPercentage")]
        public decimal DiscountPercentage { get; set; }

        [MapTo("maximumDiscount")]
        public decimal MaximumDiscount { get; set; }

        [MapTo("expiryDate")]
        public DateTimeOffset ExpiryDate { get; set; }

        // three type of status: "unused", "reserved" and "used"
        [MapTo("couponStatus")]
        public string CouponStatus { get; set; } = CouponPosition.unused.ToString();

        // relationship: the user who used the coupon
        [MapTo("benefitedUser")]
        public VenueUser? BenefitedUser { get; set; }

        // Backlink: the list of Bookings where the coupon is applied
        [MapTo("discountedBooking")]
        [Backlink(nameof(Booking.AppliedCoupon))]
        public IQueryable<Booking> DiscountedBooking { get; }
    }
}
