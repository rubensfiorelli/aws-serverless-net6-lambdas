using Aws.Application.Ports;
using Aws.Core.Ports;
using AWS.Core.DTOs.Input;

namespace Aws.Application.Adapters
{
    public class ProductDriver : IProductDriver
    {
        private readonly IProductRepository _repository;
        public ProductDriver(IProductRepository repository) => _repository = repository;
       
        public async Task Add(InputProductDto model)
        {
            var addEntity = model.ToEntity();

            await _repository.AddAsync(addEntity);

        }
    }
    
}
