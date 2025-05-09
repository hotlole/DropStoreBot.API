using DropStoreBot.Core.Entities;
using DropStoreBot.Core.Interfaces;
using DropStoreBot.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DropStoreBot.Infrastructure.Repositories;

public class ProductRepository(ApplicationDbContext context) : IProductRepository
{
    public async Task<IEnumerable<Product>> GetAllAsync() =>
        await context.Products
            .Include(p => p.Owner)
            .ToListAsync();

    public async Task<Product?> GetByIdAsync(Guid id) =>
        await context.Products
            .Include(p => p.Owner)
            .FirstOrDefaultAsync(p => p.Id == id);

    public async Task AddAsync(Product product) => await context.Products.AddAsync(product);

    public Task UpdateAsync(Product product)
    {
        context.Products.Update(product);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(Guid id)
    {
        var product = await context.Products.FindAsync(id);
        if (product is not null) context.Products.Remove(product);
    }

    public Task SaveChangesAsync() => context.SaveChangesAsync();
}
