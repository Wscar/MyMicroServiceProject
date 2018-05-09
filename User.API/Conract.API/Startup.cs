using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Contact.API.Data;
using Contact.API.Service;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Contact.API.Event;
using Contact.API.Entities;
using Consul;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Hosting.Server.Features;

namespace Contact.API
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
            services.Configure<AppSetting>(Configuration.GetSection("DBSetting"));
            services.AddScoped<IContactApplyRequestRespository, MysqlContactApplyRequestRepository>();
            services.AddScoped<IContactRepository, MysqlContactRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddTransient<UserProfileChangeEventHandle>();
            services.Configure<ServiceDiscoveryOptions>(Configuration.GetSection("ServiceDiscovery"));
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            //添加认证服务
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.Audience = "contact_api";
                    options.Authority = "http://localhost";
                    options.SaveToken = true;
                });
            //使用CAP
            services.AddCap(optinos =>
            {
                optinos.UseMySql(Configuration.GetSection("DBSetting").GetValue<string>("MySqlConnectionString"))

                .UseDashboard()
                .UseDiscovery(x => {
                x.DiscoveryServerHostName = "localhost";
                x.DiscoveryServerPort = 8500;
                x.CurrentNodeHostName = "localhost";
                x.CurrentNodePort = 62077;
                x.NodeId = 2;
                x.NodeName = "CAP No2 Node";
                })
                .UseRabbitMQ(mqOptions => {
                    mqOptions.HostName = "47.93.232.105";
                    mqOptions.Port = 5672;
                    mqOptions.UserName = "admin";
                    mqOptions.Password = "wqawd520";

                });

            });
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication();
            app.UseCap();
            app.UseMvc();

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
                    HTTP = new Uri(addr, "healthCheck").OriginalString
                };
                var registration = new AgentServiceRegistration
                {
                    Checks = new[] { httpCheck },
                    Address = addr.Host,
                    ID = serverId,
                    Name = ServiceOptions.Value.ServiceName,
                    Port = addr.Port,
                    Tags = new[] { "user_api" }


                };
                var result = consul.Agent.ServiceRegister(registration).IsCompleted;

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
