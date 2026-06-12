using Application.DTOs.Patient;
using Application.Hubs;
using Application.Interfaces;
using Application.Interfaces.SignalRInterfaces;
using Application.Services;
using Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class PatientsController : ControllerBase
    {
        private readonly FaceEnrollmentSessionManager _enrollmentManager;
        private readonly IPatientService _patientService;
        private readonly RobotConnectionTracker _tracker;
        private readonly IHubContext<RobotHub, IRobotClient> _hubContext;

        public PatientsController(
            FaceEnrollmentSessionManager enrollmentManager,
            IPatientService patientService,
            RobotConnectionTracker tracker,
            IHubContext<RobotHub, IRobotClient> hubContext)
        {
            _enrollmentManager = enrollmentManager;
            _patientService = patientService;
            _tracker = tracker;
            _hubContext = hubContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var patients = await _patientService.GetAllPatientsAsync();
            return Ok(patients);
        }

        [HttpGet("face/{faceId}")]
        public async Task<IActionResult> GetByFaceId(int faceId)
        {
            var patient = await _patientService.GetPatientByFaceIdAsync(faceId);
            return Ok(patient);
        }

        [HttpPost("enroll")]
        public async Task<IActionResult> Enroll([FromBody] EnrollPatientDto dto)
        {
            if (!_tracker.IsRobotConnected)
                return StatusCode(503, new { message = "The robot is offline." });

            _enrollmentManager.StartSession();

            // ابعت أمر الـ Learn للهاردوير
            await _hubContext.Clients.All.ReceiveLearnCommand("Learn");

            try
            {
                // السيرفر هيستنى هنا 30 ثانية لحد ما الكاميرا تخلص وتجيب الـ ID
                int assignedFaceId = await _enrollmentManager.WaitForFaceIdAsync(30);

                var createdPatient = await _patientService.CreatePatientWithFaceIdAsync(dto, assignedFaceId);

                return Ok(createdPatient); // رجع الداتا للموبايل
            }
            catch (TimeoutException)
            {
                return StatusCode(408, new { message = "Face learning timed out." });
            }
        }

        // Endpoint 2: الـ ESP32 بيبعت عليها الـ FaceID الجديد بعد ما يخلص
        [HttpPost("confirm-face-learning")]
        public IActionResult ConfirmFaceLearning([FromBody] FaceLearningResultDto dto)
        {
            bool isPaired = _enrollmentManager.CompleteSession(dto.FaceId);

            if (!isPaired)
                return BadRequest(new { message = "No active session or timed out." });

            return Ok(new { message = "Face ID registered successfully." });
        }

        [HttpDelete("delete-by-face/{faceId}")]
        public async Task<IActionResult> DeleteByFaceId(int faceId)
        {
            var isDeleted = await _patientService.DeletePatientByFaceIdAsync(faceId);

            if (!isDeleted)
            {
                // الرد لو المريض مش موجود في الداتا بيز
                return NotFound(new { message = $"No patient found with Face ID: {faceId}" });
            }

            // الرد بالتأكيد لو تم المسح بنجاح
            return Ok(new { message = "The patient has been successfully deleted." });
        }
    }
}
