using XPike.IoC;

namespace XPike.Metrics.Azure
{
    public static class IDependencyCollectionExtensions
    {
        public static IDependencyCollection AddXPikeDataDogMetrics(this IDependencyCollection collection) =>
            collection.LoadPackage(new XPike.Metrics.Azure.Package());
    }
}