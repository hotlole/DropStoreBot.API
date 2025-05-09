using DropStoreBot.Core.Entities;

namespace DropStoreBot.Core.Interfaces;

public interface ITransactionRepository
{
    Task AddAsync(Transaction tx);
    Task<IEnumerable<Transaction>> GetByUserAsync(long telegramId);
    Task SaveChangesAsync();
}
