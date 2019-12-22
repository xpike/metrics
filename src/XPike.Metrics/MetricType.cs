namespace XPike.Metrics
{
    public enum MetricType
    {
        Unknown = 0,
        Counting = 1,
        Timing = 2,
        Gauge = 3,
        Histogram = 4,
        Distribution = 5,
        Meter = 6,
        Set = 7
    }
}