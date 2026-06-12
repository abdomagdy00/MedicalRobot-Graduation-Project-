using Application.DTOs;
using Application.DTOs.Patient;
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

        public async Task<string> AddPatientVitalsAsync(VitalsUploadDto vitalsDto)
        {
            var patient = await _patientRepository.GetByFaceIdAsync(vitalsDto.FaceId);
            if (patient == null)
                throw new NotFoundException($"Vitual readings could not be recorded: No patient registered with FaceId: {vitalsDto.FaceId}");

            var record = vitalsDto.MapToEntity(patient.Id);
            await _recordRepository.AddAsync(record);

            return patient.FullName;
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

        public async Task<PatientDto> CreatePatientWithFaceIdAsync(EnrollPatientDto dto, int faceId)
        {
            var newPatient = new Patient
            {
                FullName = dto.FullName,
                Age = dto.Age,
                Gender = dto.Gender,
                FaceId = faceId // ربط الـ ID القادم من الهاردوير
            };

            var savedPatient = await _patientRepository.CreateAsync(newPatient);
            return savedPatient.MapToDto(); // حوله لـ Dto ورجعه
        }
        public async Task<bool> DeletePatientByFaceIdAsync(int faceId)
        {
            // بنستخدم دالتك القديمة عشان نجيب المريض
            var patient = await _patientRepository.GetByFaceIdAsync(faceId);

            if (patient == null)
            {
                return false; // المريض مش موجود
            }

            // لو موجود، نمسحه
            await _patientRepository.DeleteAsync(patient);
            return true;
        }
    }
}