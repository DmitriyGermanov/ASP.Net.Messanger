using AutoMapper;
using Microsoft.AspNetCore.Identity;
using UserService.DTOs;
using UserService.Models;

namespace UserService.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserModel, UserLoginModel>().ReverseMap();
        }
    }
}
