using server.Models;
using System.Collections.Concurrent;

namespace server.Commands;

public abstract class CommandHandler
{
    protected readonly ConcurrentDictionary<string, string> Users;
    protected readonly ConcurrentDictionary<string, string> UserTokens;
    protected readonly ConcurrentDictionary<string, Chat> Chats;

    public CommandHandler(
        ConcurrentDictionary<string, string> users,
        ConcurrentDictionary<string, string> tokens,
        ConcurrentDictionary<string, Chat> chats)
    {
        Users = users;
        UserTokens = tokens;
        Chats = chats;
    }

    public abstract Response Handle(Request request);

    protected bool ValidateToken(string token, out string username)
    {
        username = null;
        return !string.IsNullOrEmpty(token) && UserTokens.TryGetValue(token, out username);
    }
}