
using Core.Entities;

namespace Core.Interfaces
{
    public interface IPatientRepository 
    {
        Task<Patient> GetByIdAsync(int id);
        Task<Patient> GetByFaceIdAsync(int faceId);
        Task<IEnumerable<Patient>> GetAllAsync();
        Task AddAsync(Patient patient);
        Task UpdateAsync(Patient patient);
    }
}
