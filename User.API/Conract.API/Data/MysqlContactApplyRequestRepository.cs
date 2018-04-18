using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Contact.API.Models;

namespace Contact.API.Data
{
    public class MysqlContactApplyRequestRepository : IContactApplyRequestRespository
    {
        public Task<bool> AddRequestAsync(ContactApllyRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ApprovalAsync(int userId, int applierId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<List<ContactApllyRequest>> GetRequestListAsync(int userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
