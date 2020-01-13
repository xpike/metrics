using System.Collections.Generic;

namespace XPike.Metrics.Console
{
    public interface IConsoleMetricsProvider : IMetricsProvider
    {
        void Send<T>(MetricType metric, string name, T value, double sampleRate, IEnumerable<string> tags);
        void Add<T>(MetricType metric, string name, T value, double sampleRate, IEnumerable<string> tags);
        void Flush();
    }
}