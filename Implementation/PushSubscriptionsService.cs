using System;
using System.Collections.Generic;
using System.Linq;
using Lib.Net.Http.WebPush;
using LiteDB;
using Notification.API.Models;

namespace Notification.API.Implementation
{
    public class PushSubscriptionsService : IPushSubscriptionsService, IDisposable
    {
        private readonly LiteDatabase _db;
        private readonly ILiteCollection<UserPushSubscription> _collection;

        public PushSubscriptionsService()
        {
            _db = new LiteDatabase("PushSubscriptionsStore.db");
            _collection = _db.GetCollection<UserPushSubscription>("subscriptions");  
        }
        public void Delete(string endpoint)
        {
            var Data=_collection.FindOne(x=>x.SubscriptionDetails.Endpoint==endpoint);
            _collection.DeleteMany(x=>x.UserId==Data.UserId);
        }

        public void Insert(UserPushSubscription subscription)
        {
            _collection.Insert(subscription);
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        public IEnumerable<PushSubscription> GetSubscriberList(string UserId)
        {
            var Data=_collection.Find(x=>x.UserId==UserId).Select(y=>y.SubscriptionDetails);
            if(Data ==null)
            {
                Data=new List<PushSubscription>();
            }

            return Data;
        }
    }
}