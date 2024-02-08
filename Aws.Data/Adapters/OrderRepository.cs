using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Aws.Core.Ports;
using AWS.Core.Entities;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Aws.Data.Adapters
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IAmazonDynamoDB _dynamoDB;
        private readonly IOptions<DatabaseSettings> _databaseSettings;
        public OrderRepository(IAmazonDynamoDB dynamoDB, IOptions<DatabaseSettings> databaseSettings)
        {
            _dynamoDB = dynamoDB;
            _databaseSettings = databaseSettings;
        }

        public async Task<bool> AddAsync(Order order)
        {
            var customerAsJson = JsonSerializer.Serialize(order);
            var itemAsDocument = Document.FromJson(customerAsJson);
            var itemAttributes = itemAsDocument.ToAttributeMap();

            order.CreatedAt = DateTime.UtcNow;
            order.UpdatedAt = DateTime.UtcNow;

            var createItemRequest = new PutItemRequest
            {
                TableName = _databaseSettings.Value.TableName,
                Item = itemAttributes
            };

            var response = await _dynamoDB.PutItemAsync(createItemRequest);

            return response.HttpStatusCode == System.Net.HttpStatusCode.OK;

        }
    }
}
