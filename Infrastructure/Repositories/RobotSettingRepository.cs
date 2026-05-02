
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class RobotSettingRepository : IRobotSettingRepository
    {
        private readonly AppDbContext _context;

        public RobotSettingRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(RobotSetting setting)
        {
            await _context.RobotSettings.AddAsync(setting);
            await _context.SaveChangesAsync();
        }

        public async Task<RobotSetting> GetByKeyAsync(string key)
        {
            return await _context.RobotSettings
            .FirstOrDefaultAsync(s => s.Key == key);
        }

        public async Task UpdateAsync(RobotSetting setting)
        {
            _context.RobotSettings.Update(setting);
            await _context.SaveChangesAsync();
        }
    }
}
