namespace server.Models;

public class Response // dto
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public string Token { get; set; }
    public int? ChatId { get; set; }

    public List<string> Chats { get; set; }
}