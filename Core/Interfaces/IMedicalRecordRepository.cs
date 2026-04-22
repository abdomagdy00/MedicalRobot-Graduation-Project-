

using Core.Entities;

namespace Core.Interfaces
{
    public interface IMedicalRecordRepository : IGenericRepository<MedicalRecord>
    {
        Task<IEnumerable<MedicalRecord>> GetRecentRecordsByPatientIdAsync(int patientId, int count);
    }
}
