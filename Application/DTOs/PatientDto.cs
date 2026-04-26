using Core.Enums;
namespace Application.DTOs
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
