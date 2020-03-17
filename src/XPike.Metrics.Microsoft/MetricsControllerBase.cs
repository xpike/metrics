using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace XPike.Metrics.Microsoft
{
    public abstract class MetricsControllerBase
        : Controller
    {
        private readonly IMetricsContextAccessor _contextAccessor;
        private readonly IMetricsService _service;

        public MetricsControllerBase(IMetricsService service, IMetricsContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
            _service = service;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                var metricsContext = _contextAccessor.MetricsContext;
                
                // populate...
            }
            catch (Exception)
            {
                // Intentional no-op.
            }

            base.OnActionExecuting(context);
        }

        private async Task<T> ExecuteAsync<T>(Func<IOperationTracker, Task<T>> asyncOperation, IOperationTracker tracker)
        {
            try
            {
                var result = await asyncOperation(tracker);

                if (string.IsNullOrWhiteSpace(tracker.Result))
                    tracker.SetSuccess();
                
                return result;
            }
            catch (Exception ex)
            {
                tracker.Tags.Add($"exception:{ex.GetType().Name}");

                if (string.IsNullOrWhiteSpace(tracker.Result))
                    tracker.SetFailure("exception");

                throw;
            }
        }

        protected async Task<T> WithTimingAsync<T>(Func<IOperationTracker, Task<T>> asyncOperation)
        {
            var tracker = _contextAccessor.OperationTracker;

            if (tracker != null)
                return await ExecuteAsync<T>(asyncOperation, tracker);

            try
            {
                var metricsContext = _contextAccessor.MetricsContext;

                // populate...
            }
            catch (Exception)
            {
                // Intentional no-op.
            }

            using (tracker = _service.StartTracker("controller"))
                return await ExecuteAsync<T>(asyncOperation, tracker);
        }
    }
}
