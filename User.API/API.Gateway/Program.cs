using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Gateway.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
             //为ocelot添加配置文件
            .ConfigureAppConfiguration((webhost, builder) => {
                builder.SetBasePath(webhost.HostingEnvironment.ContentRootPath)
                .AddJsonFile("Ocelot.json");
            })
          
                .UseStartup<Startup>()
              .UseUrls("http://localhost:80")
                .Build();
    }
}
