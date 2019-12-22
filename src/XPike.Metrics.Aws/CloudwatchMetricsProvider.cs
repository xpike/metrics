using Amazon.CloudWatch;
using Amazon.CloudWatch.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using XPike.Configuration;

namespace XPike.Metrics.Aws
{
    /// <summary>
    /// xPike metrics provider for sending metrics to AWS Cloudwatch.
    /// Implements the <see cref="XPike.Metrics.IMetricsProvider" />
    /// </summary>
    /// <seealso cref="XPike.Metrics.IMetricsProvider" />
    public class CloudwatchMetricsProvider
        : ICloudwatchMetricsProvider
    {
        AmazonCloudWatchClient cloudwatch;
        IConfig<MetricsSettings> settings;

        // The only current collection with a Clear() method in Full Framework.
        ConcurrentStack<MetricDatum> queue = new ConcurrentStack<MetricDatum>();

        bool isFlushing = false;

        object monitor = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="CloudwatchMetricsProvider"/> class using the instance configured profile.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public CloudwatchMetricsProvider(IConfig<MetricsSettings> settings)
        {
            this.settings = settings;
            cloudwatch = new AmazonCloudWatchClient();
        }

        /// <summary>
        /// Adds the specified metric to a buffered queue to be sent later.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="metric">The metric.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <param name="sampleRate">The sample rate.</param>
        /// <param name="tags">The tags.</param>
        public void Add<T>(MetricType metric, string name, T value, double sampleRate, IEnumerable<string> tags)
        {
            // We only want to lock on flush so we get concurrent adds. Locks are expensive. This will ensure we only 
            // take the hit when Flush() is sweeping to it's own local array.
            if (isFlushing)
            {
                // if we're flushing, wait till the flushing lock is released
                Monitor.Wait(monitor);
            }
                
            queue.Push(GetMetricDatum(metric, name, value, sampleRate, tags));
            
        }

        /// <summary>
        /// Sends queued metrics to the destination.
        /// </summary>
        public void Flush()
        {
            // We only want to lock/block on flush so we get concurrent adds
            isFlushing = true;

            // sweep into a local array for processing...
            MetricDatum[] metrics = queue.ToArray();
            queue.Clear();

            //...and release the lock as soon as possible. This allows adds again while the slow IO happens later
            //   using the copied array.
            isFlushing = false;
            Monitor.PulseAll(monitor);

            var request = new PutMetricDataRequest
            {
                Namespace = settings.CurrentValue.Prefix,
                MetricData = new List<MetricDatum>(metrics) 
            };

            cloudwatch.PutMetricDataAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Sends the specified metric immediately to the destination.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="metric">The metric.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <param name="sampleRate">The sample rate.</param>
        /// <param name="tags">The tags.</param>
        public void Send<T>(MetricType metric, string name, T value, double sampleRate, IEnumerable<string> tags)
        {
            var request = new PutMetricDataRequest {
                Namespace = settings.CurrentValue.Prefix,
                MetricData = new List<MetricDatum> { { GetMetricDatum(metric, name, value, sampleRate, tags) } }
            };

            cloudwatch.PutMetricDataAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        private List<Dimension> GetDimensions(IEnumerable<string> tags)
        {
            List<Dimension> dimensions = new List<Dimension>();

            if (tags == null || !tags.Any())
                return dimensions;

            foreach(string tag in tags)
            {
                string[] nvp = tag.Split(':');
                if (nvp.Length != 2)
                    continue; //invalid tag pair, so skip it

                dimensions.Add(new Dimension {  Name = nvp[0], Value = nvp[1] });
            }

            return dimensions;
        }

        private MetricDatum GetMetricDatum(MetricType metric, string name, object value, double sampleRate, IEnumerable<string> tags)
        {
            var metricData = new MetricDatum
            {
                TimestampUtc = DateTime.UtcNow,
                Dimensions = GetDimensions(tags),
                MetricName = name.Remove(0, settings.CurrentValue.Prefix.Length + 1)
            };

            switch(metric)
            {
                case MetricType.Counting:
                    metricData.Value = Convert.ToDouble(value);
                    metricData.Unit = StandardUnit.Count;
                    break;
                case MetricType.Timing:
                    metricData.Value = Convert.ToDouble(value);
                    metricData.Unit = StandardUnit.Milliseconds;
                    break;
                case MetricType.Gauge:
                    metricData.Value = Convert.ToDouble(value);
                    metricData.Unit = StandardUnit.None;
                    break;
                case MetricType.Histogram:
                    metricData.Value = Convert.ToDouble(value);
                    metricData.Unit = StandardUnit.None;
                    break;
                case MetricType.Distribution:
                    metricData.Value = Convert.ToDouble(value);
                    metricData.Unit = StandardUnit.None;
                    break;
                case MetricType.Meter:
                    metricData.Value = Convert.ToDouble(value);
                    metricData.Unit = StandardUnit.None;
                    break;
                case MetricType.Set:
                    metricData.Value = Convert.ToDouble(value);
                    metricData.Unit = StandardUnit.None;
                    break;
            }
            
            return metricData;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    cloudwatch?.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                cloudwatch = null;
                settings = null;
                queue = null;

                disposedValue = true;
            }
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="CloudwatchMetricsProvider"/> class.
        /// </summary>
        ~CloudwatchMetricsProvider()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.        
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
