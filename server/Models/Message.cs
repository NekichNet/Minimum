namespace server.Models;

public class Message
{
    public int Id { get; set; }
    public string Text { get; set; }
    public DateTime Time { get; set; }
    public int ChatId { get; set; }
    public int AuthorId { get; set; }

    public Chat Chat { get; set; }
    public User Author { get; set; }

    public Message() { }

    public Message(int id, string text, User author)
    {
        Id = id;
        Text = text;
        Time = DateTime.UtcNow;
        AuthorId = author.Id;
        Author = author;
    }
}
