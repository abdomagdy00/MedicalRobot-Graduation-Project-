
namespace Core.Enums
{
    public enum CommandStatus
    {
        Pending = 1,   // The matter is pending implementation.
        Sent = 2,      // Send to the robot
        Executed = 3,  // The robot performed it successfully
        Failed = 4     // It was not implemented (there is a problem).
    }
}
