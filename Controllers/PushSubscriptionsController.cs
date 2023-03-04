using System;
using Lib.Net.Http.WebPush;
using Microsoft.AspNetCore.Mvc;
using Notification.API.CommonMethod;
using Notification.API.Implementation;
using Notification.API.Models;
using Notification.API.Service;

namespace Notification.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PushSubscriptionsController:ControllerBase
    {
        private readonly IPushSubscriptionsService _pushSubscriptionsService;
        private readonly INotificationsProducer _notificationProducer;
        private readonly IJwtDecoder _jwtDecoder;
        public PushSubscriptionsController(IPushSubscriptionsService pushSubscriptionsService,
        IJwtDecoder jwtDecoder,INotificationsProducer notificationProducer)
        {
            _pushSubscriptionsService=pushSubscriptionsService;
            _jwtDecoder=jwtDecoder;
            _notificationProducer=notificationProducer;
        }
        
        [HttpPost]
        public void Post([FromBody] PushSubscription subscription)
        {
            String authHeader = Convert.ToString(Request.HttpContext.Request.Headers["Authorization"]).Substring(7);
            String nid=_jwtDecoder.Decode(authHeader).nid;
            var userSubsciption=new UserPushSubscription(){
                UserId=nid,
                SubscriptionDetails=subscription
            };
            _pushSubscriptionsService.Insert(userSubsciption);
        }

        // [HttpPost]
        // public void Post([FromBody] PushSubscriptionPartsViewModel subscriptionData)
        // {
        //     PushSubscription subscription=new PushSubscription();
        //     subscription.Endpoint=subscriptionData.endpoint;
        //     subscription.Keys=new Dictionary<string, string>();
        //     subscription.Keys.Add("p256dh",subscriptionData.p256dh);
        //     subscription.Keys.Add("auth",subscriptionData.auth);

        //     String authHeader = Convert.ToString(Request.HttpContext.Request.Headers["Authorization"]).Substring(7);
        //     String nid=_jwtDecoder.Decode(authHeader).nid;
        //     var userSubsciption=new UserPushSubscription(){
        //         UserId=nid,
        //         SubscriptionDetails=subscription
        //     };
        //     _pushSubscriptionsService.Insert(userSubsciption);
        // }

        [HttpDelete("{endpoint}")]
        public void Delete(string endpoint)
        {
            _pushSubscriptionsService.Delete(endpoint);
        }

        [HttpGet("[action]/{userid}")]
        public IActionResult SendNotification([FromRoute] String userid)
        {
            _notificationProducer.SendNotifications(userid,"Test Notification","Lets hope it works");

            return Ok();
        }


    }
}