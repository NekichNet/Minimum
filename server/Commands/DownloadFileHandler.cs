using Minimum.Repositories.Interfaces;
using server.Models;
using System.Collections.Concurrent;
using System.Net.Sockets;

namespace server.Commands;

public class DownloadFileHandler : CommandHandler
{
    private readonly string _uploadDir;

    public DownloadFileHandler(
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

        if (string.IsNullOrEmpty(request.FileId))
        {
            return new Response { Success = false, Message = "ID файла не указан." };
        }

        string filePath = Path.Combine(_uploadDir, request.FileId);

        if (!File.Exists(filePath))
        {
            return new Response { Success = false, Message = "Файл не найден." };
        }

        try
        {
            byte[] fileBytes = await File.ReadAllBytesAsync(filePath);
            await stream.WriteAsync(fileBytes, 0, fileBytes.Length);
            return new Response { Success = true, Message = "Файл отправлен." };
        }
        catch (Exception ex)
        {
            return new Response { Success = false, Message = "Ошибка отправки файла: " + ex.Message };
        }
    }
}