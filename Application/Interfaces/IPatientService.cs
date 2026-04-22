

using Application.DTOs;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IPatientService
    {
        Task<PatientDto> GetPatientByFaceIdAsync(int faceId);
        Task<IEnumerable<PatientDto>> GetAllPatientsAsync();
        Task<bool> AddPatientVitalsAsync(VitalsUploadDto vitalsDto);
    }
}
