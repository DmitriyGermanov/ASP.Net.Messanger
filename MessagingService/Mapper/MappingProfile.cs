using AutoMapper;
using MessagingService.Dtos;
using MessagingService.Models;

namespace MessagingService.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Message, SendMessageRequestDTO>().ReverseMap();
        }
    }
}
