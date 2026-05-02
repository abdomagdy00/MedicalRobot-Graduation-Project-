
using System.Threading.Tasks;

namespace Core.Entities
{
    public class RobotSetting
    {
        public int Id { get; set; }
        public string Key { get; set; } // "CameraIp"
        public string Value { get; set; } // "192.168.1.50"
        public DateTime LastUpdated { get; set; }
    }
}
