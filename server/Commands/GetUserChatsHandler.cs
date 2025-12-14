using Minimum.Repositories.Interfaces;
using server.Models;
using System.Net.Sockets;

namespace server.Commands;

public class GetUserChatsHandler : CommandHandler
{
    public GetUserChatsHandler(
        IUserRepository userRepository,
        IChatRepository chatRepository,
        IMessageRepository messageRepository) : base(userRepository, chatRepository, messageRepository) { }

    public override async Task<Response> HandleAsync(Request request, NetworkStream stream, TcpClient client)
    {
        var (isValid, user) = await ValidateTokenAsync(request.Token);
        if (!isValid)
        {
            return new Response { Success = false, Message = "Невалидный токен." };
        }

        var chats = await ChatRepository.GetUserChatsAsync(user.Id);

        var chatNames = chats.Select(c => c.Name).ToList();

        return new Response { Success = true, Chats = chatNames };
    }
}