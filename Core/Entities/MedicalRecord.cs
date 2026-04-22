
namespace Core.Entities
{
    public class MedicalRecord
    {
        public int Id { get; set; }
        public float Temperature { get; set; }
        public int HeartRate { get; set; }
        public int SpO2 { get; set; }
        public DateTime CapturedAt { get; set; } = DateTime.Now;

        public int PatientId { get; set; }
        public Patient Patient { get; set; } = null!;
    }
}
