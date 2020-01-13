namespace XPike.Metrics
{
    public interface IOperationTracker
        : IMetricsTimer
    {
        bool Successful { get; set; }

        string Result { get; set; }
        
        void SetSuccess(string result = "success", bool stopTimer = false);
        
        void SetFailure(string result = "failure", bool stopTimer = false);
    }
}