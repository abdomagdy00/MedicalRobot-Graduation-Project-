
namespace Core.Services
{
    public class RobotConnectionTracker
    {
        public bool IsRobotConnected { get; set; } = false;
        public string LastDirection { get; set; } = "S";
    }
}
