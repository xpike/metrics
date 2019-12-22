using XPike.IoC;

namespace XPike.Metrics
{
    public class Package
        : IDependencyPackage
    {
        public void RegisterPackage(IDependencyCollection dependencyCollection)
        {
            dependencyCollection.LoadPackage(new XPike.Settings.Package());

            dependencyCollection.RegisterSingleton<IDefaultMetricsService, DefaultMetricsService>();
            dependencyCollection.RegisterSingleton<IBufferedMetricsService, BufferedMetricsService>();

            dependencyCollection.RegisterSingleton<IMetricsService>(container =>
                container.ResolveDependency<IDefaultMetricsService>());
        }
    }
}