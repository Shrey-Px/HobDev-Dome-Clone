using System;

namespace Player.Models.Chat;

public class Conversation
{
    public string ConversationId { get; set; }

    public List<ChatParticipant> ChatParticipants { get; set; }

    public List<Message> Messages { get; set; }
}
