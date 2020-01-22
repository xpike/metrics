using XPike.IoC;
using XPike.Metrics.Console;

namespace XPike.Metrics
{
    public static class IDependencyCollectionExtensions
    {
        public static IDependencyCollection AddXPikeMetrics(this IDependencyCollection collection) =>
            collection.LoadPackage(new XPike.Metrics.Package());

        public static IDependencyCollection AddXPikeConsoleMetrics(this IDependencyCollection collection)
        {
            collection.AddXPikeMetrics()
                .AddSingletonToCollection<IMetricsProvider, IConsoleMetricsProvider>(container =>
                container.ResolveDependency<IConsoleMetricsProvider>());

            return collection;
        }

        public static IDependencyCollection UseXPikeConsoleMetrics(this IDependencyCollection collection)
        {
            collection.ResetCollection<IMetricsProvider>();
            return collection.AddXPikeConsoleMetrics();
        }
    }
}