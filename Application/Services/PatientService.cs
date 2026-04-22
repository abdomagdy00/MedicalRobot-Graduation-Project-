

using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;

namespace Application.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IMedicalRecordRepository _recordRepository;
        private readonly IMapper _mapper;
        public PatientService(
            IPatientRepository patientRepository,
            IMedicalRecordRepository recordRepository,
            IMapper mapper)
        {
            _patientRepository = patientRepository;
            _recordRepository = recordRepository;
            _mapper = mapper;
        }
        

        public async Task<bool> AddPatientVitalsAsync(VitalsUploadDto vitalsDto)
        {
            var patient = await _patientRepository.GetPatientByFaceIdAsync(vitalsDto.FaceId);
            if (patient == null)
                throw new NotFoundException($"Vitual readings could not be recorded: No patient registered with FaceId: {vitalsDto.FaceId}");
         
            var record = _mapper.Map<MedicalRecord>(vitalsDto);
            record.PatientId = patient.Id;
            record.CapturedAt = DateTime.Now;

            await _recordRepository.AddAsync(record);
            return true;
        }

        public async Task<IEnumerable<PatientDto>> GetAllPatientsAsync()
        {
            var patients = await _patientRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<PatientDto>>(patients);
        }

        public async Task<PatientDto> GetPatientByFaceIdAsync(int faceId)
        {
            var patient = await _patientRepository.GetPatientByFaceIdAsync(faceId);
            if (patient == null)
                throw new NotFoundException($"The patient with face number ({faceId}) is not registered in the system.");
            return _mapper.Map<PatientDto>(patient);
        }
    }
}
