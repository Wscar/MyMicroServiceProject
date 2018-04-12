using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace User.Identity.Dtos
{
    public class ServiceDiscoveryOptions
    {
        public string UserServiceName { get; set; }
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
