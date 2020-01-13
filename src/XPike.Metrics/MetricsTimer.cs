using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace XPike.Metrics
{
    public class MetricsTimer
        : IMetricsTimer
    {
        protected readonly Stopwatch _stopWatch;
        private bool _disposed;

        protected IMetricsService MetricsService { get; set; }

        protected string Name { get; set; }
        
        protected double SampleRate { get; set; }

        public IList<string> Tags { get; set; }

        public MetricsTimer(IMetricsService metricsService, string name, double sampleRate = 1.0, IEnumerable<string> tags = null)
        {
            Name = name;
            MetricsService = metricsService;
            SampleRate = sampleRate;

            var tempTags = new List<string>();
            if (tags != null)
                tempTags.AddRange(tags);

            Tags = tempTags;

            _stopWatch = Stopwatch.StartNew();
        }

        protected virtual void RecordTiming(string name, double elapsedMs, double sampleRate, IEnumerable<string> tags) =>
            MetricsService.Timer(name, elapsedMs, sampleRate, tags.ToArray());

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
                _stopWatch.Stop();

                RecordTiming(Name, _stopWatch.Elapsed.TotalMilliseconds, SampleRate, Tags);
            }
        }
    }
}