using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Contact.API.Dtos;
using Contact.API.Models;
using MongoDB.Driver;
namespace Contact.API.Data
{
    public class MongoContactRepository : IContactRepository
    {
        private readonly ContactContext contactContext;
        public MongoContactRepository(ContactContext _contactContext)
        {
            contactContext = _contactContext;
        }

        public async Task<bool> AddContactAsync(int userId, BaseUserInfo baseUserInfo, CancellationToken token)
        {    
            if (contactContext.ContactBooks.Count(x => x.UserId == userId) == 0)
            {
                await contactContext.ContactBooks.InsertOneAsync(new ContackBook { UserId = userId });
            }
            //把当前好友信息,更新到Mongodb
            var filter = Builders<ContackBook>.Filter.Eq(x => x.UserId, userId);
            var updateFilter = Builders<ContackBook>.Update.AddToSet(x => x.Contacts,
                new Models.Contact
                {
                    UserId = baseUserInfo.UserId,
                    Avatar = baseUserInfo.Avatar,
                    Company=baseUserInfo.Company,
                    Name=baseUserInfo.Name,
                    Title=baseUserInfo.Title
                });
            var result = await contactContext.ContactBooks.UpdateOneAsync(filter, updateFilter, null, token);
            return result.MatchedCount == result.ModifiedCount&&result.ModifiedCount==1;

        }

        public async Task<List<Models.Contact>> GetContactsAsync(int userid)
        {
            var contactBook = (await contactContext.ContactBooks.FindAsync(x => x.UserId == userid)).FirstOrDefault();
            if (contactBook != null)
            {
                return contactBook.Contacts;
            }
            // log tbd
            return new List<Models.Contact>();
        }

        public async Task<bool> TagsContactAsync(int userId, int contactId, List<string> tags)
        {
            var filter = Builders<ContackBook>.Filter.And(Builders<ContackBook>.Filter.Eq(x => x.UserId, userId),
                Builders<ContackBook>.Filter.Eq("Contacts.UserId", contactId)
                );
            var update = Builders<ContackBook>.Update.Set("Contacts.$.Tasg", tags);
            var result = await contactContext.ContactBooks.UpdateOneAsync(filter, update);
            return result.MatchedCount == result.ModifiedCount&&result.ModifiedCount == 1;   
        }

        /// <summary>
        /// 更新用户好友信息
        /// </summary>
        /// <param name="baseUserInfo"></param>
        /// <returns></returns>
        public async Task<bool> UpdateContactInfoAsync(BaseUserInfo baseUserInfo,CancellationToken token)
        {    
            //查询当前用户的好友信息。
            var contactBook = await contactContext.ContactBooks.FindAsync(x => x.UserId == baseUserInfo.UserId,null,token);
            if (contactBook == null)
            {
                //throw new Exception($"wrong user id for updateContactInfo:{baseUserInfo.UserId}");
                return false;
            }
            //获得当前用户的好友ID
            var contactIds= contactBook.FirstOrDefault(token).Contacts.Select(x => x.UserId);
            var filterDefintion = Builders<ContackBook>.Filter.And(Builders<ContackBook>.Filter.In(x => x.UserId, contactIds),
                Builders<ContackBook>.Filter.ElemMatch(x=>x.Contacts,contact=>contact.UserId==baseUserInfo.UserId)

                );
            //更新用户数据
            var update = Builders<ContackBook>.Update
                .Set("Contacts.$.Name", baseUserInfo.Name)
                .Set("Contact.$.Avatar", baseUserInfo.Avatar)
                .Set("Contact.$.Company", baseUserInfo.Company)
                .Set("Contact.$.Title", baseUserInfo.Title);
            var updateResult = contactContext.ContactBooks.UpdateMany(filterDefintion, update);
            return updateResult.MatchedCount == updateResult.ModifiedCount;
        }
    }
}
