namespace XPike.Metrics.DataDog
{
    public class DataDogProviderConfig
    {
        public const int DefaultStatsdPort = 8125;
        public const int DefaultStatsdMaxUDPPacketSize = 512;

        public DataDogProviderConfig()
        {
            StatsdPort = DefaultStatsdPort;
            StatsdMaxUDPPacketSize = DefaultStatsdMaxUDPPacketSize;
        }

        public string StatsdServerName { get; set; }
        
        public int StatsdPort { get; set; }
        
        public int StatsdMaxUDPPacketSize { get; set; }
        
        public bool StatsdTruncateIfTooLong { get; set; } = true;
    }
}
