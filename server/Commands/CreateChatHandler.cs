using Minimum.Repositories.Interfaces;
using server.Models;
using System.Collections.Concurrent;
using System.Net.Sockets;

namespace server.Commands;

public class CreateChatHandler : CommandHandler
{
    public CreateChatHandler(
        IUserRepository userRepository,
        IChatRepository chatRepository,
        IMessageRepository messageRepository,
        ConcurrentDictionary<string, string> tokens) : base(userRepository, chatRepository, messageRepository, tokens) { }

    public override async Task<Response> HandleAsync(Request request, NetworkStream stream, TcpClient client)
    {
        var (isValid, user) = await ValidateTokenAsync(request.Token);
        if (!isValid)
        {
            return new Response { Success = false, Message = "Невалидный токен." };
        }

        var newChat = new Chat { Name = request.ChatName };
        await ChatRepository.AddChatAsync(newChat);

        newChat.Users.Add(user);
        await ChatRepository.UpdateChatAsync(newChat);

        return new Response { Success = true, Message = "Чат создан.", ChatId = newChat.Id };
    }
}