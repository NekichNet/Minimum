using Avalonia.Media.Imaging;
using Microsoft.Extensions.DependencyInjection;
using Minimum.Models;
using Minimum.ViewModels;
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
        private readonly CacheService _cacheService;
        public string? Token { get; private set; }


        public ServerConnectionManager(CacheService cacheService)
        {
            client = App.ServiceProvider.GetRequiredService<TcpClientService>().Client;
            _cacheService = cacheService;
            _ = RestoreTokenAsync();
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
            /*
             * Вместо этого теперь цепляемся c помощью += за делегат TcpListenerService.ResponseHandler, который в аргументы пихает Response
             * 
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
            */
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
                await _cacheService.SaveTokenAsync(Token);
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
                await _cacheService.SaveTokenAsync(Token);
            }

            return resp;
        }


        public async Task SignOut()
        {
            Token = null;
            _cacheService.ClearToken();
        }


        public async Task<Response> CreateChat(string chatName)
        {
            var req = new Request()
            {
                Token = Token,
                Type = "create_chat",
                ChatName = chatName
            };
            Response resp = await SendRequest(req);

            return resp;
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
            Response resp = await SendRequest(req);

            return resp;
        }

        public async Task<Response> CheckToken(string token)
        {
            var req = new Request()
            {
                Type = "validate_token",
                Token = token
            };

            return await SendRequest(req);
        }

        public async Task RestoreTokenAsync()
        {
            var cachedToken = await _cacheService.LoadTokenAsync();
            if (!string.IsNullOrWhiteSpace(cachedToken))
            {
                Token = cachedToken;
            }
        }


        /*
         * Вместо этого теперь цепляемся c помощью += за делегат TcpListenerService.MessageHandler, который в аргументы пихает BroadcastMessage
         * 
        public async Task StartListeningAsync(ChatViewModel chatVm)
        {
            var stream = client.GetStream();
            var buffer = new byte[4096];
            var sb = new StringBuilder();

            while (true)
            {
                int bytesRead;
                try
                {
                    bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                }
                catch
                {
                    break;
                }

                if (bytesRead <= 0) break;

                sb.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));

                while (sb.ToString().Contains("\n"))
                {
                    var full = sb.ToString();
                    var idx = full.IndexOf('\n');
                    var jsonLine = full.Substring(0, idx).Trim();
                    sb.Remove(0, idx + 1);

                    if (string.IsNullOrWhiteSpace(jsonLine)) continue;

                    try
                    {
                        var dto = JsonSerializer.Deserialize<BroadcastMessage>(jsonLine);
                        if (dto != null && dto.type == "message_broadcast")
                        {
                            var msg = new Minimum.Models.Message
                            {
                                Id = dto.id,
                                Text = dto.text,
                                Time = dto.time,
                                IsFile = dto.isFile,
                                FileName = dto.fileName,
                                FileId = dto.fileId,
                                IsUploaded = dto.isUploaded,
                                Author = new Minimum.Models.User { Name = dto.author }
                            };

                            await chatVm.AppendMessageAndCacheAsync(chatVm.Id, msg);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Ошибка парсинга: " + ex.Message);
                    }
                }
            }
        }
        */
    }
}