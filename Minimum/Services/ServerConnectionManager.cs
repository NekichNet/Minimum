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
        private TcpClient client;
        public IPEndPoint ServerEndPoint { get; set; }

        public ServerConnectionManager()
        {
            client = new TcpClient();
            ServerEndPoint = new IPEndPoint(IPAddress.Any, 8080);
        }

        public async Task StartConnection()
        {
            if (!client.Connected)
            {
                await client.ConnectAsync("127.0.0.1", 8080);
            }
            else
            {
                throw new Exception("TcpClient уже подключен");
            }
        }

        private async Task<Response> SendRequest(Request request)
        {
            if (!client.Connected)
            {
                return new Response { Success = false, Message = "Нет подключения к серверу" };
            }

            var serializedReq = JsonSerializer.Serialize(request);
            var stream = client.GetStream();
            var messageBytes = Encoding.UTF8.GetBytes(serializedReq + "\n");

            try
            {
                await stream.WriteAsync(messageBytes, 0, messageBytes.Length);
            }
            catch (Exception ex)
            {
                return new Response { Success = false, Message = $"Ошибка отправки: {ex.Message}" };
            }

            var buffer = new byte[4096];
            int bytesRead;
            try
            {
                bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
            }
            catch (Exception ex)
            {
                return new Response { Success = false, Message = $"Ошибка чтения: {ex.Message}" };
            }

            string responseJson = Encoding.UTF8.GetString(buffer, 0, bytesRead).TrimEnd('\0', '\n', '\r');

            if (string.IsNullOrWhiteSpace(responseJson))
            {
                return new Response { Success = false, Message = "Пустой ответ от сервера" };
            }

            try
            {
                return JsonSerializer.Deserialize<Response>(responseJson);
            }
            catch (JsonException ex)
            {
                return new Response { Success = false, Message = $"Не JSON: {ex.Message}" };
            }
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
                MessageText = message,
                ChatId = chatId
            };
            return await SendRequest(req);
        }

        public async Task<Response> SendFilePlaceholder(string fileName, long fileSize, string fileId, int chatId)
        {
            var req = new Request()
            {
                Type = "send_file_placeholder",
                FileName = fileName,
                FileSize = fileSize,
                FileId = fileId,
                ChatId = chatId
            };
            return await SendRequest(req);
        }

        public async Task<Response> UploadFileChunk(string fileId, byte[] fileData, bool isComplete)
        {
            var req = new Request()
            {
                Type = "upload_file_chunk",
                FileId = fileId,
                FileData = fileData,
                IsUploadComplete = isComplete
            };
            return await SendRequest(req);
        }

        public async Task<Response> DownloadFile(string fileId)
        {
            var req = new Request()
            {
                Type = "download_file",
                FileId = fileId
            };
            return await SendRequest(req); // фывфыв
        }

        public async Task<Response> JoinChat(int chatId)
        {
            var req = new Request()
            {
                Type = "join_chat",
                ChatId = chatId
            };
            return await SendRequest(req);
        }
    }
}