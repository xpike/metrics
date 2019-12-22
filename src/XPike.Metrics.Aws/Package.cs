using XPike.IoC;

namespace XPike.Metrics.Aws
{
    public class Package
        : IDependencyPackage
    {
        public void RegisterPackage(IDependencyCollection dependencyCollection)
        {
            dependencyCollection.LoadPackage(new XPike.Metrics.Package());

            dependencyCollection.RegisterSingleton<ICloudwatchMetricsProvider, CloudwatchMetricsProvider>();

            dependencyCollection.AddSingletonToCollection<IMetricsProvider, ICloudwatchMetricsProvider>(container =>
                container.ResolveDependency<ICloudwatchMetricsProvider>());
        }
    }
}