using Core.Entities;
using Core.Enums;

namespace Core.Interfaces
{
    public interface IMedicineDrawerRepository 
    {
        Task<MedicineDrawer> GetDrawerByPatientIdAsync(int patientId);
        Task UpdateDrawerStatusAsync(int drawerId,DrawerStatus status);
        Task<IEnumerable<MedicineDrawer>> GetAllDrawersWithPatientsAsync();
    }
}
