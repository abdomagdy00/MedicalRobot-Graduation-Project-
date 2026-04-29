
using Application.DTOs;
using Application.Hubs;
using Application.Interfaces;
using Application.Mappings;
using Core.Enums;
using Core.Exceptions;
using Core.Interfaces;
using Core.Interfaces.SignalRInterfaces;
using Microsoft.AspNetCore.SignalR;

namespace Application.Services
{
    public class MedicineDrawerService : IMedicineDrawerService
    {
        private readonly IMedicineDrawerRepository _drawerRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IHubContext<RobotHub, IRobotClient> _hubContext;

        public MedicineDrawerService(
            IMedicineDrawerRepository drawerRepository,
            IPatientRepository patientRepository,
            IHubContext<RobotHub, IRobotClient> hubContext)
        {
            _drawerRepository = drawerRepository;
            _patientRepository = patientRepository;
            _hubContext = hubContext;
        }
        public async Task<IEnumerable<MedicineDrawerDto>> GetAllDrawersStatusAsync()
        {
            var drawers = await _drawerRepository.GetAllDrawersWithPatientsAsync();
            return drawers.Select(d => d.MapToDto());
        }

        public async Task<string> OpenDrawerByFaceIdAsync(int faceId)
        {
            var patient = await _patientRepository.GetByFaceIdAsync(faceId);
            if (patient == null)
                throw new NotFoundException("The patient is not present.");
            if (patient.AssignedDrawer == null)
                throw new Exception("This patient does not have a designated drawer.");
            
            // Update drawer status to open in the database
            await _drawerRepository.UpdateDrawerStatusAsync(patient.AssignedDrawer.Id,DrawerStatus.Open);

            //SignalR notification to robot to open the drawer
            await _hubContext.Clients.All.DrawerCommand(patient.AssignedDrawer.CommandChar);
            
            return patient.AssignedDrawer.CommandChar;
        }

        public async Task<bool> ToggleDrawerAsync(int drawerId, DrawerStatus status)
        {
            await _drawerRepository.UpdateDrawerStatusAsync(drawerId, status);
            return true;
        }
    }
}
