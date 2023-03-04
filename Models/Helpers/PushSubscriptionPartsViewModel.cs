namespace Notification.API.Models.Helpers
{
    public class PushSubscriptionPartsViewModel
    {
        public string endpoint { get; set; }
        public string p256dh{get;set;}
        public string auth{get;set;}
    }
}