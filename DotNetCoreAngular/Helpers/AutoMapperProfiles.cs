using AutoMapper;
using DotNetCoreAngular.Dtos;
using DotNetCoreAngular.Models.Entity;

namespace DotNetCoreAngular.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserDetailDto>()
                .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src =>
                    src.Photos.FirstOrDefault(x => x.IsMain).Url));

            CreateMap<UserDetailDto, User>();

            CreateMap<Message, MessageDto>();
            CreateMap<Photo, PhotoDto>();
            CreateMap<UserUpdateDto, User>();
        }
    }
}
