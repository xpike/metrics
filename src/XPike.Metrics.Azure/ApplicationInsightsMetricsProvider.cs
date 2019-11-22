using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace XPike.Metrics.Azure
{
    /// <summary>
    /// xPike metrics provider for sending metrics to Azure Application Insights.
    /// Implements the <see cref="XPike.Metrics.IMetricsProvider" />
    /// </summary>
    /// <remarks>
    /// It is recommended that you use the default, non-buffering MetricService and let the application Insights
    /// telemetry client handle the batching and sampling.
    /// </remarks>
    /// <seealso cref="XPike.Metrics.IMetricsProvider" />
    public class ApplicationInsightsMetricsProvider : IMetricsProvider
    {
        TelemetryClient telemetryClient;

        // The only current collection with a Clear() method in Full Framework.
        ConcurrentStack<MetricTelemetry> queue = new ConcurrentStack<MetricTelemetry>();

        bool isFlushing = false;

        object monitor = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationInsightsMetricsProvider"/> class.
        /// </summary>
        /// <param name="telemetryClient">The telemetry client.</param>
        public ApplicationInsightsMetricsProvider(TelemetryClient telemetryClient)
        {
            this.telemetryClient = telemetryClient;
        }

        public void Send<T>(MetricType metric, string name, T value, double sampleRate, IEnumerable<string> tags)
        {
            telemetryClient.TrackMetric(GetTelemetry(metric, name, value, sampleRate, tags));
        }

        public void Add<T>(MetricType metric, string name, T value, double sampleRate, IEnumerable<string> tags)
        {
            // We only want to lock on flush so we get concurrent adds. Locks are expensive. This will ensure we only 
            // take the hit when Flush() is sweeping to it's own local array.
            if (isFlushing)
            {
                // if we're flushing, wait till the flushing lock is released
                Monitor.Wait(monitor);
            }

            queue.Push(GetTelemetry(metric, name, value, sampleRate, tags));
        }

        public void Flush()
        {
            // We only want to lock/block on flush so we get concurrent adds
            isFlushing = true;

            // sweep into a local array for processing...
            MetricTelemetry[] metrics = queue.ToArray();
            queue.Clear();

            //...and release the lock as soon as possible. This allows adds again while the slow IO happens later
            //   using the copied array.
            isFlushing = false;
            Monitor.PulseAll(monitor);

            foreach (var metric in metrics)
                telemetryClient.TrackMetric(metric);
        }

        private MetricTelemetry GetTelemetry(MetricType metric, string name, object value, double sampleRate, IEnumerable<string> tags)
        {
            var telemetry = new MetricTelemetry(name, Convert.ToDouble(value));

            if (tags == null || !tags.Any())
                return telemetry;

            foreach (string tag in tags)
            {
                string[] nvp = tag.Split(':');
                if (nvp.Length != 2)
                    continue; //invalid tag pair, so skip it

                telemetry.Properties.Add(nvp[0], nvp[1]);
            }

            return telemetry;
        }
    }
}
