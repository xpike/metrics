using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using XPike.IoC;
using XPike.IoC.Microsoft;

namespace XPike.Metrics.Microsoft
{
    public static class IHostBuilderExtensions
    {
        public static IHostBuilder AddXPikeMetrics(this IHostBuilder builder, Action<IDependencyCollection> metricsSetup = null) =>
            builder.ConfigureServices((context, collection) =>
            {
                collection.AddXPikeMetrics();
                collection.AddSingleton<IStartupFilter, StartupFilter>();

                metricsSetup?.Invoke(new MicrosoftDependencyCollection(collection, false, false));
            });

        public static IHostBuilder AddXPikeConsoleMetrics(this IHostBuilder builder) =>
            builder.ConfigureServices((context, collection) =>
            {
                collection.AddXPikeConsoleMetrics();
                collection.AddSingleton<IStartupFilter, StartupFilter>();
            });

        public static IHostBuilder UseXPikeConsoleMetrics(this IHostBuilder builder) =>
            builder.ConfigureServices((context, collection) =>
            {
                collection.UseXPikeConsoleMetrics();
                collection.AddSingleton<IStartupFilter, StartupFilter>();
            });

    }
}