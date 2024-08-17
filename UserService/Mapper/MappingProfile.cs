using AutoMapper;
using UserService.DTOs;
using UserService.Models;

namespace UserService.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserLoginModel>().ReverseMap();
        }
    }
}
