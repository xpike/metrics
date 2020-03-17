using System;
using System.Collections.Generic;
using System.Linq;
using XPike.Configuration;

namespace XPike.Metrics
{
    /// <summary>
    /// Base class for IMetricService implementations.
    /// Implements the <see cref="XPike.Metrics.IMetricsService" />
    /// </summary>
    /// <seealso cref="XPike.Metrics.IMetricsService" />
    public abstract class MetricsServiceBase : IMetricsService
    {
        private readonly IConfig<MetricsConfig> _config;
        private readonly IMetricsContextAccessor _contextAccessor;

        private Dictionary<string, string> _globalTags = new Dictionary<string, string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="MetricsServiceBase"/> class.
        /// </summary>
        /// <param name="config">The _config.</param>
        /// <param name="metricsProviders">The metrics providers.</param>
        protected MetricsServiceBase(IConfig<MetricsConfig> config, IEnumerable<IMetricsProvider> metricsProviders, IMetricsContextAccessor contextAccessor)
        {
            _config = config;
            _contextAccessor = contextAccessor;

            MetricsProviders = metricsProviders;
        }

        /// <summary>
        /// Gets the metrics providers.
        /// </summary>
        /// <value>The metrics providers.</value>
        protected IEnumerable<IMetricsProvider> MetricsProviders { get; }

        public void SetGlobalTag(string name, string value)
        {
            _globalTags[name] = value;
        }

        public void Counter<T>(string statName, T value, double sampleRate = 1, IEnumerable<string> tags = null)
        {
            PrepareAndSend<T>(MetricType.Counting, statName, value, sampleRate, tags);
        }

        public void Decrement(string statName, int value = 1, double sampleRate = 1, IEnumerable<string> tags = null)
        {
            PrepareAndSend<int>(MetricType.Counting, statName, -value, sampleRate, tags);
        }

        public void Distribution<T>(string statName, T value, double sampleRate = 1, IEnumerable<string> tags = null)
        {
            PrepareAndSend<T>(MetricType.Distribution, statName, value, sampleRate, tags);
        }

        public void Gauge<T>(string statName, T value, double sampleRate = 1, IEnumerable<string> tags = null)
        {
            PrepareAndSend<T>(MetricType.Gauge, statName, value, sampleRate, tags);
        }

        public void Histogram<T>(string statName, T value, double sampleRate = 1, IEnumerable<string> tags = null)
        {
            PrepareAndSend<T>(MetricType.Histogram, statName, value, sampleRate, tags);
        }

        public void Increment(string statName, int value = 1, double sampleRate = 1, IEnumerable<string> tags = null)
        {
            PrepareAndSend<int>(MetricType.Counting, statName, value, sampleRate, tags);
        }

        public void Set<T>(string statName, T value, double sampleRate = 1, IEnumerable<string> tags = null)
        {
            PrepareAndSend<T>(MetricType.Set, statName, value, sampleRate, tags);
        }

        public IMetricsTimer StartTimer(string statName, double sampleRate = 1, IEnumerable<string> tags = null)
        {
            return new MetricsTimer(this, statName, sampleRate, tags);
        }

        public IOperationTracker StartTracker(string statName, double sampleRate = 1, IEnumerable<string> tags = null)
        {
            var config = _config.CurrentValue;
            return new OperationTracker(this,
                _contextAccessor,
                statName,
                sampleRate,
                tags,
                config.TrackTiming,
                config.TrackAttempts,
                config.TrackResults);
        }

        public void Time(Action action, string statName, double sampleRate = 1, IEnumerable<string> tags = null)
        {
            using (StartTimer(statName, sampleRate, tags))
            {
                action();
            }
        }

        public T Time<T>(Func<T> func, string statName, double sampleRate = 1, IEnumerable<string> tags = null)
        {
            using (StartTimer(statName, sampleRate, tags))
            {
                return func();
            }
        }

        public void Timer<T>(string statName, T value, double sampleRate = 1, IEnumerable<string> tags = null)
        {
            PrepareAndSend<T>(MetricType.Timing, statName, value, sampleRate, tags);
        }

        private void PrepareAndSend<T>(MetricType metric, string statName, T value, double sampleRate = 1, IEnumerable<string> tags = null)
        {
            string name = $"{_config.CurrentValue.Prefix}.{statName}";

            Send<T>(metric, name, value, sampleRate, _contextAccessor.MetricsContext.Tags.Union(tags));
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
        protected abstract void Send<T>(MetricType metric, string statName, T value, double sampleRate = 1, IEnumerable<string> tags = null);
    }
}
