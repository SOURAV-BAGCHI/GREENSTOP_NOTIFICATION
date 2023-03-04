using Lib.Net.Http.WebPush;

namespace Notification.API.Implementation
{
    public interface IPushServiceClientImp
    {
        public PushServiceClient GetPushServiceClient();
    }
}