using Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using MyNotificationService;
using SmartHomeController.Key;

namespace SmartHomeController.Controllers
{
    [ApiController]
    //[ApiKey]
    [Route("/Alerts/")]
    public class AlertController : ControllerBase
    {
        private INotificationSender _NotificationSender;
        private IUserService _userService;

        public AlertController(INotificationSender NotificationSender, IUserService userService)
        {
            _NotificationSender = NotificationSender;
            _userService = userService;
        }

        /// <summary>
        /// Register a user for receiving alerts and notifications.
        /// </summary>
        [HttpPost]
        [Route("register/{token}/{userId:long}")]
        public async Task<ActionResult> RegisterForAlerts([FromRoute] string token, [FromRoute] long userId)
        {
            try
            {
                // Uncomment the line below to test notifications directly
                // await _notificationClient.SendNotificationToUser(token, "Hello", "This is an alert!");
                await _userService.SetTokenForUser(userId, token);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Unregister a user from receiving alerts and notifications.
        /// </summary>
        [HttpPatch]
        [Route("unregister/{userId:long}")]
        public async Task<ActionResult> UnregisterUser([FromRoute] long userId)
        {
            try
            {
                await _userService.RemoveTokenFromUser(userId);
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500, e.Message);
            }
        }
    }
}
