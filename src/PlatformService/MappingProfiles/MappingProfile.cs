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
        }
    }
}
