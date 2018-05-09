using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebHostBuilderTest
{
  public  class HostBuilder
    {
        public readonly TestServer userApiServer;
        public readonly TestServer identityServer;
        public readonly TestServer gateWayServer;
        public readonly TestServer contactApiServer;
        public HostBuilder()
        {
            userApiServer = new TestServer(new WebHostBuilder().UseStartup<User.API.Startup>());           
            identityServer= new TestServer(new WebHostBuilder().UseStartup<User.Identity.Startup>());
            gateWayServer= new TestServer(new WebHostBuilder().UseStartup<Gateway.API.Startup>());
        }
    }
}
