using System.Collections.Concurrent;
using System.Net.Sockets;

namespace server.Services;

public class ChatConnectionService
{
    private readonly ConcurrentDictionary<int, List<TcpClient>> _chatConnections = new();

    public void AddClient(int chatId, TcpClient client)
    {
        _chatConnections.AddOrUpdate(chatId, new List<TcpClient> { client },
            (key, list) =>
            {
                list.Add(client);
                return list;
            });
    }

    public void RemoveClient(TcpClient client)
    {
        foreach (var kvp in _chatConnections)
        {
            kvp.Value.Remove(client);
        }
    }

    public List<TcpClient> GetClients(int chatId)
    {
        return _chatConnections.TryGetValue(chatId, out var clients) ? clients : new List<TcpClient>();
    }
}