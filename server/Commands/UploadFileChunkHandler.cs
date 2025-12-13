using Newtonsoft.Json;
using server.Models;
using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Text;

namespace server.Commands;

public class UploadFileChunkHandler : CommandHandler
{
    private readonly string _uploadDir;

    public UploadFileChunkHandler(
        ConcurrentDictionary<int, User> usersById,
        ConcurrentDictionary<string, User> usersByName,
        ConcurrentDictionary<string, string> tokens,
        ConcurrentDictionary<int, Chat> chatsById,
        string uploadDir) : base(usersById, usersByName, tokens, chatsById)
    {
        _uploadDir = uploadDir;
        Directory.CreateDirectory(_uploadDir);
    }

    public override Response Handle(Request request, NetworkStream stream, TcpClient client)
    {
        if (!ValidateToken(request.Token, out User user))
        {
            return new Response { Success = false, Message = "Невалидный токен." };
        }

        if (string.IsNullOrEmpty(request.FileId))
        {
            return new Response { Success = false, Message = "ID файла не указан." };
        }

        string filePath = Path.Combine(_uploadDir, request.FileId);

        try
        {
            if (request.FileData != null)
            {
                using var fs = new FileStream(filePath, FileMode.Append, FileAccess.Write);
                fs.Write(request.FileData, 0, request.FileData.Length);
            }

            if (request.IsUploadComplete)
            {
                foreach (var chat in ChatsById.Values)
                {
                    var msg = chat.Messages.FirstOrDefault(m => m.FileId == request.FileId);
                    if (msg != null)
                    {
                        msg.IsUploaded = true;
                        BroadcastFileUpdate(chat, msg, user);
                        break;
                    }
                }
            }

            return new Response { Success = true, Message = "Часть файла загружена." };
        }
        catch (Exception ex)
        {
            return new Response { Success = false, Message = "Ошибка загрузки файла: " + ex.Message };
        }
    }

    private void BroadcastFileUpdate(Chat chat, Message message, User author)
    {
        var broadcastMsg = new
        {
            type = "file_uploaded_broadcast",
            id = message.Id,
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