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
        IMessageRepository messageRepository) : base(userRepository, chatRepository, messageRepository) { }

    public override async Task<Response> HandleAsync(Request request, NetworkStream stream, TcpClient client)
    {
        var user = await UserRepository.GetUserByNameAsync(request.Username);
        if (user != null && user.Password == request.Password)
        {
            string token = Guid.NewGuid().ToString();

            var authToken = new AuthToken
            {
                Token = token,
                UserId = user.Id,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = null
            };

            await UserRepository.AddTokenAsync(authToken);

            return new Response { Success = true, Message = "Успешный вход.", Token = token };
        }

        return new Response { Success = false, Message = "Неверный логин или пароль." };
    }
}