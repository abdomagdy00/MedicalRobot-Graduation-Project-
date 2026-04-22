
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class MedicalRecordRepository : GenericRepository<MedicalRecord>, IMedicalRecordRepository
    {
        private readonly AppDbContext _context;

        public MedicalRecordRepository(AppDbContext context)
            :base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MedicalRecord>> GetRecentRecordsByPatientIdAsync(int patientId, int count)
        {
            return await _context.MedicalRecords
                .Where(p => p.Id == patientId)
                .OrderByDescending(r => r.CapturedAt)
                .Take(count)
                .ToListAsync();
        }
    }
}
