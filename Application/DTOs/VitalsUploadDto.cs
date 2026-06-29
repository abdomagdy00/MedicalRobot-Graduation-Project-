

namespace Application.DTOs
{
    //Receiving data from the ESP32
    public class VitalsUploadDto
    {
        public int FaceId { get; set; } 
        public float Temperature { get; set; }
        public int HeartRate { get; set; }
        public int SpO2 { get; set; }
        public float RoomTemperature { get; set; }
        public float RoomHumidity { get; set; }
    }
}
