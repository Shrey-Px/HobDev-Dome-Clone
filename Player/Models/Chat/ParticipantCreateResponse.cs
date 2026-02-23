using System;

namespace Player.Models.Chat;

public class ParticipantCreateResponse
{
    [JsonPropertyName("error")]
    public string? Error { get; set; }

    [JsonPropertyName("username")]
    public string? UserName { get; set; }

    [JsonPropertyName("message")]
    public string? Message { get; set; }
}
