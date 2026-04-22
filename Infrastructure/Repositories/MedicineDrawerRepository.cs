

using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class MedicineDrawerRepository : GenericRepository<MedicineDrawer>, IMedicineDrawerRepository
    {
        private readonly AppDbContext _context;

        public MedicineDrawerRepository(AppDbContext context) :base(context) 
        { 
            _context = context; 
        }

        public async Task<MedicineDrawer> GetDrawerByPatientIdAsync(int patientId)
        {
            return await _context.MedicineDrawers
            .FirstOrDefaultAsync(d => d.PatientId == patientId);
        }

        public async Task UpdateDrawerStatusAsync(int drawerId, bool isOpen)
        {
            var drawer = await _context.MedicineDrawers.FindAsync(drawerId);
            if(drawer !=null)
            {
                drawer.IsOpened = isOpen;
                _context.Update(drawer);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<MedicineDrawer>> GetAllDrawersWithPatientsAsync()
        {
            return await _context.MedicineDrawers
                .Include(d => d.Patient)
                .ToListAsync();
        }
    }
}
