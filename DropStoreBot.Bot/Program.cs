using DropStoreBot.Application.Services;
using DropStoreBot.Core.Interfaces;
using DropStoreBot.Core.Interfaces.Services;
using DropStoreBot.Infrastructure.Data;
using DropStoreBot.Infrastructure.Repositories;
using DropStoreBot.Telegram.BotHandlers;
using Microsoft.EntityFrameworkCore;
using System;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

var builder = Host.CreateApplicationBuilder(args);
var services = builder.Services;

// Подключаем контекст и зависимости
services.AddDbContext<AppDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

services.AddScoped<IUserRepository, UserRepository>();
services.AddScoped<ITransactionRepository, TransactionRepository>();
services.AddScoped<IUserService, UserService>();

services.AddSingleton<UpdateHandler>();

var app = builder.Build();

var botToken = builder.Configuration["TelegramBotToken"]
               ?? throw new InvalidOperationException("Bot token not set");

var botClient = new TelegramBotClient(botToken);
var updateHandler = app.Services.GetRequiredService<UpdateHandler>();

using var cts = new CancellationTokenSource();

botClient.StartReceiving(
    updateHandler: new DefaultUpdateHandler(
        (bot, update, token) => updateHandler.HandleUpdateAsync(bot, update, token),
        (_, _, _) => Task.CompletedTask
    ),
    cancellationToken: cts.Token
);

Console.WriteLine("Bot started. Press any key to stop.");
Console.ReadKey();
cts.Cancel();
