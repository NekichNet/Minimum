namespace server.Models;

public class AuthToken
{
    public int Id { get; set; }
    public string Token { get; set; } = string.Empty;
    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ExpiresAt { get; set; } = DateTime.UtcNow.AddDays(7);


    public virtual User User { get; set; } = null!;
}