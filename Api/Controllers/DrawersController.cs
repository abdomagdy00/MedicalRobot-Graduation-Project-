using Application.Interfaces;
using Core.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DrawersController : ControllerBase
    {
        private readonly IMedicineDrawerService _drawerService;

        public DrawersController(IMedicineDrawerService drawerService)
        {
            _drawerService = drawerService;
        }

        [HttpGet("status")]
        public async Task<IActionResult> GetStatus()
        {
            var statuses = await _drawerService.GetAllDrawersStatusAsync();
            return Ok(statuses);
        }
        [HttpPost("{id}/toggle")]
        public async Task<IActionResult> Toggle(int id, [FromQuery] DrawerStatus status)
        {
            await _drawerService.ToggleDrawerAsync(id, status);
            return Ok(new { message = $"The Drawer status has been changed to{status}" });
        }

        [HttpPost("open-by-face/{faceId}")]
        public async Task<IActionResult> OpenByFace(int faceId)
        {
            var commandChar = await _drawerService.OpenDrawerByFaceIdAsync(faceId);
            // Note: Here we return the character that should be sent to the robot (e.g.,'A')
            return Ok(new { command = commandChar, message = "Drawer open command sent to robot." });
        }
    }
}
