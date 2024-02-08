using AWS.Core.Entities;

namespace Aws.Core.Ports
{
    public interface IProductRepository
    {
        Task AddAsync(Product product);
        Task<IReadOnlyList<Product>> GetAllAsync();
    }
}
