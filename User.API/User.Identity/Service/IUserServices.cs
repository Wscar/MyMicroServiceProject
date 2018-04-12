using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User.Identity.Service
{
   public interface IUserServices
    {
        /// <summary>
        /// 检查手机号，是否被注册，没有注册的话就注册一个用户
        /// </summary>
        /// <param name="phone">电话号码</param>
        /// <returns>返回用户id</returns>
        Task<int> CheckOrCreate(string phone);
    }
}
