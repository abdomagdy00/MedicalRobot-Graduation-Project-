

using Application.DTOs;
using Application.DTOs.Patient;

namespace Application.Interfaces
{
    public interface IPatientService
    {
        Task<PatientDto> GetPatientByFaceIdAsync(int faceId);
        Task<IEnumerable<PatientDto>> GetAllPatientsAsync();
        Task<string> AddPatientVitalsAsync(VitalsUploadDto vitalsDto);
        Task<PatientDto> CreatePatientWithFaceIdAsync(EnrollPatientDto dto, int faceId);
        Task<bool> DeletePatientByFaceIdAsync(int faceId);
    }
}
