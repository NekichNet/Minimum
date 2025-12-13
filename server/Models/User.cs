using System.Text.Json.Serialization;

namespace server.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string AvatarPath { get; set; } = string.Empty;

    [JsonIgnore]
    public List<Message> Messages { get; set; } = new List<Message>();
    [JsonIgnore]
    public List<Chat> Chats { get; set; } = new List<Chat>();
}
