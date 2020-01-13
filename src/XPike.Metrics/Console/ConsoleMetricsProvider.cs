using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace XPike.Metrics.Console
{
    public class ConsoleMetricsProvider
        : IConsoleMetricsProvider
    {
        private readonly ConcurrentQueue<Action> _events = new ConcurrentQueue<Action>();

        public void Send<T>(MetricType metric, string name, T value, double sampleRate, IEnumerable<string> tags) =>
            System.Console.WriteLine($"[{metric}] {name}: {value} (sampleRate={sampleRate}, tags={string.Join(",", tags)})");

        public void Add<T>(MetricType metric, string name, T value, double sampleRate, IEnumerable<string> tags) =>
            _events.Enqueue(() => Send<T>(metric, name, value, sampleRate, tags));

        public void Flush()
        {
            while (_events.TryDequeue(out var action))
                action();
        }
    }
}