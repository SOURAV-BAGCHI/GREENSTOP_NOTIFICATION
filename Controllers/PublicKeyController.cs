using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Notification.API.Helpers;

namespace Notification.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PublicKeyController:ControllerBase
    {
        private readonly PushNotificationsOptions _options;
        public PublicKeyController(IOptions<PushNotificationsOptions> options)
        {
            _options=options.Value;
        }

        // public ContentResult Get()
        // {
        //     return Content(_options.PublicKey, "text/plain");
        // }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new{PublicKey=_options.PublicKey});
        }
    }
}