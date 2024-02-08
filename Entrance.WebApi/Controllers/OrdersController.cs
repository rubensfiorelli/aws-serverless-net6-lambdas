using Amazon;
using Amazon.SQS;
using Aws.Application.Ports;
using Aws.Core.AdaptersAndPortsDriven;
using AWS.Application.Services;
using AWS.Core.DTOs.Input;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Entrance.Controllers
{
    [ApiController]
    [Route("")]
    public class OrdersController : ControllerBase
    {

        private readonly ILogger<OrdersController> _logger;
        private readonly IOrderService _service;


        public OrdersController(ILogger<OrdersController> logger, IOrderService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpPost("orders")]
        public async Task<IActionResult> CreateOrder(InputOrderDto request)
        {
            var sQSClient = new AmazonSQSClient(RegionEndpoint.USEast1);
            var publisher = new SqsPublisher(sQSClient);
            var options = new JsonSerializerOptions { WriteIndented = true };

            await _service.Create(request);


            if (request is not null)
            {
                await publisher.PublisherAsync("order-notification", new NotificationMsg
                {
                    Id = Guid.NewGuid().ToString()[..12].ToUpper(),
                    Notification = JsonSerializer.Serialize(request.OrderNumber, options),
                    Type = 1

                });
            }

            Console.WriteLine($" Request successfully created in DynamoDB");

            _logger.LogInformation("Request successfully created in Queue!");

            return CreatedAtAction(nameof(CreateOrder), new { request.OrderNumber }, request);
        }


    }
}
