namespace server.Models;

public class Message
{
    public int Id { get; set; }
    public string Text { get; set; }
    public DateTime Time { get; set; }
    public int ChatId { get; set; }
    public int AuthorId { get; set; }
    public bool IsFile { get; set; } = false;
    public string FileName { get; set; }
    public long FileSize { get; set; }
    public string FileId { get; set; }
    public bool IsUploaded { get; set; } = false;


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

    public Message(string fileName, long fileSize, string fileId, int authorId, User author)
    {
        Text = $"Файл: {fileName}";
        AuthorId = authorId;
        Author = author;
        Time = DateTime.UtcNow;
        IsFile = true;
        FileName = fileName;
        FileSize = fileSize;
        FileId = fileId;
        IsUploaded = false;
    }
}
