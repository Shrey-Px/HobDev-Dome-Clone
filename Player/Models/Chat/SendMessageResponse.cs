using System;

namespace Player.Models.Chat;

public class SendMessageResponse
{
    [JsonPropertyName("error")]
    public string? Error { get; set; }

    [JsonPropertyName("timestamp")]
    public string? TimeStamp { get; set; }

    [JsonPropertyName("sender")]
    public string? Sender { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }
}
