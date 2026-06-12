using Core.Entities;
using Core.Enums;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }
        public DbSet<MedicineDrawer> MedicineDrawers { get; set; }
        public DbSet<RobotSetting> RobotSettings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Patient>()
                .HasOne(p => p.AssignedDrawer)
                .WithOne(d => d.Patient)
                .HasForeignKey<MedicineDrawer>(d => d.PatientId);

            modelBuilder.Entity<Patient>().HasData(
                new Patient
                {
                    Id = 1,
                    FullName = "Mohamed Khaled",
                    Age = 45,
                    Gender = Gender.Male, // افترضت إن اسم القيمة Male في الـ Enum عندك
                    FaceId = 1001,
                    AssignedDrawerId = 1
                },
                new Patient
                {
                    Id = 2,
                    FullName = "Sara Ahmed",
                    Age = 30,
                    Gender = Gender.Female, // افترضت إن اسم القيمة Female في الـ Enum
                    FaceId = 1002,
                    AssignedDrawerId = 2
                }
            );

            // 2. إضافة أدراج وهمية (MedicineDrawers)
            modelBuilder.Entity<MedicineDrawer>().HasData(
                new MedicineDrawer
                {
                    Id = 1,
                    DrawerNumber = 1,
                    IsOpened = false,
                    CommandChar = "O1",
                    DrawerStatus = DrawerStatus.Closed, 
                    PatientId = 1
                },
                new MedicineDrawer
                {
                    Id = 2,
                    DrawerNumber = 2,
                    IsOpened = true,
                    CommandChar = "O2",
                    DrawerStatus = DrawerStatus.Open, 
                    PatientId = 2
                },
                new MedicineDrawer
                {
                    Id = 3,
                    DrawerNumber = 3,
                    IsOpened = false,
                    CommandChar = "O3",
                    DrawerStatus = DrawerStatus.Closed,
                    PatientId = null // درج فاضي مش مربوط بمريض
                }
            );

            // 3. إضافة سجلات طبية سابقة (MedicalRecords)
            modelBuilder.Entity<MedicalRecord>().HasData(
                new MedicalRecord
                {
                    Id = 1,
                    PatientId = 1,
                    HeartRate = 72,
                    Temperature = 36.8f, // لاحظ حرف الـ f عشان ده Float
                    SpO2 = 95,
                    CapturedAt = new DateTime(2026, 5, 8, 10, 0, 0)
                },
                new MedicalRecord
                {
                    Id = 2,
                    PatientId = 1,
                    HeartRate = 80,
                    Temperature = 37.5f,
                    SpO2 = 92,
                    CapturedAt = new DateTime(2026, 5, 8, 22, 0, 0)
                }
            );



        }
    }
}