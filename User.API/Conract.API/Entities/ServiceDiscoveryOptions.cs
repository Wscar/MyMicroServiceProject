using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Contact.API.Entities
{
    public class ServiceDiscoveryOptions
    {
        public string ServiceName { get; set; }
        public ConsulOptinons Consul { get; set; }

    }
    public class ConsulOptinons
    {
        public string HttpEndpoint { get; set; }
        public DnsEndpoint DnsEndpoint { get; set; }
    }
    public class DnsEndpoint
    {
        public string Address { get; set; }
        public int Port { get; set; }
        public IPEndPoint ToIPEndPoint()
        {
            return new IPEndPoint(IPAddress.Parse(Address), Port);
        }
       
    }
}
