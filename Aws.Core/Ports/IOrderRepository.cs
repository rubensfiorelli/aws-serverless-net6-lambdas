using AWS.Core.Entities;

namespace Aws.Core.Ports
{
    public interface IOrderRepository
    {
        Task<bool> AddAsync(Order order);
    }
}
