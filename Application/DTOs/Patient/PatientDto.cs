using Core.Enums;
namespace Application.DTOs.Patient
{
    public class PatientDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }=null!;
        public int Age { get; set; }
        public Gender Gender { get; set; }
        public int? AssignedDrawerNumber { get; set; }
    }
}
