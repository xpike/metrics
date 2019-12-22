using System;

namespace XPike.Metrics.DataDog
{
    public interface IDataDogMetricsProvider
        : IMetricsProvider,
          IDisposable
    {
    }
}