using System.Collections.Generic;

namespace XPike.Metrics
{
    public class OperationTracker
        : MetricsTimer,
          IOperationTracker
    {
        public bool Successful { get; set; }

        public string Result { get; set; }
        
        public OperationTracker(IMetricsService metricsService,
            string name,
            double sampleRate = 1D,
            IEnumerable<string> tags = null)
            : base(metricsService, name, sampleRate, tags)
        {
        }

        public void SetSuccess(string result = "success")
        {
            Successful = true;
            Result = result;
        }

        public void SetFailure(string result = "failure")
        {
            Successful = false;
            Result = result;
        }

        protected override void RecordTiming(string name, long elapsedMs, double sampleRate, IEnumerable<string> tags)
        {
            Tags.Add($"success:{Successful}");
            Tags.Add($"result:{Result}");

            MetricsService.Increment(Name, 1, SampleRate, Tags);
            MetricsService.Increment($"{Name}.result", 1, SampleRate, Tags);
            
            base.RecordTiming($"{name}.timing", elapsedMs, sampleRate, tags);
        }
    }
}