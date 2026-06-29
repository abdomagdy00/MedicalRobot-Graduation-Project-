using Application.DTOs;
using Application.Interfaces.SignalRInterfaces;
using Core.Entities;
using Core.Interfaces;
using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Application.Hubs
{
    //[Authorize]
    [AllowAnonymous]
    public class RobotHub : Hub<IRobotClient>
    {
        private readonly RobotConnectionTracker _tracker;
        private readonly IPatientRepository _patientRepository;
        private readonly IMedicalRecordRepository _medicalRecordRepository; 

        public RobotHub(
            RobotConnectionTracker tracker,
            IPatientRepository patientRepository,
            IMedicalRecordRepository medicalRecordRepository)
        {
            _tracker = tracker;
            _patientRepository = patientRepository;
            _medicalRecordRepository = medicalRecordRepository;
        }
        public override async Task OnConnectedAsync()
        {
            var connectionId = Context.ConnectionId;
            Console.WriteLine($"Client connected: {connectionId}");

            var deviceType = Context.GetHttpContext()?.Request.Query["deviceType"].ToString();
            if (deviceType == "robot")
            {
                _tracker.IsRobotConnected = true;
                Console.WriteLine("🤖 Medical Robot is now ONLINE!"); 
            }

            await Clients.Client(connectionId)
                .ReceiveNotification("The medical robot server was successfully connected.");

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var connectionId = Context.ConnectionId;
            Console.WriteLine($"Client disconnected: {connectionId}");

            var deviceType = Context.GetHttpContext()?.Request.Query["deviceType"].ToString();
            if (deviceType == "robot")
            {
                _tracker.IsRobotConnected = false;
                Console.WriteLine("⚠️ Medical Robot is OFFLINE!"); 
            }

            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMoveCommand(string direction)
        {
            // Send the command to all connected devices "except" the one that sent the command (the mobile phone)
            // This ensures that only the robot receives the character
            await Clients.Others.ReceiveMovementCommand(direction);
            Console.WriteLine($"Movement command sent: {direction}");
        }


        // =========================================================
        // Receive from ESP32 and send to mobile
        // =========================================================
        public async Task UploadVitals(VitalsUploadDto uploadDto)
        {
            // 1. Bring the patient with Face ID
            var patient = await _patientRepository.GetByFaceIdAsync(uploadDto.FaceId);
            if (patient == null)
            {
                Console.WriteLine($"[Warning] Unregistered FaceId: {uploadDto.FaceId}. Data ignored.");
                return;
            }

            // 2. Prepare the record for saving 
            var record = new MedicalRecord
            {
                PatientId = patient.Id,
                Temperature = uploadDto.Temperature,
                HeartRate = uploadDto.HeartRate,
                SpO2 = uploadDto.SpO2,
                RoomTemperature = uploadDto.RoomTemperature,
                RoomHumidity = uploadDto.RoomHumidity,
                CapturedAt = DateTime.Now
            };

            // 3. Save to the database
            await _medicalRecordRepository.AddAsync(record);

            // 4. Prepare the response for the mobile
            var responseDto = new VitalsResponseDto
            {
                Temperature = record.Temperature,
                HeartRate = record.HeartRate,
                SpO2 = record.SpO2,
                RoomTemperature = record.RoomTemperature,
                RoomHumidity = record.RoomHumidity,
                CapturedAt = record.CapturedAt
            };

            // 5. Send to the mobile
            await Clients.Others.ReceiveVitalsUpdated(responseDto);
            Console.WriteLine($"[Success] Vitals saved and broadcasted for Patient ID: {patient.Id}");
        }
    }
}

