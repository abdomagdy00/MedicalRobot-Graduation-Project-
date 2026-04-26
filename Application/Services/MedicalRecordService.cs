
using Application.DTOs;
using Application.Interfaces;
using Application.Mappings;
using Core.Interfaces;

namespace Application.Services
{
    public class MedicalRecordService : IMedicalRecordService
    {
        private readonly IMedicalRecordRepository _recordRepository;

        public MedicalRecordService(IMedicalRecordRepository recordRepository)
        {
            _recordRepository = recordRepository;
        }
        public async Task<IEnumerable<VitalsResponseDto>> GetPatientHistoryAsync(int patientId, int count = 10)
        {
            var records = await _recordRepository.GetRecentRecordsByPatientIdAsync(patientId, count);
            return records.Select(r => r.MapToDto());
        }
    }
}
