using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.Core;
using Amazon.Lambda.DynamoDBEvents;
using Aws.Core.DTOs.Output;
using Aws.Data;
using AWS.Core.DTOs.Input;
using AWS.Core.Entities;
using AWS.Core.Enums;
using AWS.Core.Helpers;
using Microsoft.Extensions.Options;
using System.Text.Json;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Collector;

public class Function
{
    private static IAmazonDynamoDB _amazonDynamoDB;
    private static IOptions<DatabaseSettings> _databaseSettings;
    public static async Task FunctionHandler(DynamoDBEvent dynamoDBEvent, ILambdaContext context)
    {
        context.Logger.LogInformation($"Beginning to process {dynamoDBEvent.Records.Count} records...");

        foreach (var record in dynamoDBEvent.Records)
        {
           
            if (record.EventName == OperationType.INSERT)
            {
                InputOrderDto inputOrderDto = new();

                var orderAsJson = JsonSerializer.Serialize(inputOrderDto);
                var itemAsDocument = Document.FromJson(orderAsJson);
                var itemAsAttributes = itemAsDocument.ToAttributeMap();

                var createItemRequest = new PutItemRequest
                {
                    TableName = _databaseSettings.Value.TableName,
                    Item = itemAsAttributes
                };

                var inserted = record.Dynamodb.NewImage["pk"].S;
                var response = await _amazonDynamoDB.PutItemAsync(createItemRequest);

            }
           
        }

        context.Logger.LogInformation("Stream processing complete.");
    }

    //private static async Task<OutputProductDto> GetProductStockAsync(string id, string barcode)
    //{
    //    var request = new GetItemRequest
    //    {
    //        TableName = _databaseSettings.Value.TableName,
    //        Key = new Dictionary<string, AttributeValue>
    //        {
    //            { "pk", new AttributeValue{S = id.ToString()} },
    //            { "sk", new AttributeValue{S = barcode.ToString()} }
    //        }
    //    };

    //    var response = await _amazonDynamoDB.GetItemAsync(request);
    //    if (response.Item.Count == 0)
    //        return null;

    //    var itemAsDocument = Document.FromAttributeMap(response.Item);
    //    return JsonSerializer.Deserialize<OutputProductDto>(itemAsDocument.ToJson());        

    //}
    
}