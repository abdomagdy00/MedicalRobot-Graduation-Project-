using Application.Hubs;
using Application.Interfaces.SignalRInterfaces;
using Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

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

        // 1. Endpoint: Activating the sensors (when the screen is turned on on the mobile phone)
        [HttpPost("start")]
        public async Task<IActionResult> StartSensors()
        {
            if (!_tracker.IsRobotConnected)
            {
                return StatusCode(503, new { message = "The robot is currently offline. Cannot start sensors." });
            }
            // Activate reading status in the backend
            _tracker.IsSensorReadingActive = true;

            // Sending the "Start" command to the ESP32 via SignalR
            await _hubContext.Clients.All.ReceiveSensorCommand("Start");

            Console.WriteLine("[System] Sensors reading ACTIVATED by Mobile App.");
            return Ok(new { message = "Sensors reading started successfully." });
        }

        // 2. Endpoint: Turn off sensors (when the mobile screen is locked)
        [HttpPost("stop")]
        public async Task<IActionResult> StopSensors()
        {
            // Even if the robot is offline, we disable the active state in the backend.

            // Disabling readings in the backend.
            _tracker.IsSensorReadingActive = false;

            if (_tracker.IsRobotConnected)
            {
                // Sending a "Stop" command to the ESP32 via SignalR
                await _hubContext.Clients.All.ReceiveSensorCommand("Stop");
            }

            Console.WriteLine("[System] Sensors reading DEACTIVATED by Mobile App.");
            return Ok(new { message = "Sensors reading stopped successfully." });
        }
    }
}