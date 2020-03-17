using System.Collections.Concurrent;

#if NETSTD
using System.Threading;

#elif NETFX
using System.Runtime.Remoting.Messaging;
#endif

namespace XPike.Metrics
{
    public class MetricsContextAccessor
        : IMetricsContextAccessor
    {
#if NETSTD
        private static readonly AsyncLocal<IMetricsContext> _localizer = new AsyncLocal<IMetricsContext>();
        private static readonly AsyncLocal<ConcurrentStack<IOperationTracker>> _contextLocalizer = new AsyncLocal<ConcurrentStack<IOperationTracker>>();
#endif

        private readonly IMetricsContextProvider _provider;

#if NETSTD
        public IMetricsContext MetricsContext =>
            _localizer.Value ?? (_localizer.Value = _provider.CreateContext());

        private ConcurrentStack<IOperationTracker> GetTrackers() =>
            _contextLocalizer.Value ?? (_contextLocalizer.Value = new ConcurrentStack<IOperationTracker>());
#elif NETFX
        public IMetricsContext MetricsContext
        {
            get
            {
                var context = (IMetricsContext) CallContext.LogicalGetData(GetType().ToString());

                if (context == null)
                {
                    context = _provider.CreateContext();
                    CallContext.LogicalSetData(GetType().ToString(), context);
                }

                return context;
            }
        }

        public ConcurrentStack<IOperationTracker> GetTrackers()
        {
            var context = (ConcurrentStack<IOperationTracker>) CallContext.LogicalGetData($"{GetType()}.trackers");

            if (context == null)
            {
                context = new ConcurrentStack<IOperationTracker>();
                CallContext.LogicalSetData($"{GetType()}.trackers", context);
            }

            return context;
        }
#endif

        public IOperationTracker OperationTracker =>
            GetTrackers().TryPop(out var tracker) ? tracker : null;

        public void AddTracker(IOperationTracker tracker) =>
            GetTrackers().Push(tracker);

        public void RemoveTracker() =>
            GetTrackers().TryPop(out _);

        public MetricsContextAccessor(IMetricsContextProvider provider)
        {
            _provider = provider;
        }
    }
}