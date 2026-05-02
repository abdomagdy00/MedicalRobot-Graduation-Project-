using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigurationController : ControllerBase
    {
        private readonly IRobotService _robotService;

        public ConfigurationController(IRobotService robotService)
        {
            _robotService = robotService;
        }

        [HttpPost("register-camera-ip")]
        public async Task<IActionResult> RegisterIp([FromBody] string ip)
        {
            if (string.IsNullOrEmpty(ip))
                return BadRequest("IP address cannot be empty.");

            await _robotService.UpdateCameraIpAsync(ip);
            return Ok(new { message = "Robot camera IP registered successfully." });
        }

        [HttpGet("live-stream-url")]
        public async Task<IActionResult> GetStreamUrl()
        {
            var url = await _robotService.GetLiveStreamUrlAsync();

            if (string.IsNullOrEmpty(url))
                return NotFound(new { message = "Camera IP is not registered yet. Please ensure the robot is online." });

            return Ok(new { streamUrl = url });
        }
    }
}
