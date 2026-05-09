using Application.Hubs;
using Application.Interfaces.SignalRInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RobotControlController : ControllerBase
    {
        private readonly IHubContext<RobotHub, IRobotClient> _hubContext;

        public RobotControlController(IHubContext<RobotHub, IRobotClient> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpPost("move")]
        public async Task<IActionResult> Move([FromQuery] string direction)
        {
            await _hubContext.Clients.All.ReceiveMovementCommand(direction);
            return Ok(new { message = $"Direction {direction} sent to robot." });
        }

        [HttpPost("drawer")]
        public async Task<IActionResult> SendDrawerCommand([FromQuery] string command)
        {
            await _hubContext.Clients.All.DrawerCommand(command);
            return Ok(new { message = $"Command {command} sent to robot." });
        }

        [HttpPost("notify")]
        public async Task<IActionResult> SendNotification([FromBody] string message)
        {
            await _hubContext.Clients.All.ReceiveNotification(message);
            return Ok(new { status = "Notification sent." });
        }
    }
}
