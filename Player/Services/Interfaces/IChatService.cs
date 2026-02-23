using System;
using Player.Models.Chat;

namespace Player.Services.Interfaces;

public interface IChatService
{
    Task<ParticipantCreateResponse?> CreateParticipant(string username);

    Task<ConversationCreateResponse?> CreateOneToOneConversation(
        string username,
        string other_user
    );

    Task<ConversationCreateResponse?> CreateGroupConversation(string username, string other_users);

    Task<AddRemoveUserResponse?> AddConversationUsers(
        string convo_sid,
        string username,
        string other_users
    );

    Task<AddRemoveUserResponse?> RemoveConversationUser(
        string convo_sid,
        string username,
        string other_user
    );

    Task<SendMessageResponse?> SendMessage(string sender, string convo_sid, string message);

    Task<List<LoadMessagesResponse>?> LoadMessages(string convo_sid);

    Task<DeleteConversationResponse?> DeleteConversation(string convo_sid, string username);
}
