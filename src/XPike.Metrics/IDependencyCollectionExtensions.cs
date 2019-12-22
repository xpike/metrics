using XPike.IoC;

namespace XPike.Metrics
{
    public static class IDependencyCollectionExtensions
    {
        public static IDependencyCollection AddXPikeMetrics(this IDependencyCollection collection) =>
            collection.LoadPackage(new XPike.Metrics.Package());
    }
}