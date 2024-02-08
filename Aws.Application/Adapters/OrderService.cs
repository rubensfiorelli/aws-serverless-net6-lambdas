using Aws.Application.Ports;
using Aws.Core.Ports;
using AWS.Core.DTOs.Input;

namespace Aws.Application.Adapters
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        public OrderService(IOrderRepository orderRepository) => _orderRepository = orderRepository;
      

        public async Task<string> Create(InputOrderDto model)
        {
            var order = model.ToEntity();
            var addEntity = model.Products
                .Select(p => p.ToEntity()).ToList();

            order.SetupOrders(addEntity);

            await _orderRepository.AddAsync(order);

            return order.Status.ToString();

        }
    }
}
