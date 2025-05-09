using DropStoreBot.Application.Services;
using DropStoreBot.Core.Interfaces;
using DropStoreBot.Core.Interfaces.Services;
using DropStoreBot.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace DropStoreBot.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IProductService, ProductService>();
        return services;
    }
}
