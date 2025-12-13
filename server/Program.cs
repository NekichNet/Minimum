using server.Services;
﻿using DotNetEnv;
using server.Data;
using Microsoft.EntityFrameworkCore;

namespace server;

internal class Program
{
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
    }
}
