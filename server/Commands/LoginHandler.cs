using server.Models;
using System.Collections.Concurrent;

namespace server.Commands;

public class LoginHandler : CommandHandler
{
    public LoginHandler(ConcurrentDictionary<string, string> users,
        ConcurrentDictionary<string, string> tokens,
        ConcurrentDictionary<string, Chat> chats) : base(users, tokens, chats) { }

    public override Response Handle(Request request)
    {
        if (Users.TryGetValue(request.Username, out string storedPassword) && storedPassword == request.Password)
        {
            string token = Guid.NewGuid().ToString();
            UserTokens.TryAdd(token, request.Username);
            return new Response { Success = true, Message = "Успешный вход.", Token = token };
        }

        return new Response { Success = false, Message = "Неверный логин или пароль." };
    }
}