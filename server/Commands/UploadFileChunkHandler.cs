using Minimum.Repositories.Interfaces;
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
        IUserRepository userRepository,
        IChatRepository chatRepository,
        IMessageRepository messageRepository,
        ConcurrentDictionary<string, string> tokens,
        string uploadDir) : base(userRepository, chatRepository, messageRepository, tokens)
    {
        _uploadDir = uploadDir;
        Directory.CreateDirectory(_uploadDir);
    }

    public override async Task<Response> HandleAsync(Request request, NetworkStream stream, TcpClient client)
    {
        var (isValid, user) = await ValidateTokenAsync(request.Token);
        if (!isValid)
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
            if (request.FileData.Length > 0)
            {
                using var fs = new FileStream(filePath, FileMode.Append, FileAccess.Write);
                await fs.WriteAsync(request.FileData, 0, request.FileData.Length);
            }

            if (request.IsUploadComplete)
            {
                var message = await MessageRepository.GetMessageByFileIdAsync(request.FileId);
                if (message != null)
                {
                    message.IsUploaded = true;
                    await MessageRepository.UpdateMessageAsync(message);

                    var chat = await ChatRepository.GetChatByIdAsync(message.ChatId);
                    if (chat != null)
                    {
                        BroadcastFileUpdate(chat, message, user);
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