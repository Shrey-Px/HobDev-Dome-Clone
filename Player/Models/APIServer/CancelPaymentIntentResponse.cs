namespace Player.Models.APIServer
{
    public class CancelPaymentIntentResponse
    {
        [JsonPropertyName("error")]
        public string? Error { get; set; }

        [JsonPropertyName("bookingIds")]
        public List<string> BookingIds { get; set; }

        [JsonPropertyName("environment")]
        public string Environment { get; set; }
    }
}
