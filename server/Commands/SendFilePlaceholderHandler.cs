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
        ConcurrentDictionary<int, User> usersById,
        ConcurrentDictionary<string, User> usersByName,
        ConcurrentDictionary<string, string> tokens,
        ConcurrentDictionary<int, Chat> chatsById,
        string uploadDir) : base(usersById, usersByName, tokens, chatsById)
    {
        _uploadDir = uploadDir;
    }

    public override Response Handle(Request request, NetworkStream stream, TcpClient client)
    {
        if (!ValidateToken(request.Token, out User user))
        {
            return new Response { Success = false, Message = "Невалидный токен." };
        }

        if (request.ChatId == null)
        {
            return new Response { Success = false, Message = "ID чата не указан." };
        }

        if (!ChatsById.TryGetValue(request.ChatId.Value, out Chat chat))
        {
            return new Response { Success = false, Message = "Чат не найден." };
        }

        var fileMessage = new Message(request.FileName, request.FileSize, request.FileId, user.Id, user);
        chat.Messages.Add(fileMessage);

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