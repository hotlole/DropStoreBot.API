using DropStoreBot.Core.Entities;
using DropStoreBot.Core.Interfaces;
using DropStoreBot.Core.Interfaces.Services;

namespace DropStoreBot.Application.Services;

public class UserService(IUserRepository userRepo, ITransactionRepository txRepo) : IUserService
{
    public async Task<User> GetOrCreateUserAsync(long telegramId, string? username)
    {
        var user = await userRepo.GetByTelegramIdAsync(telegramId);
        if (user == null)
        {
            user = new User
            {
                TelegramId = telegramId,
                Username = username,
                Balance = 0,
                RegisteredAt = DateTime.UtcNow
            };
            await userRepo.AddAsync(user);
            await userRepo.SaveChangesAsync();
        }
        return user;
    }

    public async Task<decimal> GetBalanceAsync(long telegramId)
    {
        var user = await userRepo.GetByTelegramIdAsync(telegramId)
                   ?? throw new InvalidOperationException("Пользователь не найден");
        return user.Balance;
    }

    public async Task AddBalanceAsync(long telegramId, decimal amount, string reason = "deposit")
    {
        var user = await userRepo.GetByTelegramIdAsync(telegramId)
                   ?? throw new InvalidOperationException("Пользователь не найден");

        user.Balance += amount;
        await userRepo.UpdateAsync(user);

        var tx = new Transaction
        {
            Id = Guid.NewGuid(),
            UserTelegramId = telegramId,
            Amount = amount,
            Timestamp = DateTime.UtcNow,
            Type = reason,
        };
        await txRepo.AddAsync(tx);

        await userRepo.SaveChangesAsync();
        await txRepo.SaveChangesAsync();
    }
}
