using System;
using System.Collections.Generic;
using Lib.Net.Http.WebPush;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Notification.API.Service
{
    public class AngularPushNotification
    {
        public class NotificationAction
        {
            public String Action { get; }
            public String Title { get; }

            public NotificationAction(string action, string title)
            {
                Action = action;
                Title = title;
            }
        }

        public String Title { get; set; }
        public String Body { get; set; }
        public String Icon { get; set; }
        public IList<Int32> Vibrate { get; set; } = new  List<Int32>();
        public IDictionary<String, object> Data { get; set; }
        public IList<NotificationAction> Actions { get; set; } = new  List<NotificationAction>();


        private const string WRAPPER_START = "{\"notification\":";
        private const string WRAPPER_END = "}";
        private static readonly JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new  CamelCasePropertyNamesContractResolver()
        };

        public PushMessage ToPushMessage(string topic = null, int? timeToLive = null, PushMessageUrgency urgency = PushMessageUrgency.Normal)
        {
            return new PushMessage(WRAPPER_START + JsonConvert.SerializeObject(this, _jsonSerializerSettings) + WRAPPER_END)
            {
                Topic = topic,
                TimeToLive = timeToLive,
                Urgency = urgency
            };
        }
    }
}