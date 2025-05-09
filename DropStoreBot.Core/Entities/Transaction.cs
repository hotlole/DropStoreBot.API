namespace DropStoreBot.Core.Entities;

public class Transaction
{
    public Guid Id { get; set; }
    public long UserTelegramId { get; set; }
    public decimal Amount { get; set; }
    public DateTime Timestamp { get; set; }
    public string Type { get; set; } = default!; // "deposit", "purchase" и т.д.    
    public User? User { get; set; }
}
