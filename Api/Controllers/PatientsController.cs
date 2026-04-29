using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientsController(IPatientService patientService)
        {
            _patientService = patientService;
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
        [HttpPost("vitals")]
        public async Task<IActionResult> AddVitals([FromBody] VitalsUploadDto vitalsDto)
        {
            await _patientService.AddPatientVitalsAsync(vitalsDto);

            var patient = await _patientService.GetPatientByFaceIdAsync(vitalsDto.FaceId);
            var vitalsResponse = new VitalsResponseDto
            {
                HeartRate = vitalsDto.HeartRate,
                Temperature = vitalsDto.Temperature,
                SpO2 = vitalsDto.SpO2
            };
            await _patientService.StreamVitalsToClients(vitalsResponse, patient.FullName);
            return Ok(new { message = "Vitals recorded and streamed successfully." });
        }
    }
}
