using XPike.IoC;

namespace XPike.Metrics.Aws
{
    public static class IDependencyCollectionExtensions
    {
        public static IDependencyCollection AddXPikeAwsMetrics(this IDependencyCollection collection) =>
            collection.LoadPackage(new XPike.Metrics.Aws.Package());
    }
}