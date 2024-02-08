using AWS.Core.Entities;
using AWS.Core.Enums;
using System.Text.Json.Serialization;

namespace AWS.Core.DTOs.Output
{
    public record OutputOrderDto
    {
        public OutputOrderDto(string orderNumber,
                              string description,
                              int qty,
                              decimal totalPrice,
                              StatusOrder status)
        {
            OrderNumber = orderNumber;
            Description = description;
            Qty = qty;
            TotalPrice = totalPrice;
            Status = status;
        }

        [JsonPropertyName("orderNumber")]
        public string OrderNumber { get; private set; }

        [JsonPropertyName("description")]
        public string Description { get; private set; }

        [JsonPropertyName("qty")]
        public int Qty { get; private set; }

        [JsonPropertyName("status")]
        public StatusOrder Status { get; set; }

        [JsonPropertyName("totalPrice")]
        public decimal? TotalPrice { get; private set; }

        [JsonPropertyName("cancelled")]
        public bool Cancelled { get; set; }


        public IReadOnlyCollection<Product> Products { get; init; }


        public static implicit operator OutputOrderDto(Order entity)
            => new(
                entity.OrderNumber,
                entity.Description,
                entity.Qty,
                entity.TotalPrice,
                entity.Status);
              

    }
}
