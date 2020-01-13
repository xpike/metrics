using XPike.IoC;
using XPike.Metrics.Console;

namespace XPike.Metrics
{
    public class Package
        : IDependencyPackage
    {
        public void RegisterPackage(IDependencyCollection dependencyCollection)
        {
            dependencyCollection.LoadPackage(new XPike.Configuration.Package());

            dependencyCollection.RegisterSingleton<IDefaultMetricsService, DefaultMetricsService>();
            dependencyCollection.RegisterSingleton<IBufferedMetricsService, BufferedMetricsService>();
            dependencyCollection.RegisterSingleton<IConsoleMetricsProvider, ConsoleMetricsProvider>();
            dependencyCollection.RegisterSingleton<IMetricsContextProvider, MetricsContextProvider>();
            dependencyCollection.RegisterSingleton<IMetricsContextAccessor, MetricsContextAccessor>();

            dependencyCollection.RegisterSingleton<IMetricsService>(container =>
                container.ResolveDependency<IDefaultMetricsService>());
        }
    }
}