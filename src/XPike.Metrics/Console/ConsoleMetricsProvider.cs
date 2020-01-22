using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace XPike.Metrics.Console
{
    public class ConsoleMetricsProvider
        : IConsoleMetricsProvider
    {
        private readonly ConcurrentQueue<Action> _events = new ConcurrentQueue<Action>();

        public void Send<T>(MetricType metric, string name, T value, double sampleRate, IEnumerable<string> tags)
        {
            var fg = System.Console.ForegroundColor;
            var bg = System.Console.BackgroundColor;

            System.Console.BackgroundColor = ConsoleColor.Black;
            System.Console.ForegroundColor = ConsoleColor.DarkCyan;

            System.Console.WriteLine($"[{metric}] {name}: {value} (sampleRate={sampleRate}, tags={string.Join(",", tags)})");

            System.Console.ForegroundColor = fg;
            System.Console.BackgroundColor = bg;
        }

        public void Add<T>(MetricType metric, string name, T value, double sampleRate, IEnumerable<string> tags) =>
            _events.Enqueue(() => Send<T>(metric, name, value, sampleRate, tags));

        public void Flush()
        {
            while (_events.TryDequeue(out var action))
                action();
        }
    }
}