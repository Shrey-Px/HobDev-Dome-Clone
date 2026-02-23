namespace Player.Models.APIServer
{
    public class PaymentIntentCreateResponse
    {
        [JsonPropertyName("error")]
        public string? Error { get; set; }

        [JsonPropertyName("clientSecret")]
        public string? ClientSecret { get; set; }
    }
}
