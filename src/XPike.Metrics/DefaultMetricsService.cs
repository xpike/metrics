using System.Collections.Generic;
using XPike.Configuration;

namespace XPike.Metrics
{
    /// <summary>
    /// A default implementation of IMetricsService that sends metrics to providers synchronously.
    /// Implements the <see cref="XPike.Metrics.MetricsServiceBase" />
    /// </summary>
    /// <seealso cref="XPike.Metrics.MetricsServiceBase" />
    public class DefaultMetricsService
        : MetricsServiceBase,
          IDefaultMetricsService
    {
        public DefaultMetricsService(IConfig<MetricsConfig> config, IEnumerable<IMetricsProvider> metricsProviders, IMetricsContextAccessor contextAccessor) 
            : base(config, metricsProviders, contextAccessor)
        {
        }

        protected override void Send<T>(MetricType metric, string statName, T value, double sampleRate = 1, IEnumerable<string> tags = null)
        {
            foreach (var provider in MetricsProviders)
                provider.Send<T>(metric, statName, value, sampleRate, tags);
        }
    }
}
