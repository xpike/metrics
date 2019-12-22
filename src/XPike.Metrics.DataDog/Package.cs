using XPike.IoC;

namespace XPike.Metrics.DataDog
{
    public class Package
        : IDependencyPackage
    {
        public void RegisterPackage(IDependencyCollection dependencyCollection)
        {
            dependencyCollection.LoadPackage(new XPike.Metrics.Package());

            dependencyCollection.RegisterSingleton<IDataDogMetricsProvider, DataDogMetricsProvider>();

            dependencyCollection.AddSingletonToCollection<IMetricsProvider, IDataDogMetricsProvider>(container =>
                container.ResolveDependency<IDataDogMetricsProvider>());
        }
    }
}