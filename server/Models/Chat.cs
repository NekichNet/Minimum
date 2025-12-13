using System.Net.Sockets;
using System.Text.Json.Serialization;

namespace server.Models;

public class Chat
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    [JsonIgnore]
    public List<User> Users { get; set; } = new List<User>();
    [JsonIgnore]
    public List<Message> Messages { get; set; } = new List<Message>();

    public List<TcpClient> ConnectedClients { get; set; } = new List<TcpClient>();
}
