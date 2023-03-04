using System;

namespace Notification.API.Service
{
    public interface INotificationsProducer
    {
        public void SendNotifications(String UserId,String Title,String Body);
    }
}