using Application.Interfaces;
using Core.Entities;
using Core.Interfaces;

namespace Application.Services
{
    public class RobotService : IRobotService
    {
        private readonly IRobotSettingRepository _settingRepository;

        public RobotService(IRobotSettingRepository settingRepository)
        {
            _settingRepository = settingRepository;
        }
        public async Task UpdateCameraIpAsync(string ip)
        {
            var setting = await _settingRepository.GetByKeyAsync("CameraIp");

            if (setting == null)
            {
                await _settingRepository.AddAsync(new RobotSetting
                {
                    Key = "CameraIp",
                    Value = ip,
                    LastUpdated = DateTime.Now
                });
            }
            else
            {
                setting.Value = ip;
                setting.LastUpdated = DateTime.Now;
                await _settingRepository.UpdateAsync(setting);
            }
        }
        public async Task<string> GetLiveStreamUrlAsync()
        {
            var setting = await _settingRepository.GetByKeyAsync("CameraIp");
            return setting != null ? $"http://{setting.Value}:81/stream" : null;
        }


    }
}
