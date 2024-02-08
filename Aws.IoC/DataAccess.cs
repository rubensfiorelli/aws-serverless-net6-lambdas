using Amazon;
using Amazon.DynamoDBv2;
using Aws.Application.Adapters;
using Aws.Application.Ports;
using Aws.Core.Ports;
using Aws.Data.Adapters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Aws.IoC
{
    public static class DataAccess
    {
        public static IServiceCollection AddData(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddRepositories()
                .AddServices();

            services.AddDefaultAWSOptions(configuration.GetAWSOptions());
            services.TryAddSingleton<IAmazonDynamoDB>(_ => new AmazonDynamoDBClient(RegionEndpoint.USEast1));


            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.TryAddScoped(typeof(IOrderRepository), typeof(OrderRepository));

            return services;
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {

            services.TryAddScoped(typeof(IOrderService), typeof(OrderService));

            return services;
        }
    }
}
