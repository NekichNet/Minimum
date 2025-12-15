using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Minimum.Services
{
    public class TcpClientService
    {
        public TcpClient Client { get; }
        public IPEndPoint ServerEndPoint { get; }

        public TcpClientService()
        {
            Client = new TcpClient();
            ServerEndPoint = new IPEndPoint(IPAddress.Any, 31584);
            Client.ConnectAsync("127.0.0.1", 31584);
        }

        public void CloseConnection()
        {
            Client.Close();
        }
    }
}
