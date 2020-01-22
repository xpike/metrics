using XPike.IoC;

namespace XPike.Metrics.DataDog
{
    public static class IDependencyCollectionExtensions
    {
        public static IDependencyCollection AddXPikeDataDogMetrics(this IDependencyCollection collection) =>
            collection.LoadPackage(new XPike.Metrics.DataDog.Package());

        public static IDependencyCollection UseXPikeDataDogMetrics(this IDependencyCollection collection)
        {
            collection.ResetCollection<IMetricsProvider>();
            return collection.AddXPikeDataDogMetrics();
        }
    }
}