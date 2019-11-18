using System.Collections.Generic;
using XPike.Settings;

namespace XPike.Metrics
{
    /// <summary>
    /// A default implemenation of IMetricsService that sends metrics to providers synchronously.
    /// Implements the <see cref="XPike.Metrics.MetricsServiceBase" />
    /// </summary>
    /// <seealso cref="XPike.Metrics.MetricsServiceBase" />
    public class DefaultMetricsService : MetricsServiceBase
    {
        public DefaultMetricsService(ISettings<MetricsSettings> settings, IEnumerable<IMetricsProvider> metricsProviders) 
            : base(settings, metricsProviders)
        {
        }

        protected override void Send<T>(MetricType metric, string statName, T value, double sampleRate = 1, string[] tags = null)
        {
            foreach (var provider in MetricsProviders)
                provider.Send<T>(metric, statName, value, sampleRate, tags);
        }
    }
}
