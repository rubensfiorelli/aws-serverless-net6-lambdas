using AWS.Core.DTOs.Input;

namespace Aws.Application.Ports
{
    public interface IOrderService
    {
        Task<string> Create(InputOrderDto model);
    }
}
