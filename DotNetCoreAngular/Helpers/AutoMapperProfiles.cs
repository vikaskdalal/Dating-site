using AutoMapper;
using DotNetCoreAngular.DTO;
using DotNetCoreAngular.Models.Entity;

namespace DotNetCoreAngular.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserDetailDto>();
        }
    }
}
