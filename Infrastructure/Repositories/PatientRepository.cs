
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly AppDbContext _context;

        public PatientRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Patient> GetByIdAsync(int id)
        {
            return await _context.Patients
                .Include(p => p.AssignedDrawer)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task<Patient> GetByFaceIdAsync(int faceId)
        {
            return await _context.Patients
            .Include(p => p.AssignedDrawer)
            .FirstOrDefaultAsync(p => p.FaceId == faceId);
        }
        public async Task<IEnumerable<Patient>> GetAllAsync()
        {
            return await _context.Patients
                .Include(p => p.AssignedDrawer)
                .AsNoTracking() 
                .ToListAsync();
        }
        public async Task AddAsync(Patient patient)
        {
            await _context.Patients.AddAsync(patient);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Patient patient)
        {
            _context.Patients.Update(patient);
            await _context.SaveChangesAsync();
        }
    }
}
