using System.Collections.Generic;

namespace XPike.Metrics
{
    /// <summary>
    /// Holds metrics related information that is specific to the async execution context.
    /// </summary>
    public interface IMetricsContext
    {
        /// <summary>
        /// A list of tags that will be applied to all metrics recorded within the execution context.
        /// </summary>
        IList<string> Tags { get; }

        /// <summary>
        /// Adds a tag to the context.
        /// </summary>
        /// <param name="key">The name of the tag.</param>
        /// <param name="value">The value for the tag.</param>
        void AddTag(string key, string value);
    }
}