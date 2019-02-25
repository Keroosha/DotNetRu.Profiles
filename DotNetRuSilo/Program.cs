using System;
using System.Threading.Tasks;
using DotNetRuGrains.Profile;
using DotNetRuGrains.Wiki;
using DotNetRuProfiles.Markdown;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;

namespace DotNetRuSilo
{
    public class Program
    {
        public static int Main(string[] args)
        {
            return RunMainAsync().Result;
        }

        private static async Task<int> RunMainAsync()
        {
            try
            {
                var host = await StartSilo();
                Console.WriteLine("\n\n Press Enter to terminate...\n\n");
                Console.ReadLine();

                await host.StopAsync();

                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 1;
            }
        }

        private static async Task<ISiloHost> StartSilo()
        {
            // define the cluster configuration
            var builder = new SiloHostBuilder()
                .UseLocalhostClustering()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "Main";
                    options.ServiceId = "DotNetRuProfiles";
                })
                .ConfigureApplicationParts(parts =>
                {
                    parts.AddApplicationPart(typeof(WikiFetcherGrain).Assembly).WithReferences();
                    parts.AddApplicationPart(typeof(ProfileGrain).Assembly).WithReferences();
                })
                .ConfigureServices(services =>
                {
                    services.AddSingleton<IMarkdownParser>(new MarkdownParser());
                })
                .ConfigureLogging(logging => logging.AddConsole());

            var host = builder.Build();
            await host.StartAsync();
            return host;
        }
    }
}