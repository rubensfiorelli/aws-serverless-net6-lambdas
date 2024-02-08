using AWS.Core.DTOs.Input;

namespace Aws.Application.Ports
{
    public interface IProductDriver
    {
        Task Add(InputProductDto model);
    }
}
