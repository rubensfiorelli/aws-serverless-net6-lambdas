using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Lambda.Core;
using Amazon.Lambda.DynamoDBEvents;
using StackExchange.Redis;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Redis;

public class Function
{
    private static IConnectionMultiplexer _connectionMultiplexer;

    private static readonly string _redisConnectionString =
        Environment.GetEnvironmentVariable("RedisConnectionString");

    public static string RedisConnectionString1 => _redisConnectionString;

    public static async Task FunctionHandler(DynamoDBEvent dynamoDbEvent, ILambdaContext context)
    {
        _connectionMultiplexer = await ConnectionMultiplexer.ConnectAsync(_redisConnectionString);

        context.Logger.LogInformation($"Beginning to process {dynamoDbEvent.Records.Count} records...");

        var database = _connectionMultiplexer.GetDatabase();
        foreach (var record in dynamoDbEvent.Records)
        {
            if (record.EventName == OperationType.REMOVE)
            {
                var deletedPk = record.Dynamodb.OldImage["pk"].S;
                await database.KeyDeleteAsync(deletedPk);
                continue;
            }

            var recordAsDocument = Document.FromAttributeMap(record.Dynamodb.NewImage);
            var json = recordAsDocument.ToJson();
            var pk = record.Dynamodb.NewImage["pk"].S;
            await database.StringSetAsync(pk, json);

        }

        context.Logger.LogInformation("Stream processing complete.");
    }
}