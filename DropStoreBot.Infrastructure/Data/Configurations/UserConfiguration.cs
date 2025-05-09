using DropStoreBot.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DropStoreBot.Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.TelegramId);

        builder.Property(x => x.Username).HasMaxLength(64);
        builder.Property(x => x.Balance).HasColumnType("decimal(18,2)");

        builder.HasMany(x => x.Transactions)
               .WithOne(x => x.User)
               .HasForeignKey(x => x.UserTelegramId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
