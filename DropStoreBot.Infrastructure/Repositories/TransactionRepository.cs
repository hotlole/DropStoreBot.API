using DropStoreBot.Core.Entities;
using DropStoreBot.Core.Interfaces;
using DropStoreBot.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DropStoreBot.Infrastructure.Repositories;

public class TransactionRepository(ApplicationDbContext context) : ITransactionRepository
{
    public async Task AddAsync(Transaction tx) => await context.Transactions.AddAsync(tx);

    public async Task<IEnumerable<Transaction>> GetByUserAsync(long telegramId) =>
        await context.Transactions
            .Where(t => t.UserTelegramId == telegramId)
            .OrderByDescending(t => t.Timestamp)
            .ToListAsync();

    public Task SaveChangesAsync() => context.SaveChangesAsync();
}
