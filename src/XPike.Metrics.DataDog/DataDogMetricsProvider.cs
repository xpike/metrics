using StatsdClient;
using System;
using System.Collections.Generic;
using System.Linq;
using XPike.Settings;

namespace XPike.Metrics.DataDog
{
    public class DataDogMetricsProvider : IMetricsProvider, IDisposable
    {
        public const string DD_ENTITY_ID_ENV_VAR = "DD_ENTITY_ID";
        public const string DD_DOGSTATSD_PORT_ENV_VAR = "DD_DOGSTATSD_PORT";
        public const string DD_AGENT_HOST_ENV_VAR = "DD_AGENT_HOST";

        private Statsd dog;
        private StatsdUDP statsdUDP;

        private readonly object semaphore = new object();

        public DataDogMetricsProvider(ISettings<DataDogProviderSettings> settings)
        {
            if (settings == null)
                    throw new ArgumentNullException(nameof(settings));

            statsdUDP = new StatsdUDP(settings.Value.StatsdServerName, settings.Value.StatsdPort, settings.Value.StatsdMaxUDPPacketSize);
            dog = new Statsd(statsdUDP);
            dog.TruncateIfTooLong = settings.Value.StatsdTruncateIfTooLong;
        }

        public void Send<T>(MetricType metric, string name, T value, double sampleRate, IEnumerable<string> tags)
        {
            switch (metric)
            {
                case MetricType.Counting:
                    dog.Send<Statsd.Counting, T>(name, value, sampleRate, tags.ToArray());
                    break;
                case MetricType.Timing:
                    dog.Send<Statsd.Timing, T>(name, value, sampleRate, tags.ToArray());
                    break;
                case MetricType.Gauge:
                    dog.Send<Statsd.Gauge, T>(name, value, sampleRate, tags.ToArray());
                    break;
                case MetricType.Histogram:
                    dog.Send<Statsd.Histogram, T>(name, value, sampleRate, tags.ToArray());
                    break;
                case MetricType.Distribution:
                    dog.Send<Statsd.Distribution, T>(name, value, sampleRate, tags.ToArray());
                    break;
                case MetricType.Meter:
                    dog.Send<Statsd.Meter, T>(name, value, sampleRate, tags.ToArray());
                    break;
                case MetricType.Set:
                    dog.Send<Statsd.Set, T>(name, value, sampleRate, tags.ToArray());
                    break;
            }
        }

        public void Add<T>(MetricType metric, string name, T value, double sampleRate, IEnumerable<string> tags)
        {
            // The "buffering" in the DataDog client is not thread safe. It uses a List<string>.
            lock (semaphore)
            {
                switch (metric)
                {
                    case MetricType.Counting:
                        dog.Add<Statsd.Counting, T>(name, value, sampleRate, tags.ToArray());
                        break;
                    case MetricType.Timing:
                        dog.Add<Statsd.Timing, T>(name, value, sampleRate, tags.ToArray());
                        break;
                    case MetricType.Gauge:
                        dog.Add<Statsd.Gauge, T>(name, value, sampleRate, tags.ToArray());
                        break;
                    case MetricType.Histogram:
                        dog.Add<Statsd.Histogram, T>(name, value, sampleRate, tags.ToArray());
                        break;
                    case MetricType.Distribution:
                        dog.Add<Statsd.Distribution, T>(name, value, sampleRate, tags.ToArray());
                        break;
                    case MetricType.Meter:
                        dog.Add<Statsd.Meter, T>(name, value, sampleRate, tags.ToArray());
                        break;
                    case MetricType.Set:
                        dog.Add<Statsd.Set, T>(name, value, sampleRate, tags.ToArray());
                        break;
                }
            }
        }

        public void Flush()
        {
            lock (semaphore)
            {
                dog.Send();
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            Flush();

            if (!disposedValue)
            {
                if (disposing)
                {
                    statsdUDP?.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.

                // TODO: set large fields to null.
                statsdUDP = null;
                dog = null;

                disposedValue = true;
            }
        }

        ~DataDogMetricsProvider()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
