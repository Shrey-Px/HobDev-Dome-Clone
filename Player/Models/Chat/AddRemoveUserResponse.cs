using System;

namespace Player.Models.Chat;

public class AddRemoveUserResponse
{
    [JsonPropertyName("error")]
    public string? Error { get; set; }

    [JsonPropertyName("convo_sid")]
    public string? ConversationId { get; set; }

    [JsonPropertyName("username")]
    public string? UserName { get; set; }
}
