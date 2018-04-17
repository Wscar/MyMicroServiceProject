using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Contact.API.Models;
namespace Contact.API.Data
{
   public interface IContactApplyRequestRespository
    {
        /// <summary>
        /// 添加申请好友的请求
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<bool> AddRequestAsync(ContactApllyRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// 通过好友申请
        /// </summary>
        /// <param name="applierId"></param>
        /// <returns></returns>
        Task<bool> ApprovalAsync(int userId,int applierId, CancellationToken cancellationToken);

        /// <summary>
        /// 好友申请列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<ContactApllyRequest>> GetRequestListAsync(int userId, CancellationToken cancellationToken);
    }
}
