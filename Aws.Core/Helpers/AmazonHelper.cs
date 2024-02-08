using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using AWS.Core.DTOs.Output;
using AWS.Core.Entities;

namespace AWS.Core.Helpers
{
    public static class AmazonHelper
    {
        public static T ToObject<T>(this Dictionary<string, AttributeValue> keyValues)
        {
            var document = Document.FromAttributeMap(keyValues);
            var clientDynamoDB = new AmazonDynamoDBClient(RegionEndpoint.USEast1);
            var context = new DynamoDBContext(clientDynamoDB);

            return context.FromDocument<T>(document);

        }

        public static async Task SaveAsync(this Order order)
        {
            var client = new AmazonDynamoDBClient(RegionEndpoint.USEast1);
            var context = new DynamoDBContext(client);
            await context.SaveAsync(order);
        }

        public static async Task SaveAsync(this OutputOrderDto order)
        {
            var client = new AmazonDynamoDBClient(RegionEndpoint.USEast1);
            var context = new DynamoDBContext(client);
            await context.SaveAsync(order);

        }
    }
}
