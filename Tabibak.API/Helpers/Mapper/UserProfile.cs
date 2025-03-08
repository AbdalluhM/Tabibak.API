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


            CreateMap<Doctor, GetDoctorBySpecialtyDto>()
                .ForMember(dest => dest.Review, opt => opt.MapFrom(src => src.Reviews.Any() ? src.Reviews.Average(r => r.Rating) : 0))
                 .ForMember(dest => dest.IsFavorite, opt => opt.Ignore());
            ;
            CreateMap<Doctor, DoctorDetailResultDto>();




        }
    }
}
