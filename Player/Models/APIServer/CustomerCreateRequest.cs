namespace Player.Models.APIServer
{
    public class CustomerCreateRequest
    {
        public CustomerCreateRequest(string customerName, string customerEmail, string environment)
        {
            CustomerName = customerName;
            CustomerEmail = customerEmail;
            Environment = environment;
        }

        [JsonPropertyName("customerName")]
        public string CustomerName { get; set; }

        [JsonPropertyName("customerEmail")]
        public string CustomerEmail { get; set; }

        [JsonPropertyName("environment")]
        public string Environment { get; set; }
    }
}
