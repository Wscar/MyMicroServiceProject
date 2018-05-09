using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using User.API.Data;
using Microsoft.AspNetCore.JsonPatch;
using System.Collections;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Net.Http;
using User.API.Models;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Authorization;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace User.API.Controllers
{
    [Route("api/users")]
    public class UserController : BaseController
    {


        private readonly UserContext userContext;
        private readonly ILogger<UserController> logger;
        private ICapPublisher capPublisher;
        public UserController(UserContext _userContext, ILogger<UserController> _logger, ICapPublisher _capPublisher)
        {
            userContext = _userContext;
            capPublisher = _capPublisher;
            logger = _logger;
        }
        private void RaiseUserProfileChangeEvent(APPUser appUser)
        {   //判断用户的信息是否修改了
            if (userContext.Entry(appUser).Property(nameof(appUser.Name)).IsModified ||
                userContext.Entry(appUser).Property(nameof(appUser.Title)).IsModified ||
                userContext.Entry(appUser).Property(nameof(appUser.Company)).IsModified ||
                userContext.Entry(appUser).Property(nameof(appUser.Avatar)).IsModified)
            {
                capPublisher.Publish("userapi.userProfileChangeEvent", new Dtos.UserIdentity
                {
                    UserId = appUser.Id,
                    Avatar=appUser.Avatar,
                    Name=appUser.Name,
                    Company=appUser.Company,
                    Title=appUser.Title
                });
            }
        }
        [Authorize]
        [Route("")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var user = await userContext.Users.AsNoTracking().Include(x => x.UserProperties).
                  SingleOrDefaultAsync(x => x.Id == UserIdentity.UserId);
            if (user == null)
            {
                var ex = new UserOperationException($"错误的用户Id{UserIdentity.UserId}");
                throw ex;
            }
            return Json(user);
        }

        [Route("")]
        [HttpPatch]
        public async Task<IActionResult> Pacth([FromBody]JsonPatchDocument<Models.APPUser> patch)
        {
            var users = await userContext.Users.SingleOrDefaultAsync(x => x.Id == UserIdentity.UserId);

            patch.ApplyTo(users);
            //删除原来的userProperties信息
            var userProperties = userContext.UserProperties.Where(x => x.APPUserId == users.Id).ToList();
            userContext.UserProperties.RemoveRange(userProperties);
            var newUser = users;
            using (var transaction = userContext.Database.BeginTransaction())
            {
                this.RaiseUserProfileChangeEvent(newUser);
                userContext.Users.Update(newUser);
                userContext.SaveChanges();
                //发布用户变更的消息。               
                transaction.Commit();
            }
            var userInfo = await userContext.Users.AsNoTracking().Include(x => x.UserProperties).SingleOrDefaultAsync(x => x.Id == UserIdentity.UserId);
            return Json(userInfo);
        }
        [Route("check-or-create")]
        [HttpPost]
        public async Task<IActionResult> CheckOrCreat(string phone)
        {
            //验证手机号码的格式


            var user = await userContext.Users.SingleOrDefaultAsync(x => x.Phone == phone);
            //如果不存在用户，就创建一个用户
            if (user == null)
            {
                user = new APPUser { Phone = phone, CityId = 1, Gender = 1 };
                APPUser newAppUser = new APPUser();
                EntityEntry<APPUser> entry = userContext.Users.Add(user);
                userContext.SaveChanges();
                //获取刚刚创建的user
                user = userContext.Users.Where(x => x.Phone == phone).FirstOrDefault();
            }
            return Ok(new { user.Id, user.Name, user.Company, user.Title, user.Avatar });
        }
        [HttpGet]
        [Route("tags")]
        /// <summary>
        /// 获取用户标签选项数据
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> GetUserTags()
        {
            return Ok(await userContext.UserTag.Where(x => x.UserId == UserIdentity.UserId).ToListAsync());
        }
        [HttpPost]
        [Route("serach")]
        /// <summary>
        /// 根据手机查找搜索用户
        /// </summary>
        /// <param name="phone">手机号</param>
        /// <returns></returns>
        public async Task<IActionResult> Search(string phone)
        {
            return Ok(await userContext.Users.Include(x => x.UserProperties).SingleOrDefaultAsync(x => x.Phone == phone));
        }
        [HttpPut]
        [Route("update-user-tags")]
        /// <summary>
        /// 更新用户标签
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> UpdateUserTags([FromBody] List<string> Tags)
        {
            //删除当前用户的标签，
            var userId = UserIdentity.UserId;
            var userTags = await userContext.UserTag.Where(x => x.UserId == userId).ToListAsync();
            if (userTags != null || userTags.Count != 0)
            {
                userContext.UserTag.RemoveRange(userTags);
                await userContext.SaveChangesAsync();
            }
            List<UserTag> newUserTags = new List<UserTag>();
            Tags.ForEach((x) =>
            {
                var userTag = new UserTag { UserId = userId, Tag = x, CreateTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")) };
                newUserTags.Add(userTag);
            });
            userContext.UserTag.AddRange(newUserTags);
            await userContext.SaveChangesAsync();
            return Ok(newUserTags);
        }
        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
