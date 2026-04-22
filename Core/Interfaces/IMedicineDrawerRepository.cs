using Core.Entities;

namespace Core.Interfaces
{
    public interface IMedicineDrawerRepository : IGenericRepository<MedicineDrawer>
    {
        Task<MedicineDrawer> GetDrawerByPatientIdAsync(int patientId);
        Task UpdateDrawerStatusAsync(int drawerId, bool isOpen);
        Task<IEnumerable<MedicineDrawer>> GetAllDrawersWithPatientsAsync();
    }
}
