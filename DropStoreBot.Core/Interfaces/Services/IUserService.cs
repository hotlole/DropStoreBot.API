using DropStoreBot.Core.Entities;

namespace DropStoreBot.Core.Interfaces.Services;

public interface IUserService
{
    Task<User> GetOrCreateUserAsync(long telegramId, string? username);
    Task<decimal> GetBalanceAsync(long telegramId);
    Task AddBalanceAsync(long telegramId, decimal amount, string reason = "deposit");
}
