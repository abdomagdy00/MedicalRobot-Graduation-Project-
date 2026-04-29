using Application.DTOs;

namespace Core.Interfaces.SignalRInterfaces
{
    public interface IMobileClient
    {
        Task UpdateLiveVitals(VitalsResponseDto vitals);
        Task UpdateRobotStatus(string status); // Connected / Disconnected
    }
}
