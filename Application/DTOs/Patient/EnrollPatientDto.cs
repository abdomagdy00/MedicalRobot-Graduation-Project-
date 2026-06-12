
using Core.Enums;

namespace Application.DTOs.Patient
{
    public class EnrollPatientDto
    {
        public string FullName { get; set; } = null!;
        public int Age { get; set; }
        public Gender Gender { get; set; }
    }
}
