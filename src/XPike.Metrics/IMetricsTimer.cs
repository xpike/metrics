using System;
using System.Collections.Generic;

namespace XPike.Metrics
{
    public interface IMetricsTimer
        : IDisposable
    {
        string Name { get; }

        IList<string> Tags { get; set; }
    }
}