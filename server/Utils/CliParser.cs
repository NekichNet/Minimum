using server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Utils
{
    public class CliParser
    {
        public static Request ParseInput(string input)
        {
            var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var command = parts[0].ToLower();

            var request = new Request { Type = command };

            foreach (var part in parts.Skip(1))
            {
                var kv = part.Split('=');
                if (kv.Length == 2)
                {
                    switch (kv[0].ToLower())
                    {
                        case "--username": request.Username = kv[1]; break;
                        case "--password": request.Password = kv[1]; break;
                        case "--token": request.Token = kv[1]; break;
                        case "--chatname": request.ChatName = kv[1]; break;
                        case "--chatid": request.ChatId = int.Parse(kv[1]); break;
                        case "--text": request.MessageText = kv[1]; break;
                    }
                }
            }

            return request;
        }
    }
}
