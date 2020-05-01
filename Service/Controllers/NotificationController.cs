/// <summary>
/// Developer: ShyamSk
/// </summary>

namespace Notification.Service
{
   
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Notification.DataContract;
    using Notification.Manager;
    using System;
    using System.Threading.Tasks;

    [ApiController]
    public class NotificationController : ControllerBase
    {
        private ILogger<NotificationController> logger;
        private INotificationManager notificationManager; 
        public NotificationController(ILogger<NotificationController> logger, INotificationManager notificationManager)
        {
            this.logger = logger;
            this.notificationManager = notificationManager;
        }


        [HttpPost("notification/sendreminder")]
        public async Task<IActionResult> Get(Reminder reminder)
        {
            logger.LogInformation("Getting location");

            //validate 
            //string message = await locationManager.ValidateLocationRequest(userId, eventId);
            //if (string.IsNullOrEmpty(message))
            //{
            //    return BadRequest(message);
            //}

            //put try catch only when you want to return custom message or status code, else this will
            //be caught in ExceptionHandling middleware so no need to put try catch here

            return Ok();
        }       
    }
}
