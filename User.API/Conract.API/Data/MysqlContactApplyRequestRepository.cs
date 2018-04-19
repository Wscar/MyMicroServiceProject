using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Contact.API.Models;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using Dapper;
namespace Contact.API.Data
{
    public class MysqlContactApplyRequestRepository : IContactApplyRequestRespository
    {
        private readonly MySqlConnection conn;
        public MysqlContactApplyRequestRepository(IOptions<AppSetting> options)
        {
            conn = new MySqlConnection(options.Value.MySqlConnectionString);
        }
        public async Task<bool> AddRequestAsync(ContactApllyRequest request, CancellationToken cancellationToken)
        {
            var result= await conn.ExecuteAsync("insert into ContactApplyRequset" +
                "(userid,applierId,approval,handleTime,applyTime)values(@)userId,@applierId,@approval,@handleTime,@applyTime",
                 request
                );
            //添加好友申请信息到Mysql数据库
            return result>0;
        }

        public async  Task<bool> ApprovalAsync(int userId, int applierId, CancellationToken cancellationToken)
        {
            //好友申请通过就更变申请表中approval
            var sql = "update ContactApplyRequest set approval=0 where userId=@UserId and applierId=@ApplierId";
            int result= await conn.ExecuteAsync(sql, new { UserId = userId, ApplierId = applierId });
            return result > 0;
        }

        public async Task<List<ContactApllyRequest>> GetRequestListAsync(int userId, CancellationToken cancellationToken)
        {
            //查询用户的好友信息
            var sql = "select * from ContactApplyRequest a where a.userId=@UserId ";
            var applyRequests = await conn.QueryAsync<ContactApllyRequest>(sql, new { UserId = userId });
            return applyRequests.ToList();
        }
    }
}
