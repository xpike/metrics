using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using XPike.Configuration;

namespace XPike.Metrics.Microsoft
{
    public class RequestMetricsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IMetricsService _metricsService;
        private readonly IConfig<MetricsConfig> _config;

        public RequestMetricsMiddleware(RequestDelegate next, IMetricsService metricsService, IConfig<MetricsConfig> config)
        {
            _next = next;
            _metricsService = metricsService;
            _config = config;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!_config.CurrentValue.EnableRequestMetrics)
            {
                await _next(context);
                return;
            }

            using (var tracker = _metricsService.StartTracker("request", tags:
                new[]
                {
                    $"path:{context.Request.Path}"
                }))
                try
                {
                    await _next(context);

                    tracker.Tags.Add($"statusCode:{context.Response.StatusCode}");
                    tracker.Tags.Add($"statusName:{((HttpStatusCode) context.Response.StatusCode)}");

                    if (context.Response.StatusCode >= 200 && context.Response.StatusCode <= 300)
                    {
                        tracker.SetSuccess();
                    }
                    else if (context.Response.StatusCode >= 500)
                    {
                        tracker.SetFailure();
                    }
                    else
                    {
                        tracker.SetFailure("warning");
                    }
                }
                catch (Exception ex)
                {
                    tracker.SetFailure();
                    tracker.Tags.Add($"exception:{ex.GetType()}");

                    throw;
                }
        }
    }
}