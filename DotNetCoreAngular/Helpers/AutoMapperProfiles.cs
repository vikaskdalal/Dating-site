using AutoMapper;
using DotNetCoreAngular.Dtos;
using DotNetCoreAngular.Models.Entity;

namespace DotNetCoreAngular.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserDetailDto>();

            CreateMap<UserDetailDto, User>();

            CreateMap<Message, MessageDto>();
        }
    }
}
