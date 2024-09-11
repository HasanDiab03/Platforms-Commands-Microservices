using AutoMapper;
using CommandService.Commands;
using CommandService.DTOs;
using CommandService.Models;

namespace CommandService.MappingProfiles
{
	public class MappingProfile : Profile
	{
        public MappingProfile()
        {
            CreateMap<CreateCommand, Command>();   
            CreateMap<Command, CommandDTO>();
            CreateMap<Platform, PlatformDTO>();
            CreateMap<PlatformPublishDTO, Platform>()
                .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));
            CreateMap<GrpcPlatformModel, Platform>()
                .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.PlatformId));
        }
    }
}
