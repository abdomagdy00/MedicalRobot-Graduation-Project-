using Core.Entities;

namespace Core.Interfaces
{
    public interface IRobotSettingRepository
    {
        Task<RobotSetting> GetByKeyAsync(string key);
        Task AddAsync(RobotSetting setting);
        Task UpdateAsync(RobotSetting setting);
    }
}
