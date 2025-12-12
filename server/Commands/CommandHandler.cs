using server.Models;
using System.Collections.Concurrent;

namespace server.Commands;

public abstract class CommandHandler
{
    protected readonly ConcurrentDictionary<int, User> UsersById;
    protected readonly ConcurrentDictionary<string, User> UsersByName;
    protected readonly ConcurrentDictionary<string, string> UserTokens;
    protected readonly ConcurrentDictionary<int, Chat> ChatsById;

    public CommandHandler(
        ConcurrentDictionary<int, User> usersById,
        ConcurrentDictionary<string, User> usersByName,
        ConcurrentDictionary<string, string> tokens,
        ConcurrentDictionary<int, Chat> chatsById)
    {
        UsersById = usersById;
        UsersByName = usersByName;
        UserTokens = tokens;
        ChatsById = chatsById;
    }

    public abstract Response Handle(Request request);

    protected bool ValidateToken(string token, out User user)
    {
        user = null;
        if (string.IsNullOrEmpty(token) || !UserTokens.TryGetValue(token, out string username))
        {
            return false;
        }

        return UsersByName.TryGetValue(username, out user);
    }
}