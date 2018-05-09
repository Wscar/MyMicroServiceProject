using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Contact.API.Models;
using Microsoft.Extensions.Options;
using MySqlClient=MySql.Data.MySqlClient;
using Dapper;

namespace Contact.API.Data
{
    public class MysqlContactApplyRequestRepository : IContactApplyRequestRespository
    {
        private readonly MySql.Data.MySqlClient.MySqlConnection conn;
        public MysqlContactApplyRequestRepository(IOptions<AppSetting> options)
        {
            conn = new MySqlClient.MySqlConnection(options.Value.MySqlConnectionString);
      
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
        /// <summary>
        /// 通过还有申请
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="applierId"></param>
        /// <param name="isApproval"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async  Task<bool> ApprovalAsync(int userId, int applierId, string isApproval, CancellationToken cancellationToken)
        {   
            string  approval=isApproval=="Y"?"0":"1"; //0申请通过,1申请不通过
            //好友申请通过就更变申请表中approval
            var sql = "update ContactApplyRequest set approval=@Approval where userId=@UserId and applierId=@ApplierId";
            int result= await conn.ExecuteAsync(sql, new { UserId = userId, ApplierId = applierId ,Approval=approval});
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
