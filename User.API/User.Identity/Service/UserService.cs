using Consul;
using DnsClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Resilience;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace User.Identity.Service
{
    public class UserService : IUserServices
    {
        private string UserServiceUrl;
        public IHttpClient httpClient;
        private readonly ILogger<UserService> logger;
        public UserService(IHttpClient _httpClient ,IDnsQuery dnsQuery,IOptions<Dtos.ServiceDiscoveryOptions> options, ILogger<UserService> _logger)
        {
            httpClient = _httpClient;
            GetServiceUrl(options);
            logger = _logger;
            //var address= dnsQuery.ResolveService("", options.Value.UserServiceName);
            //var host = address.First().AddressList.FirstOrDefault();
            //var port = address.First().Port;
            //UserServiceUrl = $"http://{host}:{port}";
        }
        public async Task<int> CheckOrCreate(string phone)
        {
            try
            {
                var form = new Dictionary<string, string>();
                form.Add("phone", phone);
                var content = new FormUrlEncodedContent(form);
                var response = await httpClient.PostAsync(UserServiceUrl + "/api/users/check-or-create",form);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var userId = await response.Content.ReadAsStringAsync();
                    int.TryParse(userId, out int intUserId);
                    return intUserId;
                }
            }
            catch (Exception ex)
            {
                logger.LogError("CheckOrCreate在重试之后失败，" + ex.Message + "," + ex.StackTrace);
            }
           
            
            return 0;
        }
        private void GetServiceUrl(IOptions<Dtos.ServiceDiscoveryOptions> options)
        {
            List<Uri> _serverUrls = new List<Uri>();
            var consulClient = new ConsulClient(p => { p.Address = new Uri(options.Value.Consul.HttpEndpoint); });
            var services = consulClient.Agent.Services().Result.Response;
            foreach(var service in services)
            {
                var isUserName = service.Value.Tags.Any(x => x == "user_api");
                if (isUserName)
                {
                    UserServiceUrl = $"http://{service.Value.Address}:{service.Value.Port}";
                }
            }
        }
        
    }
}
