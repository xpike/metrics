namespace XPike.Metrics
{
    /// <summary>
    /// Class MetricsSettings.
    /// </summary>
    public class MetricsSettings
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
        public string[] ConstantTags { get; set; }

        /// <summary>
        /// When bufffering, gets or sets the maximum messages per payload.
        /// </summary>
        /// <value>The maximum messages per payload.</value>
        public int MaxMessagesPerPayload { get; set; }
    }
}
