using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contact.API.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Contact.API.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly AppSetting appSetting;
        private readonly IUserService userService;
        public ValuesController(IOptions<AppSetting> _appSetting, IUserService _userService)
        {
            appSetting = _appSetting.Value;
            userService = _userService;
        }
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        [Route("get-userInfo")]
        public async  Task<IActionResult> GetUserInfo(int id)
        {
            var result=await userService.GetBaseUserInfoAsync(id);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest();
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
