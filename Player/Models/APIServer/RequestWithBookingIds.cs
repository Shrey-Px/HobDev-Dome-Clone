namespace Player.Models.APIServer
{
    public record RequestWithBookingIds
    {
        public RequestWithBookingIds(List<string> bookingIds, string environment)
        {
            BookingIds = bookingIds;
            Environment = environment;
        }

        [JsonPropertyName("bookingIds")]
        public List<string> BookingIds { get; set; }

        [JsonPropertyName("environment")]
        public string Environment { get; set; }
    }
}
