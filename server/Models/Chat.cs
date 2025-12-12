namespace server.Models;

public class Chat
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public List<User> Users { get; set; } = new List<User>();
    public List<Message> Messages { get; set; } = new List<Message>();
}
