using System.Text.Json.Serialization;

namespace Player.Models.APIServer
{
    public class CustomerCreateResponse
    {
        [JsonPropertyName("error")]
        public string? Error { get; set; }

        [JsonPropertyName("clientId")]
        public string? ClientId { get; set; }
    }
}
