using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contact.API.Dtos;
using Dapper;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Contact.API.Service
{
    public class UserService : IUserService
    {
        private readonly MySql.Data.MySqlClient.MySqlConnection conn;
        public UserService(IOptions<AppSetting> options)
        {
            conn = new MySql.Data.MySqlClient.MySqlConnection(options.Value.MySqlConnectionString);
        }
        public async Task<BaseUserInfo> GetBaseUserInfoAsync(int userId)
        {
            string sql = "select a.Id userId,a.name,a.company,a.title,a.avatar from beta_user.Users a where a.id=@UserId";
            var userInfo = await conn.QueryAsync<BaseUserInfo>(sql, new { UserId = userId });
            return userInfo.First();
        }
    }
}
