namespace Player.Models.APIServer
{
    public class ValidateAvailabilityResponse
    {
        [JsonPropertyName("error")]
        public string? Error { get; set; }

        [JsonPropertyName("available")]
        public List<string>? Available { get; set; }

        [JsonPropertyName("booked")]
        public List<string>? Booked { get; set; }

        [JsonPropertyName("noData")]
        public List<string>? NoData { get; set; }
    }
}
