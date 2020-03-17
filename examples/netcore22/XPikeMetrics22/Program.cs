using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using XPike.Configuration.Microsoft.AspNetCore;
using XPike.IoC.Microsoft.AspNetCore;
using XPike.Logging.Microsoft.AspNetCore;
using XPike.Metrics;
using XPike.Metrics.DataDog;
using XPike.Metrics.Microsoft;

namespace XPikeMetrics22
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .AddXPikeMetrics(collection =>
                    collection.AddXPikeDataDogMetrics()
                        .AddXPikeConsoleMetrics())
                .UseXPikeConfiguration()
                .UseXPikeLogging()
                .UseStartup<Startup>()
                .AddXPikeDependencyInjection();
    }
}