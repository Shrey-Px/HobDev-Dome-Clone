using System;

namespace Player.Models.Chat;

public class GroupMessage
{
    public string GroupId { get; set; }
    public string MessageId { get; set; }

    public string Sender { get; set; }

    public string MessageContent { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
}
