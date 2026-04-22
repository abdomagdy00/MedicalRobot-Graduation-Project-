
namespace Application.DTOs
{
    public class PatientDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }=null!;
        public int Age { get; set; }
        public string Gender { get; set; } = null!;
        public int? AssignedDrawerNumber { get; set; }
    }
}
