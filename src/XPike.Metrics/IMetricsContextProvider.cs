using System.Collections.Generic;

namespace XPike.Metrics
{
    /// <summary>
    /// Represents the way through which a new Metrics Context is created.
    /// </summary>
    public interface IMetricsContextProvider
    {
        /// <summary>
        /// Creates a new Metrics Context instance.
        /// </summary>
        /// <returns>A new Metrics Context, appropriate for the current async execution scope.</returns>
        IMetricsContext CreateContext();

        /// <summary>
        /// A list of tags which will be applied to all newly created Metrics Context instances.
        /// </summary>
        IList<string> GlobalTags { get; }

        /// <summary>
        /// Adds a tag that will be applied to all new Metrics Context instances.
        /// </summary>
        /// <param name="key">The name of the tag.</param>
        /// <param name="value">The value for the tag.</param>
        void AddGlobalTag(string key, string value);
    }
}