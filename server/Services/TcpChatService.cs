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

    private readonly Dictionary<string, Func<Request, Response>> _handlers;

    public TcpChatService(int port)
    {
        _listener = new TcpListener(IPAddress.Loopback, port);

        _handlers = new Dictionary<string, Func<Request, Response>>
        {
            ["register"] = req => new RegisterHandler(_usersById, _usersByName, _tokens, _chatsById).Handle(req),
            ["login"] = req => new LoginHandler(_usersById, _usersByName, _tokens, _chatsById).Handle(req),
            ["create_chat"] = req => new CreateChatHandler(_usersById, _usersByName, _tokens, _chatsById).Handle(req),
            ["send_message"] = req => new SendMessageHandler(_usersById, _usersByName, _tokens, _chatsById).Handle(req),
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
            catch (System.Text.Json.JsonException)
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
                response = handler(request);
            }
            else
            {
                response = new Response { Success = false, Message = "Неизвестная команда." };
            }

            await SendJsonResponse(stream, response);
        }
    }

    private async Task SendJsonResponse(NetworkStream stream, Response response)
    {
        string jsonResponse = JsonConvert.SerializeObject(response) + "\n";
        byte[] bytes = Encoding.UTF8.GetBytes(jsonResponse);
        await stream.WriteAsync(bytes, 0, bytes.Length);
    }
}