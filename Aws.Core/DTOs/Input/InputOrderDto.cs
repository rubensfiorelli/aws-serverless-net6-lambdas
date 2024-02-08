using AWS.Core.Entities;
using System.Text.Json.Serialization;

namespace AWS.Core.DTOs.Input
{
    public record InputOrderDto
    {

        [JsonPropertyName("sk")]
        public string? Sk => OrderNumber;

        [JsonPropertyName("orderNumber")]
        public string? OrderNumber { get; init; } = Guid.NewGuid().ToString()[..11].ToUpper();

        [JsonPropertyName("qty")]
        public int Qty { get; init; }

        [JsonPropertyName("createdAt")]
        public DateTime? CreatedAt { get; init; } = DateTime.UtcNow;

        [JsonPropertyName("products")]
        public List<InputProductDto> Products { get; init; } = new();

        [JsonPropertyName("reserved")]
        public bool Reserved { get; set; } = false;

        [JsonPropertyName("canceled")]
        public bool Cancelled { get; set; } = false;


        public Order ToEntity()
            => new()
            {
                OrderNumber = OrderNumber,
                Qty = Qty

            };


    }
}
