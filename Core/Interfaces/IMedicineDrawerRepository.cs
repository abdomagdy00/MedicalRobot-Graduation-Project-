using Core.Entities;
using Core.Enums;

namespace Core.Interfaces
{
    public interface IMedicineDrawerRepository 
    {
        Task<MedicineDrawer> GetDrawerByPatientIdAsync(int patientId);
        Task UpdateDrawerStatusAsync(int drawerId,DrawerStatus status);
        Task UpdateDrawerByNumberAsync(int drawerNumber, DrawerStatus status, bool isOpened);
        Task<IEnumerable<MedicineDrawer>> GetAllDrawersWithPatientsAsync();
    }
}
