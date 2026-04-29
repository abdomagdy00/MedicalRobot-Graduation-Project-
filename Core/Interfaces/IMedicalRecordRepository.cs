

using Core.Entities;

namespace Core.Interfaces
{
    public interface IMedicalRecordRepository 
    {
        Task AddAsync(MedicalRecord record);
        Task<IEnumerable<MedicalRecord>> GetRecentRecordsByPatientIdAsync(int patientId, int count);
    }
}
