using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using DropStoreBot.Core.Interfaces.Services;

namespace DropStoreBot.Telegram.BotHandlers;

public class UpdateHandler(IUserService userService)
{
    public async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
    {
        if (update.Type != UpdateType.Message || update.Message?.Text == null)
            return;

        var message = update.Message;
        var chatId = message.Chat.Id;
        var text = message.Text.Trim();

        // Реакция на команду /start
        if (text.StartsWith("/start"))
        {
            var user = await userService.GetOrCreateUserAsync(chatId, message.From?.Username);
            await bot.SendTextMessageAsync(chatId, $"Добро пожаловать, {user.Username ?? "друг"}!\nВаш баланс: {user.Balance}₽", cancellationToken: cancellationToken);
            return;
        }

        // Реакция на команду /balance
        if (text.StartsWith("/balance"))
        {
            var balance = await userService.GetBalanceAsync(chatId);
            await bot.SendTextMessageAsync(chatId, $"Ваш текущий баланс: {balance}₽", cancellationToken: cancellationToken);
            return;
        }
        // Реакция на команду /shop
        if (text.StartsWith("/shop"))
        {
            var products = await productService.GetAllProductsAsync(); // Мы создадим service для работы с продуктами
            var productList = products.Select(p => $"{p.Name} - {p.Price}₽").ToList();

            if (productList.Any())
            {
                var productMessage = string.Join("\n", productList);
                await bot.SendTextMessageAsync(chatId, $"Доступные товары:\n{productMessage}", cancellationToken: cancellationToken);
            }
            else
            {
                await bot.SendTextMessageAsync(chatId, "Товары не найдены.", cancellationToken: cancellationToken);
            }
            return;
        }

        // Тестовая реакция
        await bot.SendTextMessageAsync(chatId, $"Неизвестная команда: {text}", cancellationToken: cancellationToken);
    }
}
