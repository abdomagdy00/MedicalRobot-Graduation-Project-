using Application.DTOs;
using Application.Interfaces;
using Application.Hubs;
using Application.Interfaces.SignalRInterfaces;
using Core.Services; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecordsController : ControllerBase
    {
        private readonly IMedicalRecordService _recordService;
        private readonly IPatientService _patientService;
        private readonly IHubContext<RobotHub, IRobotClient> _hubContext;
        private readonly RobotConnectionTracker _tracker;

        public RecordsController(
            IMedicalRecordService recordService,
            IPatientService patientService,
            IHubContext<RobotHub, IRobotClient> hubContext,
            RobotConnectionTracker tracker) 
        {
            _recordService = recordService;
            _patientService = patientService;
            _hubContext = hubContext;
            _tracker = tracker; 
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

            if (!_tracker.IsSensorReadingActive)
            {
                Console.WriteLine($"[Warning] Received vitals (FaceId:{dto.FaceId}) but readings are DEACTIVATED by Mobile. Data ignored.");
                return Ok(new { message = "Data ignored. Vitals reading is currently stopped by the mobile application." });
            }

            // 1. Saving to the database using FaceId and DHT22 via the Service
            string patientName = await _patientService.AddPatientVitalsAsync(dto);
            // 2. Live streaming of the mobile application (Strongly-Typed)
            var responseDto = new VitalsResponseDto
            {
                Temperature = dto.Temperature,
                HeartRate = dto.HeartRate,
                SpO2 = dto.SpO2,
                RoomTemperature = dto.RoomTemperature,
                RoomHumidity = dto.RoomHumidity,
                CapturedAt = DateTime.Now 
            };

            await _hubContext.Clients.All.ReceiveVitalsUpdated(responseDto);

            string notificationMessage = $"Live update - Patient: {patientName}";
            await _hubContext.Clients.All.ReceiveNotification(notificationMessage);

            return Ok(new { message = "Vitals received, saved, and broadcasted successfully." });
        }
    }
}