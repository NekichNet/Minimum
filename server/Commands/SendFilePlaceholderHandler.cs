using Minimum.Repositories.Interfaces;
using Newtonsoft.Json;
using server.Models;
using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Text;

namespace server.Commands;

public class SendFilePlaceholderHandler : CommandHandler
{
    private readonly string _uploadDir;

    public SendFilePlaceholderHandler(
        IUserRepository userRepository,
        IChatRepository chatRepository,
        IMessageRepository messageRepository,
        ConcurrentDictionary<string, string> tokens,
        string uploadDir) : base(userRepository, chatRepository, messageRepository, tokens)
    {
        _uploadDir = uploadDir;
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

        var fileMessage = new Message(request.FileName, request.FileSize, request.FileId, user.Id, chat.Id, user, chat);
        await MessageRepository.AddMessageAsync(fileMessage);

        BroadcastMessageToChat(chat, fileMessage, user);

        return new Response { Success = true, Message = "Плейсхолдер файла отправлен." };
    }

    private void BroadcastMessageToChat(Chat chat, Message message, User author)
    {
        var broadcastMsg = new
        {
            type = "file_placeholder_broadcast",
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

        foreach (var chatClient in chat.ConnectedClients.ToList())
        {
            if (!chatClient.Connected) continue;

            try
            {
                var clientStream = chatClient.GetStream();
                clientStream.Write(bytes, 0, bytes.Length);
            }
            catch
            {
                chat.ConnectedClients.Remove(chatClient);
            }
        }
    }
}