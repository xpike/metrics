namespace XPike.Metrics
{
    /// <summary>
    /// Represents the way in which a Metrics Context is acquired.
    /// </summary>
    public interface IMetricsContextAccessor
    {
        /// <summary>
        /// The current Metrics Context for this async execution scope.
        /// </summary>
        IMetricsContext MetricsContext { get; }
    }
}