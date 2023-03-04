using System;
using Notification.API.Models.Helpers;

namespace Notification.API.CommonMethod
{
    public interface IJwtDecoder
    {
        JWTviewModel Decode (String JWT);
    }
}