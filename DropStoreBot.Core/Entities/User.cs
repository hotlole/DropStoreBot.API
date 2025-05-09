using System.Transactions;

namespace DropStoreBot.Core.Entities
{
    public class User
    {
        public long TelegramId { get; set; } // Telegram user ID
        public string? Username { get; set; }
        public decimal Balance { get; set; }
        public DateTime RegisteredAt { get; set; }
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
