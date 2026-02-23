using Amazon.S3.Model;

namespace Player.Models.APIServer
{
    public class CancelBookingResponse
    {
        [JsonPropertyName("error")]
        public string? Error { get; set; }

        [JsonPropertyName("bookingIds")]
        public List<string> BookingIds { get; set; }

        [JsonPropertyName("environment")]
        public string Environment { get; set; }

        [JsonPropertyName("status")]
        public string? Status { get; set; }
    }
}
