using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecordsController : ControllerBase
    {
        private readonly IMedicalRecordService _recordService;

        public RecordsController(IMedicalRecordService recordService)
        {
            _recordService = recordService;
        }
        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetHistory(int patientId, [FromQuery] int count = 10)
        {
            var history = await _recordService.GetPatientHistoryAsync(patientId, count);
            return Ok(history);
        }
    }
}
