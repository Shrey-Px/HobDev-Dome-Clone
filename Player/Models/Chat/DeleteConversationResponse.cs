using System;

namespace Player.Models.Chat;

public class DeleteConversationResponse
{
    [JsonPropertyName("error")]
    public string? Error { get; set; }

    [JsonPropertyName("message")]
    public string? Message { get; set; }
}
