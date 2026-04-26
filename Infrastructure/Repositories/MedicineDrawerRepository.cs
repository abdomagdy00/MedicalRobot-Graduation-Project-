

using Core.Entities;
using Core.Enums;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class MedicineDrawerRepository: IMedicineDrawerRepository
    {
        private readonly AppDbContext _context;

        public MedicineDrawerRepository(AppDbContext context) 
        { 
            _context = context; 
        }

        public async Task<MedicineDrawer> GetDrawerByPatientIdAsync(int patientId)
        {
            return await _context.MedicineDrawers
            .FirstOrDefaultAsync(d => d.PatientId == patientId);
        }

        public async Task UpdateDrawerStatusAsync(int drawerId,DrawerStatus status)
        {
            var drawer = new MedicineDrawer { Id = drawerId, DrawerStatus = DrawerStatus.Open };
            _context.Entry(drawer).Property(x => x.DrawerStatus).IsModified = true;
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<MedicineDrawer>> GetAllDrawersWithPatientsAsync()
        {
            return await _context.MedicineDrawers
                .Include(d => d.Patient)
                .ToListAsync();
        }
    }
}
