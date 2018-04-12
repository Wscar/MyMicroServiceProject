using Polly;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Polly.Wrap;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Resilience
{
    public class ResilienceHttpClient : IHttpClient
    {
        private readonly HttpClient httpClient;
        //根据url origin去创建Policy
        private Func<string, IEnumerable<Policy>> policyCreator;
        // 把policy 打包成PolicyWrap 进行本地缓存
        private readonly ConcurrentDictionary<string, PolicyWrap> policyWraps;
        private readonly ILogger<ResilienceHttpClient> logger;
        private readonly IHttpContextAccessor httpContextAccessor;
        public ResilienceHttpClient(Func<string, IEnumerable<Policy>> _policyCreator, ILogger<ResilienceHttpClient> _logger, IHttpContextAccessor _httpContextAccessor)
        {
            policyCreator = _policyCreator;
            httpClient = new HttpClient();
            policyWraps = new ConcurrentDictionary<string, PolicyWrap>();
            logger = _logger;
            httpContextAccessor = _httpContextAccessor;
        }
        public  Task<HttpResponseMessage> PostAsync<T>(string url, T item, string authorizationToken=null, string requestId = "", string authorizationMethod = "Bearer")
        {
            HttpRequestMessage httpRequestMessageFunc() => CreateHttpRequestMessage(HttpMethod.Post, url, item);
            return  this.DoPostAsync(HttpMethod.Post,url, httpRequestMessageFunc,  authorizationToken, requestId, authorizationMethod);
        }
        public async Task<HttpResponseMessage> PostAsync(string url, Dictionary<string, string> form, string authorizationToken=null, string requestId = "", string authorizationMethod = "Bearer")
        {
            HttpRequestMessage httpRequestMessageFunc() => CreateHttpRequestMessage(HttpMethod.Post, url, form);
            return  await this.DoPostAsync(HttpMethod.Post, url, httpRequestMessageFunc, authorizationToken, requestId, authorizationMethod);
        }
     
        private  Task<HttpResponseMessage> DoPostAsync(HttpMethod method, string url,Func<HttpRequestMessage> httpRequestMessageFunc , string authorizationToken, string requestId = "", string authorizationMethod = "Bearer")
        {
            if (method != HttpMethod.Post && method != HttpMethod.Put)
            {
                throw new ArgumentException("Value must be either post or  put ", nameof(method));
            }
            var origin = this.GetOriginFromUri(url);
         
         
            return  HttpInvoker(origin, async () =>
            {
                //实例化一个httpRequsetMessage
                HttpRequestMessage requestMessage = httpRequestMessageFunc();
                if (requestMessage == null)
                {
                    logger.LogError("requestMessage为空");
                }
               
                this.SetAuthorizationHeader(requestMessage);
             
                if (authorizationToken != null)
                {   //加上tokent
                    requestMessage.Headers.Authorization = new AuthenticationHeaderValue(authorizationMethod, authorizationToken);
                }
                if (requestId != null)
                {
                    //加上请求Id
                    requestMessage.Headers.Add("x-requestid", requestId);
                }
                //发送请求
                var response = await httpClient.SendAsync(requestMessage);
                if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    throw new HttpRequestException("response.StatusCode=500");
                }
                return response;

            });
        }
        private   async Task<T> HttpInvoker<T>(string origin,Func<Task<T>> action)
        {
            var normalizedOrigin = NormalizeOrigin(origin);
            if(!policyWraps.TryGetValue(normalizedOrigin,out PolicyWrap policyWrap))
            {
                policyWrap = Policy.WrapAsync(policyCreator(normalizedOrigin).ToArray());
                policyWraps.TryAdd(normalizedOrigin, policyWrap);

            }
            return  await policyWrap.ExecuteAsync(action, new Context(normalizedOrigin)); 
        }
        private static string NormalizeOrigin(string origin)
        {    
            //去掉所有的空格，转换为小写
            return origin?.Trim()?.ToLower();
        }
        private string GetOriginFromUri(string uri)
        {
            var url = new Uri(uri);
            var origin = $"{url.Scheme}://{url.DnsSafeHost}:{url.Port}";
            return origin;
        }
        private  void SetAuthorizationHeader(HttpRequestMessage requestMessage)
        {    
            //把验证头添加进去
            var authorizationHeader = httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            if (!string.IsNullOrEmpty(authorizationHeader))
            {
                requestMessage.Headers.Add("Authorization", new List<string>() { authorizationHeader });
            }
        }
        private HttpRequestMessage CreateHttpRequestMessage<T>(HttpMethod method,string url,T item)
        {
            var requestMessage = new HttpRequestMessage(method, url) { Content = new StringContent(JsonConvert.SerializeObject(item), System.Text.Encoding.UTF8, "application/json") };
       
            return requestMessage;
        }
        private HttpRequestMessage CreateHttpRequestMessage(HttpMethod method, string url, Dictionary<String,string> form)
        {
            var requestMessage = new HttpRequestMessage(method, url)
            {
                Content = new FormUrlEncodedContent(form)
            };
            return requestMessage;
        }


    }
}
