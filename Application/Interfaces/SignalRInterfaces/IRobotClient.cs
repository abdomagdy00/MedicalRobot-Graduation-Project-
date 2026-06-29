using Application.DTOs;

namespace Application.Interfaces.SignalRInterfaces
{
    public interface IRobotClient
    {
        // To receive the vowel letters (F, B, L, R, S)
        Task ReceiveMovementCommand(string direction);
        // To receive an order to open and close a specific drawer
        Task DrawerCommand(string commandChar);   

        // To send a notification to the robot 
        Task ReceiveNotification(string message);
        // To send a learn command to the robot camera
        Task ReceiveLearnCommand(string command);
        //Control sensor readings
        Task ReceiveSensorCommand(string command);
        //دي الميثود الجديدة اللي هتبعت الداتا للموبايل
        Task ReceiveVitalsUpdated(VitalsResponseDto vitals);
    }
}
