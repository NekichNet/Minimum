namespace server.Models;

public class Request // dto
{
    public string Type { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string ChatName { get; set; }
    public int? ChatId { get; set; }
    public string MessageText { get; set; }
    public string Token { get; set; }
}