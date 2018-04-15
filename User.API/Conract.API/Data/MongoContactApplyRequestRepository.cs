﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contact.API.Models;

namespace Contact.API.Data
{
    public class MongoContactApplyRequestRepository : IContactApplyRequestRespository
    {
        private readonly ContactContext contactContext;
        public MongoContactApplyRequestRepository(ContactContext _contactContext)
        {
            contactContext = _contactContext;
        }

        public Task<bool> AddRequestAsync(ContactApllyRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ApprovalAsync(int applierId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetRequestListAsync(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
