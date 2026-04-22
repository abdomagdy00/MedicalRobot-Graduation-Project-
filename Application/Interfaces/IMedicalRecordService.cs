
using Application.DTOs;

namespace Application.Interfaces
{
    public interface IMedicalRecordService
    {
        Task<IEnumerable<VitalsResponseDto>> GetPatientHistoryAsync(int patientId, int count = 10);
    }
}
