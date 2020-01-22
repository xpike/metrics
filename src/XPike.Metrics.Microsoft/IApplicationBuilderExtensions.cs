using Microsoft.AspNetCore.Builder;

namespace XPike.Metrics.Microsoft
{
    public static class IApplicationBuilderExtensions
    {
        public static IApplicationBuilder AddXPikeRequestMetricsMiddleware(this IApplicationBuilder builder) =>
            builder.UseMiddleware<RequestMetricsMiddleware>();
    }
}