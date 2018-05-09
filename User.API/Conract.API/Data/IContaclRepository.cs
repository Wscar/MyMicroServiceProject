using Contact.API.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Contact.API.Models;
namespace Contact.API.Data
{
   public  interface IContactRepository
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

        /// <summary>
        /// 获取联系人列表
        /// </summary>
        /// <param name="userid">用户Id</param>
        /// <returns></returns>
        Task<List<Models.Contact>> GetContactsAsync(int userid);
        /// <summary>
        /// 给好友打标签
        /// </summary>
        /// <param name="tags"></param>
        /// <returns></returns>
        Task<bool> TagsContactAsync( int userId,int contactId,List<string> tags);
    }
}
