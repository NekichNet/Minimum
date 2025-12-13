using Minimum.Models;
using System;
using System.Diagnostics;
using System.IO;
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
            ServerEndPoint = new IPEndPoint(IPAddress.Any, 8080);
        }


        public async Task StartConnection()
        {
            if (ServerEndPoint != null)
            {
                await client.ConnectAsync("127.0.0.1", 8080);
            }
            else
            {
                throw new Exception("TcpClient is NULL");
            }
        }



        private async Task<Response> SendRequest(Request request)
        {
            var serializedReq = JsonSerializer.Serialize(request);

            var stream = client.GetStream();
            var messageBytes = Encoding.UTF8.GetBytes(serializedReq.ToString());
            var lengthPrefix = BitConverter.GetBytes(messageBytes.Length);

            await stream.WriteAsync(lengthPrefix, 0, lengthPrefix.Length);
            await stream.WriteAsync(messageBytes, 0, messageBytes.Length);


            byte[] lengthBuffer = new byte[4];
            int bytesRead = 0;

            while (bytesRead < 4)
            {
                int result = await stream.ReadAsync(lengthBuffer, bytesRead, 4 - bytesRead);
                if (result == 0)
                {
                    throw new IOException("Соединение закрыто удалённой стороной");
                }

                bytesRead += result;
            }

            int messageLength = BitConverter.ToInt32(lengthBuffer, 0);

            if (messageLength <= 0 || messageLength > 10 * 1024 * 1024)
            {
                throw new InvalidOperationException($"Некорректная длина сообщения: {messageLength}");
            }

            byte[] messageBuffer = new byte[messageLength];
            bytesRead = 0;

            while (bytesRead < messageLength)
            {
                int result = await stream.ReadAsync(messageBuffer, bytesRead, messageLength - bytesRead);
                if (result == 0)
                {
                    throw new IOException("Соединение закрыто во время чтения");
                }

                bytesRead += result;
            }

            string message = Encoding.UTF8.GetString(messageBuffer);

            //var bufferedReq = UTF8Encoding.UTF8.GetBytes(serializedReq);
            //var buffer = new byte[1024];
            //if (client.Connected)
            //{
            //    await client.Client.SendAsync(bufferedReq);
            //    var responceText = await client.Client.ReceiveAsync(buffer);
            //    var responce = JsonSerializer.Deserialize<Response>(responceText);
            //    return responce;
            //}

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
