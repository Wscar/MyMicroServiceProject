using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Contact.API.Controllers;
using Contact.API.Data;
using Contact.API.Service;
using Microsoft.Extensions.Options;
using Contact.API;
using Moq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.TestHost;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using System.Linq;
namespace ContactAPiTest
{
    public class ContactControllerTest
    {

        IOptions<AppSetting> options;
        private IContactApplyRequestRespository requestRespository;
        private IUserService userService;
        private IContactRepository contactRepository;
        private IServiceCollection services;
        private ServiceProvider serviceProvider;
        private readonly TestServer _server;
        private readonly HttpClient _clien;
        public ContactControllerTest()
        {
            _server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
           // _server = new WebHostBuilderTest.HostBuilder().contactApiServer;
            _clien = _server.CreateClient();
            this.GetDIObject();
        }
        private ContactController GetContactController()
        {   
            //实例化字段
           
            var controller = new ContactController(requestRespository, userService, contactRepository);
            
           
            return controller;
        }

        /// <summary>
        /// 获取配置文件
        /// </summary>
        /// <returns></returns>
        private IConfiguration GetConfiguration()
        {
            var config = new ConfigurationBuilder()
                 .AddJsonFile("appsetting.json").Build();
            return config;
        }

        void CreateServiceCollection()
        {
            services = new ServiceCollection();
            serviceProvider = services.Configure<AppSetting>(this.GetConfiguration().GetSection("DBSetting"))
            .AddOptions()
            .AddScoped<IContactApplyRequestRespository, MysqlContactApplyRequestRepository>()
            .AddScoped<IUserService, UserService>()
            .AddScoped<IContactRepository, MysqlContactRepository>()
            .BuildServiceProvider();
        }
       
        /// <summary>
        /// 获取DI容器的中的实例对象
        /// </summary>
        void GetDIObject()
        {
            this.CreateServiceCollection();
            requestRespository = serviceProvider.GetRequiredService(typeof(IContactApplyRequestRespository)) as IContactApplyRequestRespository;
            userService = serviceProvider.GetRequiredService<IUserService>();
            contactRepository = serviceProvider.GetRequiredService<IContactRepository>();
        }

        //[Fact]
        //async void GetValues()
        //{
        //   var response= await _clien.GetAsync("api/values");
        //    response.EnsureSuccessStatusCode();
        //    var responseString = await response.Content.ReadAsStringAsync();
           
        //}
         /// <summary>
         /// 获取联系人列表
         /// </summary>
          [Fact]
          async void Get_ReturnRightContact_WithExpectedParamerters()
        {
           var contacts=  await contactRepository.GetContactsAsync(1);
            var contact = contacts[0];
            Assert.Equal(2, contact.UserId);
        }
        /// <summary>
        /// 更新用户标签
        /// </summary>
        [Fact]
        async void Update_ReturnRightContactTags_WithExpectedParamerters()
        {
            var result = await contactRepository.TagsContactAsync(1, 2, new List<string> { "大神,极客" });
            Assert.True(result);
        }
        /// <summary>
        /// 添加用户联系人
        /// </summary>
        [Fact]       
        async void Add_ReturnRightContact_WithExceptedParamerters()
        {
            var result = await contactRepository.AddContactAsync(1, new Contact.API.Dtos.BaseUserInfo { UserId = 3 }, new System.Threading.CancellationToken() );
            Assert.True(result);
        }
    }
}
