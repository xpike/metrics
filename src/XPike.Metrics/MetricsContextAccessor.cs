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
#endif

        private readonly IMetricsContextProvider _provider;

#if NETSTD
        public IMetricsContext MetricsContext =>
            _localizer.Value ?? (_localizer.Value = _provider.CreateContext());
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
#endif

        public MetricsContextAccessor(IMetricsContextProvider provider)
        {
            _provider = provider;
        }
    }
}