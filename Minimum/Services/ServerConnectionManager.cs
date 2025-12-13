using Minimum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Minimum.Services
{
    public class ServerConnectionManager
    {
        TcpClient client;
        public IPEndPoint ServerEndPoint { get; set; }

        public ServerConnectionManager()
        {
            client = new TcpClient();
        }


        public async Task StartConnection()
        {
            if (ServerEndPoint != null)
            {
                await client.ConnectAsync(ServerEndPoint);

            }
            else
            {
                throw new Exception("TcpClient is NULL");
            }
        }



        private async Task<Response> SendRequest(Request request)
        {
            var serializedReq = JsonSerializer.Serialize(request);
            var bufferedReq = UTF8Encoding.UTF8.GetBytes(serializedReq);
            var buffer = new byte[1024];
            if (client.Connected)
            {
                await client.Client.SendAsync(bufferedReq);
                var responceText = await client.Client.ReceiveAsync(buffer);
                var responce = JsonSerializer.Deserialize<Response>(responceText);
                return responce;
            }

            return new Response() { Success = false, Message="Нет соединения с сервером" };
        }





        public async Task<Response> SignUp(string login, string password)
        {
            var req = new Request()
            {
                Type = "register",
                Username = login,
                Password = password,
            };
            return await SendRequest(req);
        }
        public async Task<Response> SignIn(string login, string password)
        {
            var req = new Request()
            {
                Type = "login",
                Username = login,
                Password = password
            };

            return await SendRequest(req);
        }
        public async Task<Response> CreateChat(string chatName)
        {
            var req = new Request()
            {
                Type = "create_chat",
                ChatName = chatName
            };
            return await SendRequest(req);
        }
        public async Task<Response> SendMessage(string message, int chatId)
        {
            var req = new Request()
            {
                Type = "send_message",
                ChatId = chatId
            };
            return await SendRequest(req);
        }
        public async Task<Response> SendFilePlaceholder(string path)
        {
            var req = new Request()
            {
                Type = "send_file_placeholder"
            };
            return await SendRequest(req);
        }
        public async Task<Response> UploadFileChunk(string path)
        {
            var req = new Request()
            {
                Type = "upload_file_chunk"
            };
            return await SendRequest(req);
        }
        public async Task<Response> DownloadFile()
        {
            var req = new Request()
            {
                Type = "download_file"
            };
            return await SendRequest(req);
        }
        public async Task<Response> JoinChat(int ChatId)
        {

            var req = new Request()
            {
                Type = "join_chat",
                ChatId = ChatId
            };
            return await SendRequest(req);
        }
    }
}
