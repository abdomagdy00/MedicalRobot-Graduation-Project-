

using Application.DTOs;
using Application.Interfaces;
using Application.Mappings;
using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;

namespace Application.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IMedicalRecordRepository _recordRepository;
        public PatientService(
            IPatientRepository patientRepository,
            IMedicalRecordRepository recordRepository)
        {
            _patientRepository = patientRepository;
            _recordRepository = recordRepository;
        }
        

        public async Task<bool> AddPatientVitalsAsync(VitalsUploadDto vitalsDto)
        {
            var patient = await _patientRepository.GetByFaceIdAsync(vitalsDto.FaceId);
            if (patient == null)
                throw new NotFoundException($"Vitual readings could not be recorded: No patient registered with FaceId: {vitalsDto.FaceId}");

            var record = vitalsDto.MapToEntity(patient.Id);

            await _recordRepository.AddAsyn(record);
            return true;
        }

        public async Task<IEnumerable<PatientDto>> GetAllPatientsAsync()
        {
            var patients = await _patientRepository.GetAllAsync();
            return patients.Select(p => p.MapToDto());
        }

        public async Task<PatientDto> GetPatientByFaceIdAsync(int faceId)
        {
            var patient = await _patientRepository.GetByFaceIdAsync(faceId);
            if (patient == null)
                throw new NotFoundException($"The patient with face number ({faceId}) is not registered in the system.");
            return patient.MapToDto();
        }
    }
}
