using System.Collections.Generic;
using Lib.Net.Http.WebPush;
using Notification.API.Models;

namespace Notification.API.Implementation
{
    public interface IPushSubscriptionsService
    {
        public void Insert(UserPushSubscription subscription);
        public void Delete(string endpoint);
        public IEnumerable<PushSubscription> GetSubscriberList(string UserId);

    }
}