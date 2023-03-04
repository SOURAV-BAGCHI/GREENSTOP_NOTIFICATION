using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Net.Http.WebPush;
using Lib.Net.Http.WebPush.Authentication;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Notification.API.Helpers;
using Notification.API.Implementation;

namespace Notification.API.Service
{
    public class NotificationsProducer:INotificationsProducer//:BackgroundService
    {
        private readonly IPushSubscriptionsService _pushSubscriptionsService;
        private readonly PushServiceClient _pushClient;
        public NotificationsProducer(IOptions<PushNotificationsOptions> options,IOptions<AppSettings> appSettings,
        IPushSubscriptionsService pushSubscriptionsService, IPushServiceClientImp pushClient)
        {
            _pushSubscriptionsService=pushSubscriptionsService;
            _pushClient=pushClient.GetPushServiceClient();
            _pushClient.DefaultAuthentication=new VapidAuthentication(options.Value.PublicKey,options.Value.PrivateKey){
                Subject="https://gs.trikaltech.com/"//appSettings.Value.Site
            };
        }

        // protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        // {
        //     while (!stoppingToken.IsCancellationRequested)
        //     {
        //         await Task.Delay(1000, stoppingToken);
        //         SendNotifications(_random.Next(-20, 55), stoppingToken);
        //     }
        // }

        public void SendNotifications(String UserId,String Title,String Body)
        {
            PushMessage notification = new  AngularPushNotification
            {
                Title = Title,
                Body = Body,//$"Temp. (C): {temperatureC} | Temp. (F): {32  + (int)(temperatureC /  0.5556)}",
                Icon = "assets/icons/apple-icon-180.png",
                Vibrate=new List<Int32>(){50,100,50},
                Data=new Dictionary<String,Object>(){
                    {"url","http://127.0.0.1:8080/order"}
                }

            }.ToPushMessage();

            foreach (PushSubscription subscription in _pushSubscriptionsService.GetSubscriberList(UserId))
            {
                // fire-and-forget
                _pushClient.RequestPushMessageDeliveryAsync(subscription, notification);
            }
        }
    }
}