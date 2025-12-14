using Minimum.Repositories.Interfaces;
using Newtonsoft.Json;
using server.Models;
using server.Services;
using System.Net.Sockets;
using System.Text;

namespace server.Commands;

public class SendMessageHandler : CommandHandler
{
    private readonly string _uploadDir;
    private readonly ChatConnectionService _chatConnectionService;

    public SendMessageHandler(
        IUserRepository userRepository,
        IChatRepository chatRepository,
        IMessageRepository messageRepository,
        string uploadDir,
        ChatConnectionService chatConnectionService) : base(userRepository, chatRepository, messageRepository)
    {
        _uploadDir = uploadDir;
        _chatConnectionService = chatConnectionService;
    }

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

        var chat = await ChatRepository.GetChatByIdAsync(request.ChatId.Value);
        if (chat == null)
        {
            return new Response { Success = false, Message = "Чат не найден." };
        }

        var message = new Message(request.MessageText, user.Id, chat.Id, user, chat);
        await MessageRepository.AddMessageAsync(message);

        // Рассылаем сообщение всем подключенным клиентам в чате
        BroadcastMessageToChat(chat.Id, message, user);

        return new Response { Success = true, Message = "Сообщение отправлено." };
    }

    private void BroadcastMessageToChat(int chatId, Message message, User author)
    {
        var broadcastMsg = new
        {
            type = "message_broadcast",
            id = message.Id,
            text = message.Text,
            author = author.Name,
            time = message.Time,
            isFile = message.IsFile,
            fileName = message.FileName,
            fileId = message.FileId,
            isUploaded = message.IsUploaded
        };

        string json = JsonConvert.SerializeObject(broadcastMsg) + "\n";
        byte[] bytes = Encoding.UTF8.GetBytes(json);

        var clients = _chatConnectionService.GetClients(chatId);

        foreach (var chatClient in clients.ToList())
        {
            if (!chatClient.Connected) continue;

            try
            {
                var clientStream = chatClient.GetStream();
                clientStream.Write(bytes, 0, bytes.Length);
            }
            catch
            {
                _chatConnectionService.RemoveClient(chatClient);
            }
        }
    }
}