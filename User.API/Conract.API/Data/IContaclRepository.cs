using Contact.API.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Contact.API.Data
{
   public  interface IContaclRepository
    {   
        /// <summary>
        /// 更新用户好友信息
        /// </summary>
        /// <param name="baseUserInfo"></param>
        /// <returns></returns>
        Task<bool> UpdateContactInfoAsync(BaseUserInfo baseUserInfo, CancellationToken token);
        /// <summary>
        /// 添加联系人信息
        /// </summary>
        /// <param name="baseUserInfo"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<bool> AddContactAsync(int userId, BaseUserInfo baseUserInfo, CancellationToken token);
    }
}
