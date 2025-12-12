using server.Services;

namespace server;

internal class Program
{
    static void Main(string[] args)
    {
        var server = new TcpChatService(8080);
        server.Start();
    }
}
