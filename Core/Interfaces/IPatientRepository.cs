
using Core.Entities;

namespace Core.Interfaces
{
    public interface IPatientRepository : IGenericRepository<Patient>
    {
        Task<Patient> GetPatientByFaceIdAsync(int faceId);
    }
}
