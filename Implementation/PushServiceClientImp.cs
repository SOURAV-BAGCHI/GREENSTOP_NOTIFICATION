using Lib.Net.Http.WebPush;

namespace Notification.API.Implementation
{
    public class PushServiceClientImp : IPushServiceClientImp
    {
        public PushServiceClient GetPushServiceClient()
        {
            return new PushServiceClient();
        }
    }
}