using Application.Hubs;
using Application.Interfaces.SignalRInterfaces;
using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class RobotControlController : ControllerBase
    {
        private readonly IHubContext<RobotHub, IRobotClient> _hubContext;
        private readonly RobotConnectionTracker _tracker;

        public RobotControlController(IHubContext<RobotHub, IRobotClient> hubContext, RobotConnectionTracker tracker)
        {
            _hubContext = hubContext;
            _tracker = tracker;
        }

        [HttpPost("move")]
        public async Task<IActionResult> Move([FromQuery] string direction)
        {
            if (!_tracker.IsRobotConnected)
            {
                return StatusCode(503, new { message = "The robot is currently offline. Please ensure the robot is turned on and connected to the internet." });
            }
            string cleanDirection = direction.ToUpper().Trim();

            _tracker.LastDirection = cleanDirection;
            await _hubContext.Clients.All.ReceiveMovementCommand(direction);
            return Ok(new { message = $"Direction {direction} sent to robot." });
        }

        [HttpGet("status")]
        public IActionResult GetRobotStatus()
        {
            return Ok(new { direction = _tracker.LastDirection });
        }

        //[HttpPost("drawer")]
        //public async Task<IActionResult> SendDrawerCommand([FromQuery] string command)
        //{
        //    if (!_tracker.IsRobotConnected)
        //    {
        //        return StatusCode(503, new { message = "The robot is currently offline. Cannot send drawer command." });
        //    }
        //    await _hubContext.Clients.All.DrawerCommand(command);
        //    return Ok(new { message = $"Command {command} sent to robot." });
        //}

        [HttpPost("notify")]
        public async Task<IActionResult> SendNotification([FromBody] string message)
        {
            if (!_tracker.IsRobotConnected)
            {
                return StatusCode(503, new { message = "The robot is currently offline. Cannot send notification." });
            }
            await _hubContext.Clients.All.ReceiveNotification(message);
            return Ok(new { status = "Notification sent." });
        }
    }
}
