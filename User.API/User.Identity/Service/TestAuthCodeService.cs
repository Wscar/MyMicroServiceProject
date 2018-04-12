using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User.Identity.Service
{
    public class TestAuthCodeService : IAuthCodeService
    {
        public bool Validate(string code, string authCode)
        {
            return true;
        }
    }
}
