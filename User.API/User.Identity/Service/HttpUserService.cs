using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User.Identity.Service
{
    public class HttpUserService : IUserServices
    {
        public Task<int> CheckOrCreate(string phone)
        {
            throw new NotImplementedException();
        }
    }
}
