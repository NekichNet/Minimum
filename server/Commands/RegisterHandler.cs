using server.Models;
using System.Collections.Concurrent;
using System.Net.Sockets;

namespace server.Commands;

public class RegisterHandler : CommandHandler
{
    public RegisterHandler(
        ConcurrentDictionary<int, User> usersById,
        ConcurrentDictionary<string, User> usersByName,
        ConcurrentDictionary<string, string> tokens,
        ConcurrentDictionary<int, Chat> chatsById) : base(usersById, usersByName, tokens, chatsById) { }

    public override Response Handle(Request request, NetworkStream stream, TcpClient client)
    {
        if (UsersByName.ContainsKey(request.Username))
        {
            return new Response { Success = false, Message = "Пользователь уже существует." };
        }

        var newUser = new User
        {
            Id = UsersById.Count + 1,
            Name = request.Username,
            Password = request.Password
        };

        UsersById.TryAdd(newUser.Id, newUser);
        UsersByName.TryAdd(newUser.Name, newUser);

        return new Response { Success = true, Message = "Регистрация успешна." };
    }
}