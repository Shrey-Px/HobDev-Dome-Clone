using System;

namespace Player.Models.Chat;

public class LoadMessagesResponse
{
    [JsonPropertyName("error")]
    public string? Error { get; set; }

    [JsonPropertyName("author")]
    public string? Author { get; set; }

    [JsonPropertyName("body")]
    public string? Body { get; set; }

    [JsonPropertyName("timestamp")]
    public string? TimeStamp { get; set; }

    [Ignored]
    public bool IsMine { get; set; }

    [Ignored]
    public string? DateSend { get; set; }

    [Ignored]
    public string? TimeSend { get; set; }

    [Ignored]
    public string? AuthorName { get; set; }
}
