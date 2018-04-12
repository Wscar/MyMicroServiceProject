using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace Gateway.API
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var authenticationProviderKey = "finbook";
            //var options = o =>
            //{
            //    o.Authority = "http://localhost:56647";
            //    o.ApiName = "api";
            //    o.SupportedTokens = SupportedTokens.Both;
            //    o.ApiSecret = "secret";
            //};

            services.AddAuthentication()
                .AddIdentityServerAuthentication(authenticationProviderKey,options=> {
                    options.Authority= "http://localhost:56647";
                    options.ApiName = "gateway_api";
                    options.SupportedTokens = IdentityServer4.AccessTokenValidation.SupportedTokens.Both;
                    options.ApiSecret = "yemobai";
                    options.RequireHttpsMetadata = false;
                    

                });


            //把ocelot添加到依赖注入中
            services.AddOcelot();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //把ocelot添加到管道之中
            app.UseOcelot();
            
        }
    }
}
