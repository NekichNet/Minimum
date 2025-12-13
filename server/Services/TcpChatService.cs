using Newtonsoft.Json;
using server.Commands;
using server.Models;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace server.Services;

public class TcpChatService
{
    private readonly TcpListener _listener;
    private readonly ConcurrentDictionary<int, User> _usersById = new();
    private readonly ConcurrentDictionary<string, User> _usersByName = new();
    private readonly ConcurrentDictionary<string, string> _tokens = new();
    private readonly ConcurrentDictionary<int, Chat> _chatsById = new();
    private readonly string _uploadDir = "./uploads";

    private readonly Dictionary<string, Func<Request, NetworkStream, TcpClient, Response>> _handlers;

    public TcpChatService(int port)
    {
        _listener = new TcpListener(IPAddress.Loopback, port);

        _handlers = new Dictionary<string, Func<Request, NetworkStream, TcpClient, Response>>
        {
            ["register"] = (req, s, c) => new RegisterHandler(_usersById, _usersByName, _tokens, _chatsById).Handle(req, s, c),
            ["login"] = (req, s, c) => new LoginHandler(_usersById, _usersByName, _tokens, _chatsById).Handle(req, s, c),
            ["create_chat"] = (req, s, c) => new CreateChatHandler(_usersById, _usersByName, _tokens, _chatsById).Handle(req, s, c),
            ["send_message"] = (req, s, c) => new SendMessageHandler(_usersById, _usersByName, _tokens, _chatsById, _uploadDir).Handle(req, s, c),
            ["join_chat"] = (req, s, c) => new JoinChatHandler(_usersById, _usersByName, _tokens, _chatsById).Handle(req, s, c),
            ["send_file_placeholder"] = (req, s, c) => new SendFilePlaceholderHandler(_usersById, _usersByName, _tokens, _chatsById, _uploadDir).Handle(req, s, c),
            ["upload_file_chunk"] = (req, s, c) => new UploadFileChunkHandler(_usersById, _usersByName, _tokens, _chatsById, _uploadDir).Handle(req, s, c),
            ["download_file"] = (req, s, c) => new DownloadFileHandler(_usersById, _usersByName, _tokens, _chatsById, _uploadDir).Handle(req, s, c),
        };
    }

    public void Start()
    {
        _listener.Start();
        Console.WriteLine($"Сервер запущен на порту {_listener.LocalEndpoint}...");

        try
        {
            while (true)
            {
                var client = _listener.AcceptTcpClient();
                _ = Task.Run(() => HandleClient(client));
            }
        }
        finally
        {
            _listener.Stop();
        }
    }

    private async Task HandleClient(TcpClient client)
    {
        using var stream = client.GetStream();
        var buffer = new byte[4096];
        int bytesRead;

        while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
        {
            var data = Encoding.UTF8.GetString(buffer, 0, bytesRead).TrimEnd('\0', '\n', '\r');
            Request request = null;

            try
            {
                request = JsonConvert.DeserializeObject<Request>(data);
            }
            catch (JsonException)
            {
                await SendJsonResponse(stream, new Response { Success = false, Message = "Неверный формат JSON." });
                continue;
            }

            if (request == null)
            {
                await SendJsonResponse(stream, new Response { Success = false, Message = "Запрос пуст." });
                continue;
            }

            Response response;
            if (_handlers.TryGetValue(request.Type, out var handler))
            {
                response = handler(request, stream, client);
            }
            else
            {
                response = new Response { Success = false, Message = "Неизвестная команда." };
            }

            await SendJsonResponse(stream, response);
        }

        foreach (var chat in _chatsById.Values)
        {
            chat.ConnectedClients.Remove(client);
        }
    }

    private async Task SendJsonResponse(NetworkStream stream, Response response)
    {
        string jsonResponse = JsonConvert.SerializeObject(response) + "\n";
        byte[] bytes = Encoding.UTF8.GetBytes(jsonResponse);
        await stream.WriteAsync(bytes, 0, bytes.Length);
    }
}