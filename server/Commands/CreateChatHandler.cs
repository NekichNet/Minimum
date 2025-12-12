using server.Models;
using System.Collections.Concurrent;

namespace server.Commands;

public class CreateChatHandler : CommandHandler
{
    public CreateChatHandler(
        ConcurrentDictionary<int, User> usersById,
        ConcurrentDictionary<string, User> usersByName,
        ConcurrentDictionary<string, string> tokens,
        ConcurrentDictionary<int, Chat> chatsById) : base(usersById, usersByName, tokens, chatsById) { }

    public override Response Handle(Request request)
    {
        if (!ValidateToken(request.Token, out User user))
        {
            return new Response { Success = false, Message = "Невалидный токен." };
        }

        var newChat = new Chat { Id = 1, Name = request.ChatName };
        newChat.Users.Add(user);

        ChatsById.TryAdd(newChat.Id, newChat);

        return new Response { Success = true, Message = "Чат создан.", ChatId = newChat.Id };
    }
}