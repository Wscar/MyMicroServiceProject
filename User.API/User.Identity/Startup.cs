using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DnsClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Resilience;
using User.Identity.Dtos;
using User.Identity.Infrastructure;
using User.Identity.Service;

namespace User.Identity
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        { //.AddDeveloperSigningCredential()
            services.AddIdentityServer()
                  .AddExtensionGrantValidator<Authentication.SmaAuthCodeValidaor>()
                 .AddDeveloperSigningCredential()
                 .AddInMemoryClients(Config.GetClients())
                 .AddInMemoryIdentityResources(Config.GetIdentityResource())
                 .AddInMemoryApiResources(Config.GetApiResource());
            services.AddScoped<IAuthCodeService, TestAuthCodeService>()
                .AddScoped<IUserServices, UserService>();
            services.AddSingleton(new System.Net.Http.HttpClient());
            //获取配置文件中的服务发现
            services.Configure<ServiceDiscoveryOptions>(Configuration.GetSection("ServiceDiscovery"));
            services.AddSingleton<IDnsQuery>(p =>
            {
                var serviceConfigation = p.GetRequiredService<IOptions<ServiceDiscoveryOptions>>().Value;
                return new LookupClient(serviceConfigation.Consul.DnsEndpoint.ToIPEndPoint());
            });
            //注册全局唯一单例ResilienceClientFactory
            services.AddSingleton(typeof(ResilienceClientFactory), sp =>
            {
                var logger = sp.GetRequiredService<ILogger<ResilienceHttpClient>>();
                var httpClinetAccessor = sp.GetRequiredService<IHttpContextAccessor>();
                var retryCount = 3;
                var exceptionCountAllowBeforeBreaker = 3;
                var factory = new ResilienceClientFactory(logger,httpClinetAccessor,retryCount,exceptionCountAllowBeforeBreaker);
                return factory;
            });
            //注册全局唯一单例IHttpClient;
            services.AddSingleton<IHttpClient>(sp =>
            {
                return sp.GetRequiredService<ResilienceClientFactory>().GetResilienceHttpClient();
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
                   
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            app.UseIdentityServer();
            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}
