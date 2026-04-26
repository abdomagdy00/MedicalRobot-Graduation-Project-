
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class MedicalRecordRepository:IMedicalRecordRepository
    {
        private readonly AppDbContext _context;

        public MedicalRecordRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsyn(MedicalRecord record)
        {
            await _context.MedicalRecords.AddAsync(record);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<MedicalRecord>> GetRecentRecordsByPatientIdAsync(int patientId, int count)
        {
            return await _context.MedicalRecords
                .Where(p => p.Id == patientId)
                .OrderByDescending(r => r.CapturedAt)
                .Take(count)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
