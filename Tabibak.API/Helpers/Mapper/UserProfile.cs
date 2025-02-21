using AutoMapper;
using Tabibak.API.Core.Models;
using Tabibak.API.Dtos.Specialities;

namespace Tabibak.Api.Mapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            //CreateMap<UserInputDto, ApplicationUser>();


            CreateMap<Doctor, GetDoctoctorBySpecialtyDto>();
            CreateMap<Doctor, DoctorDetailResultDto>();




        }
    }
}
