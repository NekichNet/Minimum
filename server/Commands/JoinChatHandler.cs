using server.Models;
using System.Collections.Concurrent;
using System.Net.Sockets;

namespace server.Commands;

public class JoinChatHandler : CommandHandler
{
    public JoinChatHandler(
        ConcurrentDictionary<int, User> usersById,
        ConcurrentDictionary<string, User> usersByName,
        ConcurrentDictionary<string, string> tokens,
        ConcurrentDictionary<int, Chat> chatsById) : base(usersById, usersByName, tokens, chatsById) { }

    public override Response Handle(Request request, NetworkStream stream, TcpClient client)
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

        if (!chat.Users.Contains(user))
        {
            return new Response { Success = false, Message = "Пользователь не состоит в этом чате." };
        }

        if (!chat.ConnectedClients.Contains(client))
        {
            chat.ConnectedClients.Add(client);
        }

        return new Response { Success = true, Message = "Вы присоединились к чату." };
    }
}