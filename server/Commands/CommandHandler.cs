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
    protected readonly ConcurrentDictionary<string, string> UserTokens;

    public CommandHandler(
        IUserRepository userRepository,
        IChatRepository chatRepository,
        IMessageRepository messageRepository,
        ConcurrentDictionary<string, string> tokens)
    {
        UserRepository = userRepository;
        ChatRepository = chatRepository;
        MessageRepository = messageRepository;
        UserTokens = tokens;
    }

    public abstract Task<Response> HandleAsync(Request request, NetworkStream stream, TcpClient client);

    protected bool ValidateToken(string token, out User user)
    {
        user = null;
        if (string.IsNullOrEmpty(token) || !UserTokens.TryGetValue(token, out string username))
        {
            return false;
        }

        user = UserRepository.GetUserByNameAsync(username).Result;
        return user != null;
    }

    protected async Task<(bool isValid, User user)> ValidateTokenAsync(string token)
    {
        if (string.IsNullOrEmpty(token) || !UserTokens.TryGetValue(token, out string username))
        {
            return (false, null);
        }

        var user = await UserRepository.GetUserByNameAsync(username);
        return (user != null, user);
    }
}