using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contact.API.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
namespace Contact.API.Data
{
    public class ContactContext
    {
        private IMongoDatabase database;
        private IMongoCollection<ContackBook> collection;
        private AppSetting appSetting;
        public ContactContext(IOptionsSnapshot<AppSetting> _appSetting)
        {
            appSetting = _appSetting.Value;
            var client = new MongoClient(appSetting.MongonContactConnectionString);
            if (client != null)
            {
                database = client.GetDatabase(appSetting.MongonContactConnectionString);
            }
        }
        /// <summary>
        /// 检查是否包含collection
        /// </summary>
        /// <param name="collectionName">集合名称 </param>
        private void CheckAddCreateCollection(string collectionName)
        {
            var collectionList = database.ListCollections().ToList();
            var collectionNames = new List<string>();
            collectionList.ForEach(x => { collectionNames.Add(x["name"].AsString); });
            if (!collectionNames.Contains(collectionName))
            {
                database.CreateCollection(collectionName);
            }
        }
        /// <summary>
        /// 用户通讯录
        /// </summary>
        public IMongoCollection<ContackBook> ContactBooks
        {
            get
            {

                CheckAddCreateCollection("ContactBooks");
                return database.GetCollection<ContackBook>("ContactBooks");
            }
        }

        /// <summary>
        /// 好友请求
        /// </summary>
        public IMongoCollection<ContactApllyRequest> ContactApplyRequests
        {
            get
            {
                CheckAddCreateCollection("ContactApplyContact");
                return database.GetCollection<ContactApllyRequest>("ContactApplyContact");

            }
        }


    }
}
