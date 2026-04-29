using Application.DTOs;
using Application.Hubs;
using Application.Interfaces;
using Application.Mappings;
using Core.Exceptions;
using Core.Interfaces;
using Core.Interfaces.SignalRInterfaces;
using Microsoft.AspNetCore.SignalR;

namespace Application.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IMedicalRecordRepository _recordRepository;
        private readonly IHubContext<RobotHub, IRobotClient> _hubContext;

        public PatientService(
            IPatientRepository patientRepository,
            IMedicalRecordRepository recordRepository,
            IHubContext<RobotHub, IRobotClient> hubContext)
        {
            _patientRepository = patientRepository;
            _recordRepository = recordRepository;
            _hubContext = hubContext;
        }

       

        public async Task<bool> AddPatientVitalsAsync(VitalsUploadDto vitalsDto)
        {
            var patient = await _patientRepository.GetByFaceIdAsync(vitalsDto.FaceId);
            if (patient == null)
                throw new NotFoundException($"Vitual readings could not be recorded: No patient registered with FaceId: {vitalsDto.FaceId}");

            var record = vitalsDto.MapToEntity(patient.Id);

            await _recordRepository.AddAsync(record);
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

        public async Task StreamVitalsToClients(VitalsResponseDto vitals, string patientName)
        {
            await _hubContext.Clients.All
                .ReceiveNotification($"Live medical update - Patient: {patientName}" +
                $" | Heart Rate: {vitals.HeartRate} | blood oxygen level {vitals.SpO2} | Temperature: {vitals.Temperature}");
        }
    }
}
