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

    public Message(string text, int authorId, int chatId, User author, Chat chat)
    {
        Text = text;
        AuthorId = authorId;
        ChatId = chatId;
        Author = author;
        Chat = chat;
        Time = DateTime.UtcNow;
    }

    public Message(string fileName, long fileSize, string fileId, int authorId, int chatId, User author, Chat chat)
    {
        Text = $"Файл: {fileName}";
        AuthorId = authorId;
        ChatId = chatId;
        Author = author;
        Chat = chat;
        Time = DateTime.UtcNow;
        IsFile = true;
        FileName = fileName;
        FileSize = fileSize;
        FileId = fileId;
        IsUploaded = false;
    }
}
