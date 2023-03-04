using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Notification.API.CommonMethod;
using Notification.API.Data;
using Notification.API.Service;

namespace Notification.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SendNotificationController:ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly INotificationsProducer _notificationProducer;
        private readonly IJwtDecoder _jwtDecoder;
        public SendNotificationController(ApplicationDbContext db,IJwtDecoder jwtDecoder,
        INotificationsProducer notificationProducer)
        {
            _db=db;
            _jwtDecoder=jwtDecoder;
            _notificationProducer=notificationProducer;
        }

        [HttpGet("[action]/{orderid}")]
        public async Task<IActionResult> GetOrderDetails([FromRoute] string orderid)
        {
            var orderDetails=await _db.OrderDetails.FindAsync(orderid);

            return Ok(orderDetails);
        }
    }
}