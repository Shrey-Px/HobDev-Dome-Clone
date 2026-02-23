using System;

namespace Player.Models.Chat;

public class ConversationCreateResponse
{
    [JsonPropertyName("error")]
    public string? Error { get; set; }

    [JsonPropertyName("conversationId")]
    public string? ConversationId { get; set; }

    [JsonPropertyName("message")]
    public string? Message { get; set; }
}
