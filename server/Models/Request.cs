namespace server.Models;

public class Request // it is DTO
{
    public string Type { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string ChatName { get; set; }
    public string Message { get; set; }
    public string Token { get; set; }
}
