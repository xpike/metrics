using XPike.IoC;

namespace XPike.Metrics.Azure
{
    public class Package
        : IDependencyPackage
    {
        public void RegisterPackage(IDependencyCollection dependencyCollection)
        {
            dependencyCollection.LoadPackage(new XPike.Metrics.Package());

            dependencyCollection.RegisterSingleton<IApplicationInsightsMetricsProvider, ApplicationInsightsMetricsProvider>();

            dependencyCollection.AddSingletonToCollection<IMetricsProvider, IApplicationInsightsMetricsProvider>(container =>
                container.ResolveDependency<IApplicationInsightsMetricsProvider>());
        }
    }
}