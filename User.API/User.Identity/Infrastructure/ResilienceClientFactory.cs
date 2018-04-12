using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Resilience;
using Polly;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.Net.Http;

namespace User.Identity.Infrastructure
{
    public class ResilienceClientFactory
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<ResilienceHttpClient> logger;
        private readonly IHttpContextAccessor httpContextAccessor;
        
        /// <summary>
        /// 重试的次数
        /// </summary>
        private int retryCount;
        /// <summary>
        /// 熔断之前异常的次数
        /// </summary>

        private int exceptionAllowBeforeBreaking;
        public ResilienceClientFactory(ILogger<ResilienceHttpClient> _logger,
            IHttpContextAccessor _httpContextAccesor,
            int _retryCount,
            int _exceptionAllowBeforeBreaking)
        {
            logger = _logger;
            httpContextAccessor = _httpContextAccesor;
            this.retryCount = _retryCount;
            this.exceptionAllowBeforeBreaking = _exceptionAllowBeforeBreaking;
        }
        public ResilienceHttpClient GetResilienceHttpClient() =>
             new ResilienceHttpClient(origin => this.CreatePolicy(origin), logger, httpContextAccessor);

        /// <summary>
        /// 创建Policy
        /// </summary>
        /// <returns></returns>
        private Policy[] CreatePolicy(string origin)
        {
            return new Policy[]
            {
                Policy.Handle<HttpRequestException>()
                //重试
                .WaitAndRetryAsync(retryCount,retryAttemp=>TimeSpan.FromSeconds( Math.Pow(2,retryAttemp)),(exception,timeSpan,retryCount,context)=>{
                    var msg=$"第{retryCount}次重试"+
                    $"of{context.PolicyKey}"+
                    $"at{context.ExecutionKey},"+
                    $"due to{exception}.";
                   logger.LogWarning(msg);
                    logger.LogDebug(msg);
                }),
                //熔断
                Policy.Handle<HttpRequestException>()
                .CircuitBreakerAsync(exceptionAllowBeforeBreaking,
                TimeSpan.FromMinutes(1),
                ((exception,duration)=>{ logger.LogWarning("熔断器打开");
                    logger.LogDebug("熔断器打开");
                }),
                (()=>{
                  
                 logger.LogWarning("熔断气关闭");
                 logger.LogDebug("熔断器关闭");}))
            };
        }
    }
}
