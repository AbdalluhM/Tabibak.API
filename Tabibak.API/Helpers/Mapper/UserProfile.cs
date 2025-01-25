using AutoMapper;
using Tabibak.Api.Dtos.AuthDtos;
using Tabibak.Models;

namespace Tabibak.Api.Mapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserInputDto, ApplicationUser>().ReverseMap();





        }
    }
}
