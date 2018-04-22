using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using User.Identity.Dtos;

namespace User.Identity.Service
{
    public class HttpUserService : IUserServices
    {
        public Task<UserInfo> CheckOrCreate(string phone)
        {
            throw new NotImplementedException();
        }
    }
}
