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

            CreateMap<Message, MessageDto>()
                .ForMember(dest => dest.SenderPhotoUrl, opt => opt.MapFrom(src =>
                    src.Sender.Photos.FirstOrDefault(x => x.IsMain).Url))
                .ForMember(dest => dest.RecipientPhotoUrl, opt => opt.MapFrom(src =>
                    src.Recipient.Photos.FirstOrDefault(x => x.IsMain).Url));
            CreateMap<Photo, PhotoDto>();
            CreateMap<UserUpdateDto, User>();
        }
    }
}
