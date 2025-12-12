using server.Models;
using System.Collections.Concurrent;

namespace server.Commands;

public class RegisterHandler : CommandHandler
{
    public RegisterHandler(ConcurrentDictionary<string, string> users,
        ConcurrentDictionary<string, string> tokens,
        ConcurrentDictionary<string, Chat> chats) : base(users, tokens, chats) { }

    public override Response Handle(Request request)
    {
        if (Users.ContainsKey(request.Username))
        {
            return new Response { Success = false, Message = "Пользователь уже существует." };
        }

        Users.TryAdd(request.Username, request.Password);
        return new Response { Success = true, Message = "Регистрация успешна." };
    }
}