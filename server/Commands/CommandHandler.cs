using Minimum.Repositories.Interfaces;
using server.Models;
using System.Collections.Concurrent;
using System.Net.Sockets;

namespace server.Commands;

public abstract class CommandHandler
{
    protected readonly IUserRepository UserRepository;
    protected readonly IChatRepository ChatRepository;
    protected readonly IMessageRepository MessageRepository;

    public CommandHandler(
        IUserRepository userRepository,
        IChatRepository chatRepository,
        IMessageRepository messageRepository)
    {
        UserRepository = userRepository;
        ChatRepository = chatRepository;
        MessageRepository = messageRepository;
    }

    public abstract Task<Response> HandleAsync(Request request, NetworkStream stream, TcpClient client);

    protected async Task<(bool isValid, User user)> ValidateTokenAsync(string token)
    {
        if (string.IsNullOrEmpty(token))
        {
            return (false, null);
        }

        var tokenEntity = await UserRepository.GetTokenByValueAsync(token);
        if (tokenEntity != null)
        {
            if (tokenEntity.ExpiresAt.HasValue && tokenEntity.ExpiresAt.Value < DateTime.UtcNow)
            {
                await UserRepository.DeleteTokenAsync(token);
                return (false, null);
            }

            return (true, tokenEntity.User);
        }

        return (false, null);
    }
}