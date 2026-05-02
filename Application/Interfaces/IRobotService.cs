using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IRobotService
    {
        Task UpdateCameraIpAsync(string ip);
        Task<string> GetLiveStreamUrlAsync();
    }
}
