

namespace Player.Models.APIServer
{
    public class CreateBookingsRequest
    {
        public CreateBookingsRequest(List<NewBooking> bookingData, string environment)
        {
            BookingData = bookingData;
            Environment = environment;
        }

        [JsonPropertyName("bookingData")]
        public List<NewBooking> BookingData { get; set; }

        [JsonPropertyName("environment")]
        public string Environment { get; set; }
    }
}
