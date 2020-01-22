using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace XPike.Metrics.Microsoft
{
    public class StartupFilter
        : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next) =>
            builder =>
            {
                builder.UseMiddleware<RequestMetricsMiddleware>();

                next(builder);
            };
    }
}