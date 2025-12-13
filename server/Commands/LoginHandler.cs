using server.Models;
using System.Collections.Concurrent;
using System.Net.Sockets;

namespace server.Commands;

public class LoginHandler : CommandHandler
{
    public LoginHandler(
        ConcurrentDictionary<int, User> usersById,
        ConcurrentDictionary<string, User> usersByName,
        ConcurrentDictionary<string, string> tokens,
        ConcurrentDictionary<int, Chat> chatsById) : base(usersById, usersByName, tokens, chatsById) { }

    public override Response Handle(Request request, NetworkStream stream, TcpClient client)
    {
        if (UsersByName.TryGetValue(request.Username, out User user) && user.Password == request.Password)
        {
            string token = Guid.NewGuid().ToString();
            UserTokens.TryAdd(token, user.Name);
            return new Response { Success = true, Message = "Успешный вход.", Token = token };
        }

        return new Response { Success = false, Message = "Неверный логин или пароль." };
    }
}