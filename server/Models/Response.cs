namespace server.Models;

public class Response // it is DTO
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public string Token { get; set; }
}
