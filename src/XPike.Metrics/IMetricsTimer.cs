using System;
using System.Collections.Generic;

namespace XPike.Metrics
{
    public interface IMetricsTimer
        : IDisposable
    {
        IList<string> Tags { get; set; }
    }
}