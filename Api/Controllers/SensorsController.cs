using Application.Hubs;
using Application.Interfaces.SignalRInterfaces;
using Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SensorsController : ControllerBase
    {
        private readonly RobotConnectionTracker _tracker;
        private readonly IHubContext<RobotHub, IRobotClient> _hubContext;

        public SensorsController(
            RobotConnectionTracker tracker,
            IHubContext<RobotHub, IRobotClient> hubContext)
        {
            _tracker = tracker;
            _hubContext = hubContext;
        }

        // 1. Endpoint: Activate the sensors (the phone calls to them as soon as the screen is turned on)
        [HttpPost("start")]
        public async Task<IActionResult> StartSensors()
        {
            if (!_tracker.IsRobotConnected)
            {
                return StatusCode(503, new { message = "The robot is currently offline. Cannot start sensors." });
            }
            // Sending the "Start" command to the ESP32 via SignalR
            await _hubContext.Clients.All.ReceiveSensorCommand("Start");

            return Ok(new { message = "Sensors reading started successfully." });
        }

        // 2. Endpoint: Turn off the sensors (the phone calls to them when the screen is locked)
        [HttpPost("stop")]
        public async Task<IActionResult> StopSensors()
        {
            if (!_tracker.IsRobotConnected)
            {
                return StatusCode(503, new { message = "The robot is currently offline." });
            }

            // Sending a "Stop" command to the ESP32 via SignalR
            await _hubContext.Clients.All.ReceiveSensorCommand("Stop");

            return Ok(new { message = "Sensors reading stopped successfully." });
        }
    }
}
