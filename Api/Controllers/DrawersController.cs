using Application.DTOs.DrawerDtos;
using Application.Hubs;
using Application.Interfaces;
using Application.Interfaces.SignalRInterfaces;
using Core.Enums;
using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class DrawersController : ControllerBase
    {
        private readonly IMedicineDrawerService _drawerService;
        private readonly RobotConnectionTracker _tracker;
        private readonly IHubContext<RobotHub, IRobotClient> _hubContext;

        public DrawersController(
            IMedicineDrawerService drawerService,
            RobotConnectionTracker tracker,
            IHubContext<RobotHub, IRobotClient> hubContext)
        {
            _drawerService = drawerService;
            _tracker = tracker;
            _hubContext = hubContext;
        }

        [HttpGet("status")]
        public async Task<IActionResult> GetStatus()
        {
            var statuses = await _drawerService.GetAllDrawersStatusAsync();
            return Ok(statuses);
        }

        [HttpPost("{id}/toggle")]
        public async Task<IActionResult> Toggle(int id, [FromBody] ToggleDrawerDto dto)
        {
            if (!_tracker.IsRobotConnected)
            {
                return StatusCode(503, new { message = "The robot is currently offline. The Drawer status cannot be modified." });
            }

            var status = dto.Open ? DrawerStatus.Open : DrawerStatus.Closed;
            await _drawerService.ToggleDrawerAsync(id, status);

            // إرسال الأمر الصريح للـ ESP32 بناءً على طلب الموبايل
            string command = dto.Open ? $"O{id}" : $"C{id}";
            await _hubContext.Clients.All.DrawerCommand(command);

            return Ok(new { message = $"The Drawer status has been successfully requested to be {status}." });
        }

        [HttpPost("report-switch-state")]
        public async Task<IActionResult> ReportSwitchState([FromBody] HardwareSwitchReportDto dto)
        {
            // مزامنة الداتا بيز مع الحالة المادية الحقيقية للدرج
            await _drawerService.UpdateDrawerPhysicalStateAsync(dto.DrawerNumber, dto.IsOpened);

            // إرسال إشعار فوري للموبايل لتحديث الواجهة أمام الدكتور تلقائياً
            string state = dto.IsOpened ? "Open" : "Closed";
            await _hubContext.Clients.All.ReceiveNotification($"Drawer number {dto.DrawerNumber} is now actually {state}.");

            return Ok(new { message = "Physical state synchronized successfully." });
        }
        [HttpPost("open-by-face/{faceId}")]
        public async Task<IActionResult> OpenByFace(int faceId)
        {
            var commandChar = await _drawerService.OpenDrawerByFaceIdAsync(faceId);

            await _hubContext.Clients.All.ReceiveNotification($"Patient drawer opened with face ID {faceId}");

            return Ok(new
            {
                command = commandChar,
                message = "Face recognized. Open the drawer."
            });
        }
    }
}
