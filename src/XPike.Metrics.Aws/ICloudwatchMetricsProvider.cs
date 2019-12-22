using System;

namespace XPike.Metrics.Aws
{
    public interface ICloudwatchMetricsProvider
        : IMetricsProvider,
          IDisposable
    {
    }
}