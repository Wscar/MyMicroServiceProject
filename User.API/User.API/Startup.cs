using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using User.API.Data;
using User.API.Filters;
using User.API.Entities;
using Microsoft.Extensions.Options;
using Consul;
namespace User.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<UserContext>(optinos =>
            {
                optinos.UseMySQL(Configuration.GetConnectionString("Mysql"));
            });
            //获取配置文件
            services.Configure<ServiceDiscoveryOptions>(Configuration.GetSection("ServiceDiscovery"));
            //注册IConsulClient
            services.AddSingleton<IConsulClient>(p => new ConsulClient(consul =>
            {
                var serviceConfigation = p.GetRequiredService<IOptions<ServiceDiscoveryOptions>>().Value;
                if (!string.IsNullOrEmpty(serviceConfigation.Consul.HttpEndpoint))
                {
                    consul.Address = new Uri(serviceConfigation.Consul.HttpEndpoint);
                }
            }));
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(GlobalExceptionFilter));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime applicationLifetime,
            IOptions<ServiceDiscoveryOptions> options, IConsulClient consul)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            app.UseStaticFiles();
            //启动的时候注册服务
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
             applicationLifetime.ApplicationStarted.Register(() => { RegisterService(app, options, consul); });
            //RegisterService(app, options, consul);
            //停止时移除服务
            applicationLifetime.ApplicationStopped.Register(() => { DeRegisterService(app, options, consul); });
        }
        /// <summary>
        /// 注册服务
        /// </summary>
        public void RegisterService(IApplicationBuilder app, IOptions<ServiceDiscoveryOptions> ServiceOptions,
            IConsulClient consul)
        {
            //获取本地服务地址
            var features = app.Properties["server.Features"] as FeatureCollection;
            var address = features.Get<IServerAddressesFeature>()
                .Addresses.Select(x => new Uri(x));
            foreach (var addr in address)
            {
                var serverId = $"{ ServiceOptions.Value.ServiceName}_{addr.Host}:{addr.Port}";
                var httpCheck = new AgentServiceCheck()
                {
                    DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(1),
                    Interval = TimeSpan.FromSeconds(30),
                    HTTP = new Uri(addr,"healthCheck").OriginalString
                };
                var registration = new AgentServiceRegistration
                {
                    Checks = new[] { httpCheck },
                    Address = addr.Host,
                    ID = serverId,
                    Name = ServiceOptions.Value.ServiceName,
                    Port = addr.Port,
                    Tags=new[] {"user_api"}
                    

                };
               var result= consul.Agent.ServiceRegister(registration).IsCompleted;
               
                //applicationLifetime.ApplicationStopped.Register(() =>
                //{
                //    consul.Agent.ServiceDeregister(serverId).GetAwaiter().GetResult();
                //});
            }
        }
        /// <summary>
        /// 服务注册取消
        /// </summary>
        public void DeRegisterService(IApplicationBuilder app, IOptions<ServiceDiscoveryOptions> ServiceOptions,
            IConsulClient consul)
        {
            //获取本地服务地址
            var features = app.Properties["server.Features"] as FeatureCollection;
            var address = features.Get<IServerAddressesFeature>()
                .Addresses.Select(x => new Uri(x));
            foreach (var addr in address)
            {
                var serverId = $"{ ServiceOptions.Value.ServiceName}_{addr.Host}:{addr.Port}";
                consul.Agent.ServiceDeregister(serverId).GetAwaiter().GetResult();
            }
        }
    }
}
