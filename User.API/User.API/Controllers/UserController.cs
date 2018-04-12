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
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace User.API.Controllers
{
    [Route("api/users")]
    public class UserController : BaseController
    {


        private readonly UserContext userContext;
        private readonly ILogger<UserController> logger;
        public UserController (UserContext _userContext, ILogger<UserController> _logger)
        {
            userContext = _userContext;
            logger = _logger;
        }

        [Route("")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
          var user=  await userContext.Users.AsNoTracking().Include(x=>x.UserProperties).
                SingleOrDefaultAsync(x => x.Id == UserIdentity.UserId);
            if (user == null)
            {
                var ex= new UserOperationException($"错误的用户Id{UserIdentity.UserId}");
                throw ex;
            }
            return Json(user);
        }
        
        [Route("")]
        [HttpPatch]
        public async Task<IActionResult> Pacth([FromBody]JsonPatchDocument<Models.APPUser> patch)
        {
            var users =  await userContext.Users.SingleOrDefaultAsync(x => x.Id == UserIdentity.UserId);
          
            //foreach (var prop in users.UserProperties)
            //{  
            //    userContext.Entry(prop).State = EntityState.Detached;   //不跟踪
            //}
            patch.ApplyTo(users);        
            //原先有的userProperties信息
            //AsNoTracking()不进行自动映射
            var originProperties = await userContext.UserProperties.AsNoTracking().Where(x => x.APPUserId == UserIdentity.UserId).AsNoTracking().ToListAsync();
            //做并集 
            var allProperties = originProperties.Union(users.UserProperties,new Models.UserPropertiesComparer()).Distinct();
            //用原来的userProperties与新来的uersProperties求差集，找出要移除的元素
            var removedProperties = originProperties.Except(users.UserProperties, new Models.UserPropertiesComparer());
            //用并集与原有的userProperties求差集，找出新来的元素.
            var newProperties = allProperties.Except(originProperties, new Models.UserPropertiesComparer());                                
            foreach(var prop in removedProperties)
            {
                userContext.UserProperties.Remove(prop);
            }
            //删除原来的UserProperties
            var deleteUserProperties = originProperties;
            foreach (var prop in deleteUserProperties)
            {
                deleteUserProperties.Remove(prop);
            }
            foreach (var prop in newProperties)
            {
                userContext.UserProperties.Add(prop);
            }
            userContext.Users.Update(users);
            userContext.SaveChanges();
            return Json(users);
        }
        [Route("check-or-create")]
        [HttpPost]
        public async Task<IActionResult> CheckOrCreat(string phone)
        {
            //验证手机号码的格式

            if (phone != "123456789")
            {
                throw new HttpRequestException("不让你访问");
            }
            var user = await userContext.Users.SingleOrDefaultAsync(x => x.Phone == phone);
            //如果不存在用户，就创建一个用户
            if(user==null)
            {
                user = new Models.APPUser { Phone = phone ,CityId=1,Gender=1};
                Models.APPUser newAppUser = new Models.APPUser();
                EntityEntry < Models.APPUser > entry  =   userContext.Users.Add(user);
                userContext.SaveChanges();
                //获取刚刚创建的user
              user=  userContext.Users.Where(x => x.Phone == phone).FirstOrDefault();
            }
            return Ok(user.Id);
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
