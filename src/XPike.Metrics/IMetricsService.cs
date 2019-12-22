using System;
using System.Collections.Generic;

namespace XPike.Metrics
{
    /// <summary>
    /// MetricsService interface
    /// </summary>
    public interface IMetricsService
    {
        void Counter<T>(string statName, T value, double sampleRate = 1, IEnumerable<string> tags = null);

        void Decrement(string statName, int value = 1, double sampleRate = 1, IEnumerable<string> tags = null);

        void Gauge<T>(string statName, T value, double sampleRate = 1, IEnumerable<string> tags = null);
        
        void Histogram<T>(string statName, T value, double sampleRate = 1, IEnumerable<string> tags = null);
        
        void Distribution<T>(string statName, T value, double sampleRate = 1, IEnumerable<string> tags = null);
        
        void Increment(string statName, int value = 1, double sampleRate = 1, IEnumerable<string> tags = null);
        
        void Set<T>(string statName, T value, double sampleRate = 1, IEnumerable<string> tags = null);
        
        IMetricsTimer StartTimer(string name, double sampleRate = 1, IEnumerable<string> tags = null);

        IOperationTracker StartTracker(string name, double sampleRate = 1, IEnumerable<string> tags = null);
        
        void Time(Action action, string statName, double sampleRate = 1, IEnumerable<string> tags = null);
        
        T Time<T>(Func<T> func, string statName, double sampleRate = 1, IEnumerable<string> tags = null);
        
        void Timer<T>(string statName, T value, double sampleRate = 1, IEnumerable<string> tags = null);
    }
}
