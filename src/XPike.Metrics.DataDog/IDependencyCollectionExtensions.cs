using XPike.IoC;

namespace XPike.Metrics.DataDog
{
    public static class IDependencyCollectionExtensions
    {
        public static IDependencyCollection AddXPikeDataDogMetrics(this IDependencyCollection collection) =>
            collection.LoadPackage(new XPike.Metrics.DataDog.Package());
    }
}