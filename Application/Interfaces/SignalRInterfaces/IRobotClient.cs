
namespace Core.Interfaces.SignalRInterfaces
{
    public interface IRobotClient
    {
        // To receive the vowel letters (F, B, L, R, S)
        Task ReceiveMovementCommand(string direction);
        // To receive an order to open and close a specific drawer
        Task DrawerCommand(string commandChar);   

        // To send a notification to the robot 
        Task ReceiveNotification(string message);
    }
}
