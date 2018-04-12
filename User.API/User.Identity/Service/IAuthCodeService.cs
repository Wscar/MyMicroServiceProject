using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User.Identity.Service
{
   public interface IAuthCodeService
    {   
        /// <summary>
        /// 根据手机号去验证
        /// </summary>
        /// <param name="code">手机号</param>
        /// <param name="authCode">验证码</param>
        /// <returns></returns>
        bool Validate(string code, string authCode);
    }
}
