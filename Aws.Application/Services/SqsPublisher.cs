using Amazon.SQS;
using Amazon.SQS.Model;
using Aws.Core.AdaptersAndPortsDriven;
using AWS.Core.Enums;
using System.Text.Json;

namespace AWS.Application.Services
{
    public class SqsPublisher
    {
        private readonly IAmazonSQS _sqs;
        public SqsPublisher(IAmazonSQS sqs) => _sqs = sqs;


        public async Task PublisherAsync<T>(string queueName, T message) where T : IMessage
        {
            try
            {
                var response = await _sqs.GetQueueUrlAsync(queueName);
                if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {

                    var request = new SendMessageRequest
                    {
                        QueueUrl = response.QueueUrl,
                        MessageBody = JsonSerializer.Serialize(message),
                        MessageAttributes = new Dictionary<string, MessageAttributeValue>
                        {
                            {
                                nameof(IMessage.MessageTypeName),
                                new MessageAttributeValue
                                {
                                    StringValue = message.MessageTypeName,
                                    DataType = "String"
                                }
                            },
                            {
                                "timestamp",
                                new MessageAttributeValue
                                {
                                    StringValue = DateTime.UtcNow.ToString(),
                                    DataType = "String"

                                }
                            },
                            {
                                "status",
                                new MessageAttributeValue
                                {
                                    StringValue = StatusOrder.Started.ToString(),
                                    DataType = "String"
                                }
                            }
                        }
                    };

                    await _sqs.SendMessageAsync(request);
                }

            }
            catch (QueueDoesNotExistException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine($"The queue {queueName} was not found.");
            }
        }
    }
}
