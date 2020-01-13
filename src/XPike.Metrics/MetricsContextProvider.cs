using System;
using System.Collections.Generic;
using System.Linq;
using XPike.Configuration;

namespace XPike.Metrics
{
    public class MetricsContextProvider
        : IMetricsContextProvider
    {
        private readonly List<string> _globalTags = new List<string>();
        private readonly IConfig<MetricsConfig> _config;

        public IList<string> GlobalTags =>
            _globalTags;
            
        public MetricsContextProvider(IConfig<MetricsConfig> config)
        {
            _config = config;

            AddGlobalTag("machine", Environment.MachineName);
            AddGlobalTag("hostname", NetUtil.GetHostname());
            AddGlobalTag("publicip", NetUtil.GetPublicIp());
            AddGlobalTag("localip", NetUtil.GetLocalIp());
        }
        
        public IMetricsContext CreateContext() =>
            new MetricsContext(_globalTags.Union(_config.CurrentValue.ConstantTags).ToList());

        public void AddGlobalTag(string key, string value) =>
            _globalTags.Add($"{key}:{value}");
    }
}