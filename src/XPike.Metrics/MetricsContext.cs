using System.Collections.Generic;
using System.Linq;

namespace XPike.Metrics
{
    /// <summary>
    /// NOTE: This implementation is not currently thread-safe when modifying the Tags collection.
    /// </summary>
    public class MetricsContext
        : IMetricsContext
    {
        public IList<string> Tags { get; }

        public MetricsContext(IEnumerable<string> tags)
        {
            Tags = tags.ToList();
        }

        public void AddTag(string key, string value) =>
            Tags.Add($"{key}:{value}");
    }
}