using System.Collections.Generic;

namespace XPike.Metrics
{
    /// <summary>
    /// Configuration for the default implementation of XPike.Metrics.
    /// </summary>
    public class MetricsConfig
    {
        /// <summary>
        /// Gets or sets the prefix to apply to all metric names.
        /// </summary>
        /// <value>The prefix.</value>
        public string Prefix { get; set; }

        /// <summary>
        /// Gets or sets the tags to attach to every metric.
        /// </summary>
        /// <value>The constant tags.</value>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1819:Properties should not return arrays", Justification = "We want an array here.")]
        public IList<string> ConstantTags { get; set; }

        /// <summary>
        /// When buffering, gets or sets the maximum messages per payload.
        /// </summary>
        /// <value>The maximum messages per payload.</value>
        public int MaxMessagesPerPayload { get; set; }

        /// <summary>
        /// Controls if OperationTracker records an independent metric when an operation is attempted.
        /// </summary>
        public bool TrackAttempts { get; set; }

        /// <summary>
        /// Controls if OperationTracker records an independent metric when an operation completes.
        /// </summary>
        public bool TrackResults { get; set; }

        /// <summary>
        /// Controls if OperationTracker records an independent metric for the elapsed time.
        /// </summary>
        public bool TrackTiming { get; set; }

        public bool EnableRequestMetrics { get; set; }
    }
}