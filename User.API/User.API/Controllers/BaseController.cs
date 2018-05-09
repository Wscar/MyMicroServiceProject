using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using User.API.Dtos;
namespace User.API.Controllers
{
    public class BaseController:Controller
    {
        protected UserIdentity UserIdentity {
            get {
                UserIdentity identity = new UserIdentity();

                identity.UserId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "sub").Value);
                identity.Name = User.Claims.FirstOrDefault(x => x.Type == "Name").Value;
                identity.Company = User.Claims.FirstOrDefault(x => x.Type == "Company").Value;
                identity.Title = User.Claims.FirstOrDefault(x => x.Type == "Title").Value;
                identity.Avatar = User.Claims.FirstOrDefault(x => x.Type == "Avatar").Value;
                return identity;

            } }
    }
}
