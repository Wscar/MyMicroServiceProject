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

namespace ContactAPiTest
{
   public class ContactControllerTest
    {
        IOptions<AppSetting> options;
        private IContactApplyRequestRespository requestRespository;
        private IUserService userService;
        private IContaclRepository contaclRepository;
        private ContactController GetContactController()
        {
            var controller = new ContactController(requestRespository, userService, contaclRepository);
            return controller;
        }
        private IConfiguration GetConfiguration()
        {  
            var config = new ConfigurationBuilder()              
                 .AddJsonFile("appsetting.json").Build();
            return config;
        }
        [Fact]
        void GteIOptions()
        {   
            
            var optionsMoq = new Mock<IOptions<AppSetting>>();
            options = optionsMoq.Object;
            var connectionStr = options.Value.MySqlConnectionString;
        }
         [Fact]
        void GetAppSettingValue()
        {  //创建DI注入容器
            var service = new ServiceCollection()
            .Configure<AppSetting>(this.GetConfiguration().GetSection("DBSetting"))
             .AddOptions()
            .BuildServiceProvider();
            //获取要注入的示例
            var appSetting = service.GetRequiredService<IOptions<AppSetting>>();
        }
    }
}
