using System.Collections.Generic;

namespace XPike.Metrics
{
    public enum MetricType
    {
        Counting,
        Timing,
        Gauge,
        Histogram,
        Distribution,
        Meter,
        Set
    }

    /// <summary>
    /// Interface implemented by all xPike metrics providers.
    /// </summary>
    public interface IMetricsProvider
    {
        /// <summary>
        /// Sends the specified metric immediately to the destination.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="metric">The metric.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <param name="sampleRate">The sample rate.</param>
        /// <param name="tags">The tags.</param>
        void Send<T>(MetricType metric, string name, T value, double sampleRate, IEnumerable<string> tags);

        /// <summary>
        /// Adds the specified metric to a buffered queue to be sent later.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="metric">The metric.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <param name="sampleRate">The sample rate.</param>
        /// <param name="tags">The tags.</param>
        void Add<T>(MetricType metric, string name, T value, double sampleRate, IEnumerable<string> tags);

        /// <summary>
        /// Sends queued metrics to the destination.
        /// </summary>
        void Flush();
    }
}
