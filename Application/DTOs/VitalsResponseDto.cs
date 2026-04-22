
namespace Application.DTOs
{
    //To view medical records
    public class VitalsResponseDto
    {
        public float Temperature { get; set; }
        public int HeartRate { get; set; }
        public int SpO2 { get; set; }
        public DateTime CapturedAt { get; set; }
    }
}
