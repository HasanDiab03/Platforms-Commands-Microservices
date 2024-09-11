using AutoMapper;
using PlatformService.Commands;
using PlatformService.DTOs;
using PlatformService.Models;

namespace PlatformService.MappingProfiles
{
	public class MappingProfile : Profile
	{
        public MappingProfile()
        {
            CreateMap<Platform, PlatformDTO>();
            CreateMap<PlatformCommand, Platform>();
            CreateMap<PlatformDTO, PlatformPublishDTO>();
            CreateMap<Platform, GrpcPlatformModel>()
                .ForMember(dest => dest.PlatformId, opt => opt.MapFrom(src => src.Id));
        }
    }
}
