using DropStoreBot.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DropStoreBot.Infrastructure.Data.Configurations;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Type).HasMaxLength(32).IsRequired();
        builder.Property(t => t.Amount).HasColumnType("decimal(18,2)");

        builder.HasOne(t => t.User)
               .WithMany(u => u.Transactions)
               .HasForeignKey(t => t.UserTelegramId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
