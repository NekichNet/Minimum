using Minimum.Repositories.Interfaces;
using server.Models;
using System.Collections.Concurrent;
using System.Net.Sockets;

namespace server.Commands;

public class LoginHandler : CommandHandler
{
    public LoginHandler(
        IUserRepository userRepository,
        IChatRepository chatRepository,
        IMessageRepository messageRepository,
        ConcurrentDictionary<string, string> tokens) : base(userRepository, chatRepository, messageRepository, tokens) { }

    public override async Task<Response> HandleAsync(Request request, NetworkStream stream, TcpClient client)
    {
        try
        {
            var user = await UserRepository.GetUserByNameAsync(request.Username);
            if (user != null && user.Password == request.Password)
            {
                string token = Guid.NewGuid().ToString();
                UserTokens.TryAdd(token, user.Name);
                return new Response { Success = true, Message = "Успешный вход.", Token = token };
            }

            return new Response { Success = false, Message = "Неверный логин или пароль." };
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }

        return new Response { Success = false, Message = "123" };
    }
}