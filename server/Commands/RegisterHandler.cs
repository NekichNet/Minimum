using Minimum.Repositories.Interfaces;
using server.Models;
using System.Collections.Concurrent;
using System.Net.Sockets;

namespace server.Commands;

public class RegisterHandler : CommandHandler
{
    public RegisterHandler(
        IUserRepository userRepository,
        IChatRepository chatRepository,
        IMessageRepository messageRepository,
        ConcurrentDictionary<string, string> tokens) : base(userRepository, chatRepository, messageRepository, tokens) { }

    public override async Task<Response> HandleAsync(Request request, NetworkStream stream, TcpClient client)
    {
        var existingUser = await UserRepository.GetUserByNameAsync(request.Username);
        if (existingUser != null)
        {
            return new Response { Success = false, Message = "Пользователь уже существует." };
        }

        var newUser = new User { Name = request.Username, Password = request.Password };
        await UserRepository.AddUserAsync(newUser);

        return new Response { Success = true, Message = "Регистрация успешна." };
    }
}