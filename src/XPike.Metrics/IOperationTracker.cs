namespace XPike.Metrics
{
    public interface IOperationTracker
        : IMetricsTimer
    {
        bool Successful { get; set; }

        string Result { get; set; }
        
        void SetSuccess(string result = "success");
        
        void SetFailure(string result = "failure");
    }
}