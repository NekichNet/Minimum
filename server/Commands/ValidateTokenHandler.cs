using Minimum.Repositories.Interfaces;
using server.Models;
using System.Net.Sockets;

namespace server.Commands;

public class ValidateTokenHandler : CommandHandler
{
    public ValidateTokenHandler(
        IUserRepository userRepository,
        IChatRepository chatRepository,
        IMessageRepository messageRepository) : base(userRepository, chatRepository, messageRepository) { }

    public override async Task<Response> HandleAsync(Request request, NetworkStream stream, TcpClient client)
    {
        if (string.IsNullOrEmpty(request.Token))
        {
            return new Response { Success = false, Message = "Токен не указан." };
        }

        var tokenEntity = await UserRepository.GetTokenByValueAsync(request.Token);

        if (tokenEntity != null)
        {
            if (tokenEntity.ExpiresAt.HasValue && tokenEntity.ExpiresAt.Value < DateTime.UtcNow)
            {
                await UserRepository.DeleteTokenAsync(request.Token);
                return new Response { Success = false, Message = "Токен истёк." };
            }

            return new Response { Success = true, Message = "Токен валиден." };
        }

        return new Response { Success = false, Message = "Токен недействителен." };
    }
}