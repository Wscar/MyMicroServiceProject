using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Contact.API.Models;
using MongoDB.Driver;
namespace Contact.API.Data
{
    public class MongoContactApplyRequestRepository : IContactApplyRequestRespository
    {
        private readonly ContactContext contactContext;
        public MongoContactApplyRequestRepository(ContactContext _contactContext)
        {
            contactContext = _contactContext;
        }
        /// <summary>
        /// 添加好友申请
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<bool> AddRequestAsync(ContactApllyRequest request, CancellationToken cancellationToken)
        {
            //查找当前好友是否存在
            var filter = Builders<ContactApllyRequest>.
                Filter.Where(x => x.UserId == request.UserId && x.Applierid == request.Applierid);
            if (await contactContext.ContactApplyRequests.CountAsync(filter) > 0)
            {
                //当发现用户拥有此好友时，更新信息
                var update = Builders<ContactApllyRequest>.Update.Set(x => x.ApplyTime, DateTime.Now);
                var optinons = new UpdateOptions() { IsUpsert = true };
                var result = await contactContext.ContactApplyRequests.UpdateOneAsync(filter, update, null, cancellationToken);
                return result.MatchedCount == result.ModifiedCount;
            }
            contactContext.ContactApplyRequests.InsertOne(request);
            return true;
        }
        /// <summary>
        /// 是否通过好友申请
        /// </summary>
        /// <param name="applierId">申请人Id</param>
        /// <returns></returns>
        public async Task<bool> ApprovalAsync(int userId, int applierId, CancellationToken cancellationToken)
        {
            var filter = Builders<ContactApllyRequest>.Filter.Where(x => x.UserId == userId && x.Applierid == applierId);
            var updateFilter = Builders<ContactApllyRequest>.Update.Set(x => x.Approval, "1")
                .Set(x => x.HandleTime, DateTime.Now);
               
            var result = await contactContext.ContactApplyRequests.UpdateOneAsync(filter, updateFilter, null, cancellationToken);
            return result.MatchedCount == result.ModifiedCount && result.ModifiedCount == 1;
        }
        /// <summary>
        /// 获取好友申请列表
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="cancellationToken">是否取消线程</param>
        /// <returns></returns>
        public async Task<List<ContactApllyRequest>> GetRequestListAsync(int userId, CancellationToken cancellationToken)
        {
            var result = await contactContext.ContactApplyRequests.FindAsync(x => x.UserId == userId);
            return result.ToList(cancellationToken);
        }
    }
}
