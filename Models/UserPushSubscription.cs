using System;
using Lib.Net.Http.WebPush;

namespace Notification.API.Models
{
    public class UserPushSubscription
    {
        public String UserId{get;set;}
        public PushSubscription SubscriptionDetails{get;set;}
    }
}