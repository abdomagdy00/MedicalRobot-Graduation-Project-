
namespace Core.Entities
{
    public class Patient
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public int Age { get; set; }
        public int FaceId { get; set; }
        public string Gender { get; set; } = null!;
        public ICollection<MedicalRecord> MedicalRecords { get; set; } = null!;
        public int? AssignedDrawerId { get; set; }
        public MedicineDrawer AssignedDrawer { get; set; } = null!;
    }
}
