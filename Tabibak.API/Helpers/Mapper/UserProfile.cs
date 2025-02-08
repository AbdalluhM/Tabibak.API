using AutoMapper;
using Tabibak.Api.Dtos.AuthDtos;
using Tabibak.API.Core.Models;

namespace Tabibak.Api.Mapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            //CreateMap<UserInputDto, ApplicationUser>();

            CreateMap<DoctorInputDto, Doctor>()
                .ForMember(d => d.Specialties, opt => opt.MapFrom(s => s.Specialties))
                ;

            CreateMap<SpecialtyInputDto, Specialty>();

            CreateMap<PatientInputDto, Patient>();




        }
    }
}
