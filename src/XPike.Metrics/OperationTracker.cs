using System.Collections.Generic;

namespace XPike.Metrics
{
    public class OperationTracker
        : MetricsTimer,
          IOperationTracker
    {
        private readonly bool _recordTiming;
        private readonly bool _recordAttempt;
        private readonly bool _recordResult;

        public bool Successful { get; set; }

        public string Result { get; set; }
        
        public OperationTracker(IMetricsService metricsService,
            string name,
            double sampleRate = 1D,
            IEnumerable<string> tags = null,
            bool recordTiming = true,
            bool recordAttempt = false,
            bool recordResult = false)
            : base(metricsService, name, sampleRate, tags)
        {
            _recordTiming = recordTiming;
            _recordAttempt = recordAttempt;
            _recordResult = recordResult;
        }

        public void SetSuccess(string result = "success", bool stopTimer = false)
        {
            Successful = true;
            Result = result;

            if (stopTimer)
                _stopWatch.Stop();
        }

        public void SetFailure(string result = "failure", bool stopTimer = false)
        {
            Successful = false;
            Result = result;

            if (stopTimer)
                _stopWatch.Stop();
        }

        protected override void RecordTiming(string name, double elapsedMs, double sampleRate, IEnumerable<string> tags)
        {
            Tags.Add($"success:{Successful}");
            Tags.Add($"result:{Result}");

            if (_recordAttempt)
                MetricsService.Increment(Name, 1, SampleRate, Tags);

            if (_recordResult)
                MetricsService.Increment($"{Name}.result", 1, SampleRate, Tags);

            if (_recordTiming)
                base.RecordTiming($"{name}.timing", elapsedMs, sampleRate, Tags);
        }
    }
}