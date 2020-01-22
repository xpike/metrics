using Microsoft.Extensions.DependencyInjection;
using XPike.IoC.Microsoft;

namespace XPike.Metrics.Microsoft
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddXPikeMetrics(this IServiceCollection collection)
        {
            new MicrosoftDependencyCollection(collection, false, false).AddXPikeMetrics();
            return collection;
        }
        
        public static IServiceCollection AddXPikeConsoleMetrics(this IServiceCollection collection)
        {
            new MicrosoftDependencyCollection(collection, false, false).AddXPikeConsoleMetrics();
            return collection;
        }

        public static IServiceCollection UseXPikeConsoleMetrics(this IServiceCollection collection)
        {
            new MicrosoftDependencyCollection(collection, false, false).UseXPikeConsoleMetrics();
            return collection;
        }
    }
}