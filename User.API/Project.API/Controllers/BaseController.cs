using Microsoft.AspNetCore.Mvc;
using Project.API.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.API.Controllers
{
    public class BaseController : Controller
    {
        protected UserIdentity UserIdentity
        {
            get
            {
                var userIdentity = new UserIdentity();

                userIdentity.UserId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "sub").Value);
                userIdentity.Name = User.Claims.FirstOrDefault(x => x.Type == "Name").Value;
                userIdentity.Company = User.Claims.FirstOrDefault(x => x.Type == "Company").Value;
                userIdentity.Title = User.Claims.FirstOrDefault(x => x.Type == "Title").Value;
                userIdentity.Avatar = User.Claims.FirstOrDefault(x => x.Type == "Avatar").Value;
                return userIdentity;
            }




        }
    }
}
