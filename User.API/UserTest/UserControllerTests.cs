using System;
using Xunit;
using User.API;
using User.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using User.API.Controllers;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;
using User.API.Models;
using System.Linq;

namespace UserTest
{
    public class UserControllerTests
    {   
        private UserContext GetUserContext()
        {
            var options = new DbContextOptionsBuilder<UserContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var userContext = new UserContext(options);
            userContext.Users.Add(new User.API.Models.APPUser
            {
                Id = 1,
                Name = "ҹĪ��"
            });
            userContext.SaveChanges();
            return userContext;
        }
        private (UserController controller,UserContext context )GetUserController()
        {    
            //ʹ��Ԫ�鷵��2��ֵ
            var context = GetUserContext();
            var loggingMoq = new Moq.Mock<ILogger<UserController>>();
            var logger = loggingMoq.Object;
            var controller = new User.API.Controllers.UserController(context, logger);
            return (controller: controller,context:context);
        }
        [Fact]
        public async void Get_ReturnRightUser_WithExpectedParamerters()
        {
            (UserController controller,UserContext context) = GetUserController();
            var response = await controller.Get();
            //��ͨ�Ĳ��Է���
            // Assert.IsType<JsonResult>(response);
            //Fluent����
            //�õ�����ֵ
           var result= response.Should().BeOfType<JsonResult>().Subject;
            //�õ�appUser
            var appUser = result.Value.Should().BeAssignableTo<User.API.Models.APPUser>().Subject;
            //��֤appuser�е�ֵ
            appUser.Id.Should().Be(1);
            appUser.Name.Should().Be("ҹĪ��");
        }
        [Fact]
        public async void Patch_ReturnNewName_WithExpectedNewNameParamerters()
        {
            (UserController controller, UserContext context) = GetUserController();
            var document = new JsonPatchDocument<User.API.Models.APPUser>();
            document.Replace(u => u.Name, "Jolly");

            var  response = await controller.Pacth(document);

            //������֤
            var result = response.Should().BeOfType<JsonResult>().Subject;
            var appUser = result.Value.Should().BeAssignableTo<User.API.Models.APPUser>().Subject;
            appUser.Name.Should().Be("Jolly");
            var userModel=await context.Users.SingleOrDefaultAsync(x => x.Id==1);
            userModel.Should().NotBeNull();
            userModel.Name.Should().Be("Jolly");
        }

        [Fact]
        public async void Patch_ReturnNewProperties_WithAddNewProperties()
        {
            (UserController controller, UserContext context) = GetUserController();
            var document = new JsonPatchDocument<User.API.Models.APPUser>();
            document.Replace(u => u.UserProperties, new List<User.API.Models.UserProperty> {
                new UserProperty{Key="fin_industry",Value="������",Text="������"}

            });

            var response = await controller.Pacth(document);

            //���з���ֵ��֤
            var result = response.Should().BeOfType<JsonResult>().Subject;
            var appUser = result.Value.Should().BeAssignableTo<User.API.Models.APPUser>().Subject;
            appUser.UserProperties.Count.Should().Be(1);
            appUser.UserProperties.First().Value.Should().Be("������");
            appUser.UserProperties.First().Key.Should().Be("fin_industry");
            //���ݿ���֤
            var userModel = await context.Users.SingleOrDefaultAsync(x => x.Id == 1);
            userModel.UserProperties.Count.Should().Be(1);
            userModel.UserProperties.First().Value.Should().Be("������");
            userModel.UserProperties.First().Key.Should().Be("fin_industry");
        }
        [Fact]
        public async void Patch_ReturnNewProperties_WithRemoveProperties()
        {
            (UserController controller, UserContext context) = GetUserController();
            var document = new JsonPatchDocument<User.API.Models.APPUser>();
            document.Replace(u => u.UserProperties, new List<User.API.Models.UserProperty> {
               

            });

            var response = await controller.Pacth(document);

            //���з���ֵ��֤
            var result = response.Should().BeOfType<JsonResult>().Subject;
            var appUser = result.Value.Should().BeAssignableTo<User.API.Models.APPUser>().Subject;
            //��֤�����Ƿ�Ϊ��
            appUser.UserProperties.Should().BeEmpty();
          
            //���ݿ���֤
            var userModel = await context.Users.SingleOrDefaultAsync(x => x.Id == 1);
            userModel.UserProperties.Should().BeEmpty();
        }
    }
}
