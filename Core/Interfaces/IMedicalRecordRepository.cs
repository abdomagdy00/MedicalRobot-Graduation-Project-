

using Core.Entities;

namespace Core.Interfaces
{
    public interface IMedicalRecordRepository 
    {
        Task AddAsyn(MedicalRecord record);
        Task<IEnumerable<MedicalRecord>> GetRecentRecordsByPatientIdAsync(int patientId, int count);
    }
}
