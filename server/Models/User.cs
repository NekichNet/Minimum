using System.Text.Json.Serialization;

namespace server.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string AvatarPath { get; set; } = string.Empty;
    public string? Theme { get; set; }
    public string? Accent1 { get; set; }
    public string? Accent2 { get; set; }
    public string? AccentForeground { get; set; }


    [JsonIgnore]
    public List<Message> Messages { get; set; } = new List<Message>();
    [JsonIgnore]
    public List<Chat> Chats { get; set; } = new List<Chat>();
    [JsonIgnore]
    public List<AuthToken> AuthTokens { get; set; } = new List<AuthToken>();
}
