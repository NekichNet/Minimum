﻿using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Minimum.Repositories.Interfaces;
using server.Data;
using server.Models;
using server.Repositories;
using server.Services;
using server.Utils;

namespace server;

internal class Program
{
    private const int _port = 31584;

    static async Task Main(string[] args)
    {
        Env.Load(".env");

        var connectionString = Env.GetString("DB_CONNECTION_STRING")
            ?? throw new InvalidOperationException("Connection string not configured.");

        var builder = Host.CreateDefaultBuilder(args);

        builder.ConfigureServices((context, services) =>
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(connectionString));

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IChatRepository, ChatRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();

            services.AddSingleton<TcpChatService>(provider =>
            {
                var userRepository = provider.GetRequiredService<IUserRepository>();
                var chatRepository = provider.GetRequiredService<IChatRepository>();
                var messageRepository = provider.GetRequiredService<IMessageRepository>();
                return new TcpChatService(_port, userRepository, chatRepository, messageRepository);
            });
        });

        var host = builder.Build();

        // вот тут сервер запускается
        var server = host.Services.GetRequiredService<TcpChatService>();
        _ = Task.Run(() => server.Start());
        _ = Task.Run(async () => await host.RunAsync());

        // а что идёт дальше даже бог не знает

        Console.WriteLine("CLI сервер запущен. Введите команду (или 'exit' для выхода):");

        while (true)
        {
            string? input = Console.ReadLine();
            if (string.IsNullOrEmpty(input)) continue;

            var request = CliParser.ParseInput(input);
            if (request.Type == "exit")
            {
                Console.WriteLine("Завершение работы CLI сервера...");
                break;
            }

            Response response = new Response
            {
                Success = true,
                Message = $"Команда '{request.Type}' передана на обработку."
            };

            // Логирование
            Console.WriteLine($"[LOG] Команда: {request.Type}, Пользователь: {request.Username ?? "N/A"}");
            Console.WriteLine($"[{(response.Success ? "OK" : "ERROR")}] {response.Message}");

            if (!string.IsNullOrEmpty(response.Token))
            {
                Console.WriteLine($"Ваш токен: {response.Token}");
            }
            if (response.ChatId.HasValue)
            {
                Console.WriteLine($"ID чата: {response.ChatId}");
            }
        }
    }
}
