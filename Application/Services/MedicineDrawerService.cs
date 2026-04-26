
using Application.DTOs;
using Application.Interfaces;
using Application.Mappings;
using Core.Enums;
using Core.Exceptions;
using Core.Interfaces;

namespace Application.Services
{
    public class MedicineDrawerService : IMedicineDrawerService
    {
        private readonly IMedicineDrawerRepository _drawerRepository;
        private readonly IPatientRepository _patientRepository;

        public MedicineDrawerService(
            IMedicineDrawerRepository drawerRepository,
            IPatientRepository patientRepository)
        {
            _drawerRepository = drawerRepository;
            _patientRepository = patientRepository;
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

            await _drawerRepository.UpdateDrawerStatusAsync(patient.AssignedDrawer.Id,DrawerStatus.Open);
            return patient.AssignedDrawer.CommandChar;
        }

        public async Task<bool> ToggleDrawerAsync(int drawerId, DrawerStatus status)
        {
            await _drawerRepository.UpdateDrawerStatusAsync(drawerId, status);
            return true;
        }
    }
}
