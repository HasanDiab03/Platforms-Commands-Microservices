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
        }
    }
}
