using Application.Hubs;
using Core.Interfaces.SignalRInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
    }
}
