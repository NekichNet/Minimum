using server.Models;
using System.Collections.Concurrent;
using System.Net.Sockets;

namespace server.Commands;

public class DownloadFileHandler : CommandHandler
{
    private readonly string _uploadDir;

    public DownloadFileHandler(
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
            byte[] fileBytes = File.ReadAllBytes(filePath);
            stream.Write(fileBytes, 0, fileBytes.Length);
            return new Response { Success = true, Message = "Файл отправлен." };
        }
        catch (Exception ex)
        {
            return new Response { Success = false, Message = "Ошибка отправки файла: " + ex.Message };
        }
    }
}