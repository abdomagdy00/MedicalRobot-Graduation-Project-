
using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Core.Exceptions;
using Core.Interfaces;

namespace Application.Services
{
    public class MedicineDrawerService : IMedicineDrawerService
    {
        private readonly IMedicineDrawerRepository _drawerRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IMapper _mapper;

        public MedicineDrawerService(
            IMedicineDrawerRepository drawerRepository,
            IPatientRepository patientRepository,
             IMapper mapper)
        {
            _drawerRepository = drawerRepository;
            _patientRepository = patientRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<MedicineDrawerDto>> GetAllDrawersStatusAsync()
        {
            var drawers = await _drawerRepository.GetAllDrawersWithPatientsAsync();
            return _mapper.Map<IEnumerable<MedicineDrawerDto>>(drawers);
        }

        public async Task<string> OpenDrawerByFaceIdAsync(int faceId)
        {
            var patient = await _patientRepository.GetPatientByFaceIdAsync(faceId);
            if (patient == null)
                throw new NotFoundException("The patient is not present.");
            if (patient.AssignedDrawer == null)
                throw new Exception("This patient does not have a designated drawer.");

            await _drawerRepository.UpdateDrawerStatusAsync(patient.AssignedDrawer.Id, true);
            return patient.AssignedDrawer.CommandChar;
        }

        public async Task<bool> ToggleDrawerAsync(int drawerId, bool open)
        {
            await _drawerRepository.UpdateDrawerStatusAsync(drawerId, open);
            return true;
        }
    }
}
