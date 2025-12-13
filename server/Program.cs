﻿using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using server.Commands;
using server.Data;
using server.Models;
using server.Services;
using server.Utils;
using System.Collections.Concurrent;

namespace server;

internal class Program
{
    private static int _userIdCounter = 1;
    private static int _chatIdCounter = 1;

    static void Main(string[] args)
    {
        Env.Load("../../../");

        var connectionString = Env.GetString("DB_CONNECTION_STRING")
            ?? throw new InvalidOperationException("Connection string not configured.");

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseNpgsql(connectionString);

        using var db = new AppDbContext(optionsBuilder.Options);

        Console.WriteLine("EF Core connected to db!");

        var server = new TcpChatService(8080);
        server.Start();



        var usersById = new ConcurrentDictionary<int, User>();
        var usersByName = new ConcurrentDictionary<string, User>();
        var tokens = new ConcurrentDictionary<string, string>();
        var chatsById = new ConcurrentDictionary<int, Chat>();

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

            CommandHandler handler = request.Type switch
            {
                "register" => new RegisterHandler(usersById, usersByName, tokens, chatsById),
                "login" => new LoginHandler(usersById, usersByName, tokens, chatsById),
                "create-chat" => new CreateChatHandler(usersById, usersByName, tokens, chatsById),
                "send-message" => new SendMessageHandler(usersById, usersByName, tokens, chatsById),
                _ => null
            };

            if (handler == null)
            {
                Console.WriteLine($"[LOG] Неизвестная команда: {request.Type}");
                continue;
            }


            Response response = handler.Handle(request);

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
