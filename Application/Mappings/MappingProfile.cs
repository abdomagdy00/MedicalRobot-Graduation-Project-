using AutoMapper;
using Application.DTOs;
using Core.Entities;

namespace Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Patient, PatientDto>()
                .ForMember(dest => dest.AssignedDrawerNumber,
                           opt => opt.MapFrom(src => src.AssignedDrawer.DrawerNumber));

            CreateMap<MedicalRecord, VitalsResponseDto>();

            CreateMap<VitalsUploadDto, MedicalRecord>();

            CreateMap<MedicineDrawer, MedicineDrawerDto>()
                .ForMember(dest => dest.PatientName,
               opt => opt.MapFrom(src => src.Patient != null ? src.Patient.FullName : "There is no patient"));

        }
    }
}
