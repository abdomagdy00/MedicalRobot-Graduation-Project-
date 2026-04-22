
using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Core.Interfaces;

namespace Application.Services
{
    public class MedicalRecordService : IMedicalRecordService
    {
        private readonly IMedicalRecordRepository _recordRepository;
        private readonly IMapper _mapper;

        public MedicalRecordService(IMedicalRecordRepository recordRepository, IMapper mapper)
        {
            _recordRepository = recordRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<VitalsResponseDto>> GetPatientHistoryAsync(int patientId, int count = 10)
        {
            var records = await _recordRepository.GetRecentRecordsByPatientIdAsync(patientId, count);
            return _mapper.Map<IEnumerable<VitalsResponseDto>>(records);
        }
    }
}
