using AWS.Core.Entities;
using System.Text.Json.Serialization;

namespace AWS.Core.DTOs.Input
{
   
    public record InputProductDto
    {

        [JsonPropertyName("sk")]
        public string? Sk => Barcode;

        [JsonPropertyName("orderNumber")]
        public string? Barcode { get; init; } = Guid.NewGuid().ToString()[..15].Replace("-", "");

        [JsonPropertyName("title")]
        public string? Title { get; init; }

        [JsonPropertyName("price")]
        public decimal Price { get; init; }

        [JsonPropertyName("qty")]
        public int Qty { get; init; }

        [JsonPropertyName("reserved")]
        public bool Reserved { get; init; } = false;

        public Product ToEntity()
            => new()
            {
                Barcode = Barcode,
                Title = Title,
                Price = Price,
                Qty = Qty
            };
    }
}