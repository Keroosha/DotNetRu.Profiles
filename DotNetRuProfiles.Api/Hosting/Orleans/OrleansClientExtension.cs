using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;

namespace DotNetRuProfilesApi.Hosting.Orleans
{
    public static class OrleansClientExtension
    {
        /// <summary>
        /// Registering IClusterClient in DI
        /// </summary>
        /// <param name="services"></param>
        /// <param name="clusterId">Cluster ID of silo</param>
        /// <param name="serviceId">Service ID of silo</param>
        public static void AddOrleansClient(this IServiceCollection services, string clusterId, string serviceId)
        {
            var client = new ClientBuilder()
                .UseLocalhostClustering()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = clusterId;
                    options.ServiceId = serviceId;
                })
                .ConfigureLogging(logging => logging.AddConsole())
                .Build();
            client.Connect().GetAwaiter().GetResult();

            services.AddSingleton(client);
        }
    }
}