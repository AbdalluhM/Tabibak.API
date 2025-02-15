using AutoMapper;
using Tabibak.Api.Dtos.AuthDtos;
using Tabibak.API.Core.Models;
using Tabibak.API.Dtos.Specialities;

namespace Tabibak.Api.Mapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            //CreateMap<UserInputDto, ApplicationUser>();

            CreateMap<DoctorInputDto, Doctor>()
                .ForMember(d => d.DoctorSpecialties, opt => opt.MapFrom(s => s.Specialties))
                ;

            CreateMap<SpecialtyInputDto, DoctorSpecialty>();

            CreateMap<PatientInputDto, Patient>();

            CreateMap<Doctor, GetDoctoctorBySpecialtyDto>();




        }
    }
}
