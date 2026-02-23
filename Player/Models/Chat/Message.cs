using System;

namespace Player.Models.Chat;

public class Message
{
    public string MessageId { get; set; }

    public string Sender { get; set; }

    public string Receiver { get; set; }

    public string MessageContent { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
}
