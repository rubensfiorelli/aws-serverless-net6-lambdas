using Amazon.DynamoDBv2.DataModel;
using AWS.Core.Common;
using AWS.Core.Enums;
using System.Text.Json.Serialization;

namespace AWS.Core.Entities
{

    [DynamoDBTable("orders")]
    public sealed class Order : BaseEntity
    {
        private List<OrderLine> _orderLines => new();


        [JsonPropertyName("pk")]
        public string? Pk => Id;

        [JsonPropertyName("sk")]
        public string? Sk => OrderNumber;

        [JsonPropertyName("orderNumber")]
        public string? OrderNumber { get; init; }

        [JsonPropertyName("description")]
        public string? Description { get; init; }

        [JsonPropertyName("qty")]
        public int Qty { get; init; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        [DynamoDBProperty("status")]
        public StatusOrder Status { get; set; } = StatusOrder.Started;

        [JsonPropertyName("totalPrice")]
        public decimal TotalPrice { get; init; }

        [JsonPropertyName("reserved")]
        public bool Reserved { get; set; }

        public IReadOnlyCollection<OrderLine> Items => _orderLines;


        public void SetupOrders(List<Product> products)
        {
            foreach (var item in products)
            {
                var subTotal = item.Price * item.Qty;

                Order order = new()
                { TotalPrice = subTotal };

                _orderLines.Add(new OrderLine(item.Title, Qty, subTotal));
            }

        }
    }


}
