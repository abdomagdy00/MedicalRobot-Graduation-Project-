using Application.DTOs;
using Application.Interfaces;
using Application.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecordsController : ControllerBase
    {
        private readonly IMedicalRecordService _recordService;
        private readonly IPatientService _patientService;
        private readonly IHubContext<RobotHub> _hubContext;

        public RecordsController(
            IMedicalRecordService recordService,
            IPatientService patientService, 
            IHubContext<RobotHub> hubContext)
        {
            _recordService = recordService;
            _patientService = patientService;
            _hubContext = hubContext;
        }

        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetHistory(int patientId, [FromQuery] int count = 10)
        {
            var history = await _recordService.GetPatientHistoryAsync(patientId, count);
            return Ok(history);
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadVitals([FromBody] VitalsUploadDto dto)
        {
            if (dto == null)
                return BadRequest("Invalid record data.");

            string patientName = await _patientService.AddPatientVitalsAsync(dto);

            string notificationMessage = $"Live medical update - Patient: {patientName} | Heart Rate: {dto.HeartRate} | blood oxygen level {dto.SpO2} | Temperature: {dto.Temperature}";
            await _hubContext.Clients.All.SendAsync("ReceiveNotification", notificationMessage);

            await _hubContext.Clients.All.SendAsync("ReceiveLiveVitals", new
            {
                patientName = patientName,
                temperature = dto.Temperature,
                heartRate = dto.HeartRate,
                spO2 = dto.SpO2,
                capturedAt = DateTime.Now
            });

            return Ok(new { message = "Vitals received and broadcasted successfully." });
        }
    }
}