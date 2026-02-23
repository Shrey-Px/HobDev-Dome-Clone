using System;
using Player.Models.Chat;

namespace Player.Services.Implementations;

public class ChatService : IChatService
{
    HttpClient httpClient = new HttpClient()
    {
        BaseAddress = new Uri("http://domechat.site/dome/"),
    };

    public async Task<ParticipantCreateResponse?> CreateParticipant(string username)
    {
        // send a request to the server to create a user using the username in formdata format
        var formData = new MultipartFormDataContent();
        formData.Add(new StringContent(username), "username");

        var response = await httpClient.PostAsync("createUser", formData);

        var dataInJSON = await response.Content.ReadAsStringAsync();

        var data = JsonSerializer.Deserialize<ParticipantCreateResponse>(dataInJSON);

        return data;
    }

    public async Task<ConversationCreateResponse?> CreateOneToOneConversation(
        string username,
        string other_user
    )
    {
        var formData = new MultipartFormDataContent();
        formData.Add(new StringContent(username), "username");
        formData.Add(new StringContent(other_user), "other_user");

        var response = await httpClient.PostAsync("createConversation", formData);

        var dataInJSON = await response.Content.ReadAsStringAsync();

        var data = JsonSerializer.Deserialize<ConversationCreateResponse>(dataInJSON);

        return data;
    }

    public async Task<ConversationCreateResponse?> CreateGroupConversation(
        string username,
        string other_users
    )
    {
        var formData = new MultipartFormDataContent();
        formData.Add(new StringContent(username), "username");
        formData.Add(new StringContent(other_users), "other_users");

        var response = await httpClient.PostAsync("createConversation", formData);

        var dataInJSON = await response.Content.ReadAsStringAsync();

        var data = JsonSerializer.Deserialize<ConversationCreateResponse>(dataInJSON);

        return data;
    }

    public async Task<AddRemoveUserResponse?> AddConversationUsers(
        string convo_sid,
        string username,
        string other_users
    )
    {
        var formData = new MultipartFormDataContent();
        formData.Add(new StringContent(convo_sid), "convo_sid");
        formData.Add(new StringContent(username), "username");
        formData.Add(new StringContent(other_users), "other_users");

        var response = await httpClient.PostAsync("addConversationUsers", formData);

        var dataInJSON = await response.Content.ReadAsStringAsync();

        var data = JsonSerializer.Deserialize<AddRemoveUserResponse>(dataInJSON);

        return data;
    }

    public async Task<AddRemoveUserResponse?> RemoveConversationUser(
        string convo_sid,
        string username,
        string other_user
    )
    {
        var formData = new MultipartFormDataContent();
        formData.Add(new StringContent(convo_sid), "convo_sid");
        formData.Add(new StringContent(username), "username");
        formData.Add(new StringContent(other_user), "other_user");

        var response = await httpClient.PostAsync("removeConversationUser", formData);

        var dataInJSON = await response.Content.ReadAsStringAsync();

        var data = JsonSerializer.Deserialize<AddRemoveUserResponse>(dataInJSON);

        return data;
    }

    public async Task<SendMessageResponse?> SendMessage(
        string sender,
        string convo_sid,
        string message
    )
    {
        var formData = new MultipartFormDataContent();
        formData.Add(new StringContent(sender), "sender");
        formData.Add(new StringContent(convo_sid), "convo_sid");
        formData.Add(new StringContent(message), "message");

        var response = await httpClient.PostAsync("sendMessage", formData);

        var dataInJSON = await response.Content.ReadAsStringAsync();

        var data = JsonSerializer.Deserialize<SendMessageResponse>(dataInJSON);

        return data;
    }

    public async Task<List<LoadMessagesResponse>?> LoadMessages(string convo_sid)
    {
        var formData = new MultipartFormDataContent();
        formData.Add(new StringContent(convo_sid), "convo_sid");

        var response = await httpClient.PostAsync("loadMessages", formData);

        var dataInJSON = await response.Content.ReadAsStringAsync();

        var data = JsonSerializer.Deserialize<List<LoadMessagesResponse>>(dataInJSON);

        return data;
    }

    public async Task<DeleteConversationResponse?> DeleteConversation(
        string convo_sid,
        string username
    )
    {
        var formData = new MultipartFormDataContent();
        formData.Add(new StringContent(convo_sid), "convo_sid");
        formData.Add(new StringContent(username), "username");

        var response = await httpClient.PostAsync("deleteConversation", formData);

        var dataInJSON = await response.Content.ReadAsStringAsync();

        var data = JsonSerializer.Deserialize<DeleteConversationResponse>(dataInJSON);

        return data;
    }
}
