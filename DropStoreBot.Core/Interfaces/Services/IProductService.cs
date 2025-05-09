using DropStoreBot.Core.Entities;

namespace DropStoreBot.Core.Interfaces.Services;

public interface IProductService
{
    Task<IEnumerable<Product>> GetAllProductsAsync();
    Task<Product?> GetProductByIdAsync(Guid id);
}
