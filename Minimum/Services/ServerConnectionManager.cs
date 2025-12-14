using Avalonia.Media.Imaging;
using Minimum.Models;
using System;
using System.Collections.Generic;
using System.IO;
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
        private TcpClient client;
        public IPEndPoint ServerEndPoint { get; set; }

        public string? Token { get; private set; }


        public ServerConnectionManager()
        {
            client = new TcpClient();
            ServerEndPoint = new IPEndPoint(IPAddress.Any, 31584);
        }



        public async Task StartConnection()
        {
            if (!client.Connected)
            {
                await client.ConnectAsync("127.0.0.1", 31584);
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


            if (string.IsNullOrWhiteSpace(request.Token) && !string.IsNullOrWhiteSpace(Token))
            {
                request.Token = Token;
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



        public async Task<Response> UpdateUser(User user)
        {
            byte[] bytes;
            using (var ms = new MemoryStream())
            {
                // Save the bitmap to the stream in a specific format
                user.Avatar.Save(ms); // Default format is usually PNG or matches the source
                bytes =  ms.ToArray();
            }



            var req = new Request()
            {
                Type = "update_user",
                Username = user.Name,
                Password = user.Password,
                AvatarData = bytes
            };
            return await SendRequest(req);
            
            return new Response();
        }


        public async Task<Response> SignUp(string login, string password)
        {
            var req = new Request()
            {
                Type = "register",
                Username = login,
                Password = password,
            };

            var resp = await SendRequest(req);

            if (resp != null && resp.Success && !string.IsNullOrWhiteSpace(resp.Token))
            {
                Token = resp.Token;
            }

            return resp;
        }


        public async Task<Response> SignIn(string login, string password)
        {
            var req = new Request()
            {
                Type = "login",
                Username = login,
                Password = password
            };

            var resp = await SendRequest(req);

            if (resp != null && resp.Success && !string.IsNullOrWhiteSpace(resp.Token))
            {
                Token = resp.Token;
            }

            return resp;
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
                ChatId = chatId,
                Token = Token
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
                ChatId = chatId,
                Token = Token
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
                IsUploadComplete = isComplete,
                Token = Token
            };
            return await SendRequest(req);
        }


        public async Task<Response> DownloadFile(string fileId, string destinationPath, long expectedFileSize, string? token = null)
        {
            var req = new Request()
            {
                Type = "download_file",
                FileId = fileId,
                Token = token ?? string.Empty
            };
            return await DownloadFile(req, destinationPath, expectedFileSize);
        }


        public async Task<Response> DownloadFile(Request request, string destinationPath, long expectedFileSize)
        {
            if (!client.Connected)
            {
                return new Response { Success = false, Message = "Нет подключения к серверу" };
            }

            if (string.IsNullOrWhiteSpace(request.Token) && !string.IsNullOrWhiteSpace(Token))
            {
                request.Token = Token;
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

            try
            {
                using (var fs = new FileStream(destinationPath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    var buffer = new byte[8192];
                    long remaining = expectedFileSize;
                    while (remaining > 0)
                    {
                        int toRead = (int)Math.Min(buffer.Length, remaining);
                        int read = await stream.ReadAsync(buffer, 0, toRead);
                        if (read == 0)
                        {
                            throw new Exception("Соединение закрыто сервером во время получения файла.");
                        }
                        await fs.WriteAsync(buffer, 0, read);
                        remaining -= read;
                    }
                    await fs.FlushAsync();
                }

                var jsonBytes = new List<byte>();
                var temp = new byte[1024];
                while (true)
                {
                    int r = await stream.ReadAsync(temp, 0, temp.Length);
                    if (r <= 0) break;
                    for (int i = 0; i < r; i++) jsonBytes.Add(temp[i]);

                    if (jsonBytes.Contains((byte)'\n')) break;
                }

                if (jsonBytes.Count == 0)
                {
                    return new Response { Success = true, Message = "Файл сохранён, ответ сервера отсутствует." };
                }

                int newlineIndex = jsonBytes.FindIndex(b => b == (byte)'\n');
                int jsonLength = newlineIndex >= 0 ? newlineIndex + 1 : jsonBytes.Count;
                var jsonSegment = jsonBytes.Take(jsonLength).ToArray();
                string responseJson = Encoding.UTF8.GetString(jsonSegment).TrimEnd('\n', '\r', '\0');

                if (string.IsNullOrWhiteSpace(responseJson))
                {
                    return new Response { Success = true, Message = "Файл сохранён, пустой ответ сервера." };
                }

                try
                {
                    var resp = JsonSerializer.Deserialize<Response>(responseJson);
                    return resp ?? new Response { Success = true, Message = "Файл скачан" };
                }
                catch (JsonException ex)
                {
                    return new Response { Success = true, Message = "Файл скачан, но не удалось распарсить ответ сервера: " + ex.Message };
                }
            }
            catch (Exception ex)
            {
                return new Response { Success = false, Message = "Ошибка загрузки файла: " + ex.Message };
            }
        }


        public async Task<Response> JoinChat(int chatId)
        {
            var req = new Request()
            {
                Type = "join_chat",
                ChatId = chatId,
                Token = Token
            };
            return await SendRequest(req);
        }
    }
}