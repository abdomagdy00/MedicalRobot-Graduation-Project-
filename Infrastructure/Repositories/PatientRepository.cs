

using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class PatientRepository : GenericRepository<Patient>, IPatientRepository
    {
        private readonly AppDbContext _context;

        public PatientRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Patient> GetPatientByFaceIdAsync(int faceId)
        {
            return await _context.Patients
                .Include(p => p.MedicalRecords)
                .Include(p => p.AssignedDrawer)
                .FirstOrDefaultAsync(p => p.FaceId == faceId);
        }
    }
}
