

using Application.DTOs;
using Core.Entities;

namespace Application.Mappings
{
    public static class MappingExtensions
    {
        public static PatientDto MapToDto(this Patient patient)
        {
            if (patient == null) return null;
            return new PatientDto
            {
                Id = patient.Id,
                FullName = patient.FullName,
                Age = patient.Age,
                Gender = patient.Gender,
                AssignedDrawerNumber = patient.AssignedDrawer?.DrawerNumber

            };
        }
        public static MedicalRecord MapToEntity(this VitalsUploadDto dto, int patientId)
        {
            return new MedicalRecord
            {
                PatientId = patientId,
                Temperature = dto.Temperature,
                HeartRate = dto.HeartRate,
                SpO2 = dto.SpO2,
                CapturedAt = DateTime.Now
            };
        }
        public static VitalsResponseDto MapToDto(this MedicalRecord record)
        {
            return new VitalsResponseDto
            {
                Temperature = record.Temperature,
                HeartRate = record.HeartRate,
                SpO2 = record.SpO2,
                CapturedAt = record.CapturedAt
            };
        }
        public static MedicineDrawerDto MapToDto(this MedicineDrawer drawer)
        {
            return new MedicineDrawerDto
            {
                Id = drawer.Id,
                DrawerNumber = drawer.DrawerNumber,
                DrawerStatus= drawer.DrawerStatus,
                PatientName = drawer.Patient?.FullName ?? "Not Found Patient"
            };
        }
    }
}
