using Application.DTOs;
using Core.Enums;

namespace Application.Interfaces
{
    public interface IMedicineDrawerService
    {
        Task<string> OpenDrawerByFaceIdAsync(int faceId);

        // Manual control from mobile
        Task<bool> ToggleDrawerAsync(int drawerId, DrawerStatus status);
        // Get the status of all drawers 
        Task<IEnumerable<MedicineDrawerDto>> GetAllDrawersStatusAsync();
    }
}
