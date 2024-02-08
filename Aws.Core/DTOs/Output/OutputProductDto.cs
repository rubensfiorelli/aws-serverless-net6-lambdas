using AWS.Core.Entities;

namespace Aws.Core.DTOs.Output
{
    public record OutputProductDto
    {
        public OutputProductDto(string id, string title, decimal price, int qty, bool reserved)
        {
            Id = id;
            Title = title;
            Price = price;
            Qty = qty;
            Reserved = reserved;
        }

        public string Id { get; init; }
        public string Title { get; private set; }
        public decimal Price { get; private set; }
        public int Qty { get; private set; }
        public bool Reserved { get; private set; }


        public static implicit operator OutputProductDto(Product entity)
            => new(
                entity.Id,
                entity.Title,
                entity.Price,
                entity.Qty,
                entity.Reserved);
        
            
    }
}
