namespace Player.Services.Interfaces
{
    public interface IAddToCartService
    {
        Task<bool> IsSlotAvailableAsync(
            ObjectId selectedVenueId,
            string selectedFieldName,
            DateTime selectedDate,
            DateTime selectedStartTime,
            DateTime selectedEndTime
        );
    }
}
