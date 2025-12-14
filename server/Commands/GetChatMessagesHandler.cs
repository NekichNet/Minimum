using Minimum.Repositories.Interfaces;
using server.Models;
using System.Net.Sockets;

namespace server.Commands;

public class GetChatMessagesHandler : CommandHandler
{
    public GetChatMessagesHandler(
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

        if (request.ChatId == null)
        {
            return new Response { Success = false, Message = "ID чата не указан." };
        }

        var messages = await MessageRepository.GetLastMessagesAsync(request.ChatId.Value, request.Limit ?? 25);

        var messageList = messages.Select(m => new { m.Text, m.Author.Name, m.Time, m.IsFile, m.FileName, m.IsUploaded }).ToList();

        return new Response
        {
            Success = true,
            Message = "Сообщения получены.",
            Data = messageList
        };
    }
}