using System.Collections.Generic;
using System.Threading;
using XPike.Configuration;

namespace XPike.Metrics
{
    /// <summary>
    /// MetricsService implementation that buffers metrics asynchronously and sends them when the max payload is reached.
    /// Implements the <see cref="XPike.Metrics.MetricsServiceBase" />
    /// </summary>
    /// <seealso cref="XPike.Metrics.MetricsServiceBase" />
    public class BufferedMetricsService
        : MetricsServiceBase,
          IBufferedMetricsService
    {
        private int count = 0;
        private int maxMessagesPerPayload;

        /// <summary>
        /// Initializes a new instance of the <see cref="BufferedMetricsService"/> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="metricsProviders">The metrics providers.</param>
        public BufferedMetricsService(IConfig<MetricsSettings> settings, IEnumerable<IMetricsProvider> metricsProviders) 
            : base(settings, metricsProviders)
        {
            maxMessagesPerPayload = settings.CurrentValue.MaxMessagesPerPayload;
        }

        /// <summary>
        /// Sends the specified metric to the configured providers.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="metric">The metric.</param>
        /// <param name="statName">Name of the stat.</param>
        /// <param name="value">The value.</param>
        /// <param name="sampleRate">The sample rate.</param>
        /// <param name="tags">The tags.</param>
        protected override void Send<T>(MetricType metric, string statName, T value, double sampleRate = 1, IEnumerable<string> tags = null)
        {
            Interlocked.Increment(ref count);
            if (count > maxMessagesPerPayload)
            {
                Interlocked.Exchange(ref count, 1);
                foreach (var provider in MetricsProviders)
                    provider.Flush();
            }

            foreach (var provider in MetricsProviders)
                provider.Add<T>(metric, statName, value, sampleRate, tags);
        }
    }
}
