using Application.DTOs;

namespace Application.Interfaces.SignalRInterfaces
{
    public interface IMobileClient
    {
        Task UpdateLiveVitals(VitalsResponseDto vitals);
        Task UpdateRobotStatus(string status); // Connected / Disconnected
    }
}
