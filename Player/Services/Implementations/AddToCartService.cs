namespace Player.Services.Implementations
{
    public class AddToCartService : IAddToCartService
    {
        private readonly IRealmService realmService;

        readonly Realm syncedRealm;

        readonly VenueUser? venueUser;

        public AddToCartService(IRealmService realmService)
        {
            this.realmService = realmService;

            syncedRealm = Realm.GetInstance(realmService.Config);

            venueUser = syncedRealm
                .All<VenueUser>()
                .FirstOrDefault(n => n.OwnerId == realmService.RealmUser.Id);
        }

        /// <summary>
        /// this method is used in BookingTimingViewModel to check if any field is available in the selected time slot. Only fields that are not on Hold, booked or reserved are displayed to the user for booking.
        /// </summary>
        /// <param name="selectedVenueId"></param>
        /// <param name="selectedFieldName"></param>
        /// <param name="selectedDate"></param>
        /// <param name="selectedStartTime"></param>
        /// <param name="selectedEndTime"></param>
        /// <returns></returns>
        public async Task<bool> IsSlotAvailableAsync(
            ObjectId selectedVenueId,
            string selectedFieldName,
            DateTime selectedDate,
            DateTime selectedStartTime,
            DateTime selectedEndTime
        )
        {
            Venue? venue = syncedRealm.All<Venue>().FirstOrDefault(g => g.Id == selectedVenueId);

            bool isSelectedTimeAvailable = true;

            // find existing bookings in Selected Venue where SelectedDate & Field Name are same and BookingStatus is one of the following: OnHold, Reserved, Booked.
            IEnumerable<Booking> bookingsInSelectedField = venue.Bookings.Where(s =>
                s.FieldName == selectedFieldName
            );
            IEnumerable<Booking> existingBookings = bookingsInSelectedField.Where(f =>
                (f.StartTime.Date == selectedDate)
                && (
                    (f.BookingStatus == Dome.Shared.Constants.AppConstants.Booked)
                    || (f.BookingStatus == Dome.Shared.Constants.AppConstants.Reserved)
                    || (f.BookingStatus == Dome.Shared.Constants.AppConstants.Created)
                )
            );

            foreach (Booking existingBooking in existingBookings)
            {
                for (
                    DateTime time = selectedStartTime;
                    time < selectedEndTime;
                    time = time.AddHours(1)
                )
                {
                    // if the  StartTime and EndTime of requestedBooking and existingBooking overlaps then the booking is not available.
                    if (
                        existingBooking.StartTime.DateTime <= time
                        && existingBooking.EndTime.DateTime > time
                    )
                    {
                        return false;
                    }
                }
            }

            return isSelectedTimeAvailable;
        }
    }
}
