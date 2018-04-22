using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Contact.API.Dtos;
using Contact.API.Models;
using Dapper;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
namespace Contact.API.Data
{
    public class MysqlContactRepository : IContaclRepository
    {

        private readonly MySqlConnection conn;

        public MysqlContactRepository(IOptions<AppSetting> options)
        {
            conn = new MySqlConnection(options.Value.MySqlConnectionString);
        }
        /// <summary>
        /// 添加用户好友信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="baseUserInfo"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<bool> AddContactAsync(int userId, BaseUserInfo baseUserInfo, CancellationToken token)
        {
            var sql = "intser into Contact values(@UserId,@ContactId,@Each)";
            var result= await  conn.ExecuteAsync(sql, new { UserId = userId, ContactId = baseUserInfo.UserId });
            return result > 0;
        }

        /// <summary>
        /// 获取用户所有好友信息
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public async Task<List<Models.Contact>> GetContactsAsync(int userid)
        {
            var sql = "select a.contactid userid, b.Name name, b.Company,b.title,b.Avatar " +
                "from beta_user.Contact a," +
                "beta_user.Users b where a.userid = @UserId" +
                " and b.id = a.contactId";
            var contacts= await    conn.QueryAsync<Models.Contact>(sql, new { UserId = userid });
            if (contacts != null)
            {
                return contacts.ToList();
            }
            return new List<Models.Contact>();
        }

        /// <summary>
        /// 给好友打标签
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="contactId"></param>
        /// <param name="tags"></param>
        /// <returns></returns>
        public async Task<bool> TagsContactAsync(int userId, int contactId, List<string> tags)
        {
            var sql = "update beta.Contact  set tasg=@Tags where userId=@UserId and contactId=@ContactId";
            var contactTags = "";
            tags.ForEach((x) =>
            {
                contactTags += x + ",";
            });
            var result= await conn.ExecuteAsync(sql, new { UserId = userId, ContactId = contactId, Tags = contactTags });
            return result > 0;
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
