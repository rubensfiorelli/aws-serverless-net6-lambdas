using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using Amazon.SQS;
using Aws.Application.Ports;
using Aws.Core.AdaptersAndPortsDriven;
using AWS.Application.Services;
using AWS.Core.DTOs.Input;
using AWS.Core.DTOs.Output;
using AWS.Core.Enums;
using AWS.Core.Helpers;
using System.Text.Json;


[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Booker;

public class Function
{
    private AmazonDynamoDBClient _DBClient { get; }
    private readonly IOrderService _orderService;

    public Function(IOrderService orderService)
    {
        _DBClient = new AmazonDynamoDBClient(RegionEndpoint.USEast1);
        _orderService = orderService;
    }

    public Function()
    {

    }

    public async Task FunctionHandler(SQSEvent evnt, ILambdaContext context)
    {
        if (evnt.Records.Count > 1)
            throw new InvalidOperationException("Only one message can be processed at a time");

    }

    private async Task ProcessMessageAsync(SQSEvent.SQSMessage message, ILambdaContext context)
    {
        context.Logger.LogLine($"Processing message {message.Body}");

        var order = JsonSerializer.Deserialize<OutputOrderDto>(message.Body);
        order.Status = StatusOrder.Started;

        foreach (var item in order.Products)
        {
            try
            {
                await DowloadStock(item.Title, item.Qty);
                order.Status.ToString();
                context.Logger.LogLine($"Download stock successfully {item.Title}");
            }
            catch (ConditionalCheckFailedException)
            {
                order.Status.ToString();
                break;
            }


        }

        if (order.Cancelled)
        {
            foreach (var item in order.Products)
            {
                if (item.Reserved)
                    await GiveBackStock(item.Title, item.Qty);
            }
            await order.SaveAsync();

        }
        else
        {
            var request = new InputOrderDto();
            var sQSClient = new AmazonSQSClient(RegionEndpoint.USEast1);
            var publisher = new SqsPublisher(sQSClient);
            var options = new JsonSerializerOptions { WriteIndented = true };

            await _orderService.Create(request);
            await publisher.PublisherAsync("order-notification", new NotificationMsg
            {
                Id = Guid.NewGuid().ToString()[..12].ToUpper(),
                Notification = JsonSerializer.Serialize(request.OrderNumber, options),
                Type = 1

            });
            await order.SaveAsync();

        }

        await Task.CompletedTask;
    }

    private async Task GiveBackStock(string? title, int qty)
    {
        var request = new UpdateItemRequest
        {
            TableName = "stock",
            ReturnValues = "NONE",
            Key = new Dictionary<string, AttributeValue>
            {
                {"title", new AttributeValue{S = title}}
            },
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                {":qtyOrder", new AttributeValue{N = qty.ToString() } }

            },
            UpdateExpression = "SET Qty = (Qty + :qtyOrder)"
        };

        await _DBClient.UpdateItemAsync(request);
    }

    private async Task DowloadStock(string? title, int qty)
    {
        var request = new UpdateItemRequest
        {
            TableName = "stock",
            ReturnValues = "NONE",
            Key = new Dictionary<string, AttributeValue>
            {
                {"title", new AttributeValue{S = title}}
            },
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                {":qtyOrder", new AttributeValue{N = qty.ToString() } }

            },
            ConditionExpression = "Qty >= :qtyOrder",
            UpdateExpression = "SET Qty = (Qty - :qtyOrder)"
        };

        await _DBClient.UpdateItemAsync(request);
    }
}