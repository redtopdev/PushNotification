/// <summary>
/// Developer: ShyamSk
/// </summary>

namespace Notification.Service
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json.Linq;
    using Notification.DataContract;
    using Notification.Manager;
    using System;
    using System.Threading.Tasks;

    [ApiController]
    [Route("/")]
    public class NotificationController : ControllerBase
    {
        private ILogger<NotificationController> logger;
        private INotificationManager notificationManager; 
        public NotificationController(ILogger<NotificationController> logger, INotificationManager notificationManager)
        {
            this.logger = logger;
            this.notificationManager = notificationManager;
        }

/*
        [HttpPost("notification/sendreminder")]
        public async Task<IActionResult> Post(Reminder reminder)
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

    */


        [HttpPost("notification/notifyparticipants")]
        public async Task<IActionResult> Post(EventNotification eventNotification)
        {
            logger.LogInformation("Getting location");
            try
            {
                JObject eventNotificationJObj = JObject.FromObject(eventNotification);
                await notificationManager.HandleEvent(eventNotificationJObj);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
