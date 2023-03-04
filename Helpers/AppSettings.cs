using System;

namespace Notification.API.Helpers
{
    public class AppSettings
    {
                //Properties for JWT Token Signature
        public String Site{get;set;}
        public String Audience{get;set;}
        public String ExpireTime{get;set;} //in minutes
        public String Secret{get;set;}  
        public String ClientId{get;set;}
    }
}