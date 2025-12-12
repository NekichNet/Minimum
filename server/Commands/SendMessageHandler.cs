using server.Models;
using System.Collections.Concurrent;

namespace server.Commands;

public class SendMessageHandler : CommandHandler
{
    public SendMessageHandler(
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

        if (request.ChatId == null)
        {
            return new Response { Success = false, Message = "ID чата не указан." };
        }

        if (!ChatsById.TryGetValue(request.ChatId.Value, out Chat chat))
        {
            return new Response { Success = false, Message = "Чат не найден." };
        }

        var message = new Message(user.Id, request.MessageText, user);
        chat.Messages.Add(message);

        return new Response { Success = true, Message = "Сообщение отправлено." };
    }
}