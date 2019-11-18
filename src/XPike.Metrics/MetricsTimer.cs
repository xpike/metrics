using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace XPike.Metrics
{
    internal class MetricsTimer : IDisposable
    {
        private readonly string _name;
        private readonly IMetricsService _metricsService;
        private readonly Stopwatch _stopWatch;
        private bool _disposed;
        private readonly double _sampleRate;

        public MetricsTimer(IMetricsService metricsService, string name, double sampleRate = 1.0, string[] tags = null)
        {
            _name = name;
            _metricsService = metricsService;
            _sampleRate = sampleRate;
            Tags = new List<string>();
            if (tags != null)
                Tags.AddRange(tags);

            _stopWatch = Stopwatch.StartNew();
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
                _stopWatch.Stop();

                _metricsService.Timer(_name, _stopWatch.ElapsedMilliseconds, _sampleRate, Tags.ToArray());
            }
        }

        public List<string> Tags { get; set; }
    }
}
