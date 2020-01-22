using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using XPike.IoC;
using XPike.IoC.Microsoft;

namespace XPike.Metrics.Microsoft
{
    public static class IWebHostBuilderExtensions
    {
        public static IWebHostBuilder AddXPikeMetrics(this IWebHostBuilder builder, Action<IDependencyCollection> metricsSetup = null) =>
            builder.ConfigureServices((context, collection) =>
            {
                collection.AddXPikeMetrics();
                collection.AddSingleton<IStartupFilter, StartupFilter>();

                metricsSetup?.Invoke(new MicrosoftDependencyCollection(collection, false, false));
            });

        public static IWebHostBuilder AddXPikeConsoleMetrics(this IWebHostBuilder builder) =>
            builder.ConfigureServices((context, collection) =>
            {
                collection.AddXPikeConsoleMetrics();
                collection.AddSingleton<IStartupFilter, StartupFilter>();
            });

        public static IWebHostBuilder UseXPikeConsoleMetrics(this IWebHostBuilder builder) =>
            builder.ConfigureServices((context, collection) =>
            {
                collection.UseXPikeConsoleMetrics();
                collection.AddSingleton<IStartupFilter, StartupFilter>();
            });

    }
}