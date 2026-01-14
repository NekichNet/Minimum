using Microsoft.Extensions.DependencyInjection;
using Minimum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;

namespace Minimum.Services
{
    public class TcpListenerService
    {
        private TcpClient _client;

        public delegate void MessageHandler(BroadcastMessage message);
        public delegate void ResponseHandler(Response response);

        // Привязываемся к этим делегатам
        public MessageHandler messageHandler { get; set; } // Вызывается, когда получает сообщение другого пользователя
        public ResponseHandler responseHandler { get; set; } // Вызывается, когда получает Response в ответ на отправку Request

        public TcpListenerService() {
            _client = App.ServiceProvider.GetRequiredService<TcpClientService>().Client;
            Task.Run(() => ListeningAsync());
        }

        private async Task ListeningAsync()
        {
            var stream = _client.GetStream();
            var buffer = new byte[4096];
            var sb = new StringBuilder();

            while (_client.Connected)
            {
                int bytesRead;

                try
                {
                    bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка чтения из сокета: " + ex.Message);
                    break;
                }

                if (bytesRead <= 0)
                {
                    break;
                }

                sb.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));

                //do
                //{
                //    bytesRead = await stream.ReadAsync(buffer);
                //    sb.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));
                //} while (bytesRead > 0);

                string jsonString = sb.ToString();
                sb.Clear();

                if (!string.IsNullOrWhiteSpace(jsonString))
                {
                    try
                    {
                        JsonObject? dto = JsonSerializer.Deserialize<JsonObject>(jsonString);

                        if (dto != null && dto.ContainsKey("type"))
                        {
                            var message = dto.Deserialize<BroadcastMessage>();
                            if (message != null)
                            {
                                messageHandler?.Invoke(message);
                            }
                        }
                        else //if (dto != null && dto.ContainsKey("Data"))
                        {
                            var response = dto.Deserialize<Response>();
                            if (response != null)
                            {
                                responseHandler?.Invoke(response);
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Ошибка парсинга: " + ex.Message);
                    }
                }
            }
        }
    }
}
