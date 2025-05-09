using DropStoreBot.Core.Entities;
using DropStoreBot.Core.Interfaces;
using DropStoreBot.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DropStoreBot.Infrastructure.Repositories;

public class UserRepository(ApplicationDbContext context) : IUserRepository
{
    public async Task<User?> GetByTelegramIdAsync(long telegramId) =>
        await context.Users
            .Include(u => u.Transactions)
            .FirstOrDefaultAsync(u => u.TelegramId == telegramId);

    public async Task AddAsync(User user) => await context.Users.AddAsync(user);

    public Task UpdateAsync(User user)
    {
        context.Users.Update(user);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync() => context.SaveChangesAsync();
}
