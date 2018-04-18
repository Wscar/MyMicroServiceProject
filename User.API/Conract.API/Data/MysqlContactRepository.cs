using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Contact.API.Dtos;
using Contact.API.Models;

namespace Contact.API.Data
{
    public class MysqlContactRepository : IContaclRepository
    {    
        /// <summary>
        /// 添加用户好友信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="baseUserInfo"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<bool> AddContactAsync(int userId, BaseUserInfo baseUserInfo, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取用户所有好友信息
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public Task<List<Models.Contact>> GetContactsAsync(int userid)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 给好友打标签
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="contactId"></param>
        /// <param name="tags"></param>
        /// <returns></returns>
        public Task<bool> TagsContactAsync(int userId, int contactId, List<string> tags)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 更新用户好友信息
        /// </summary>
        /// <param name="baseUserInfo"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<bool> UpdateContactInfoAsync(BaseUserInfo baseUserInfo, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
