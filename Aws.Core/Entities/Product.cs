using Amazon.DynamoDBv2.DataModel;
using AWS.Core.Common;
using System.Text.Json.Serialization;

namespace AWS.Core.Entities
{
    [DynamoDBTable("products")]
    public class Product : BaseEntity
    {

        [JsonPropertyName("pk")]
        public string? Pk => Id;

        [JsonPropertyName("sk")]
        public string? Sk => Barcode;

        [JsonPropertyName("barcode")]
        public string? Barcode { get; init; }

        [JsonPropertyName("title")]
        public string? Title { get; init; }

        [JsonPropertyName("price")]
        public decimal Price { get; init; }

        [JsonPropertyName("qty")]
        public int Qty { get; init; }

        [JsonPropertyName("reseverd")]
        public bool Reserved { get; init; }
    }
}