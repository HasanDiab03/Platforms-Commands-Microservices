using AutoMapper;
using Grpc.Core;
using PlatformService.DTOs;
using PlatformService.Repositories;

namespace PlatformService.SyncDataServices.Grpc
{
	public class GrpcPlatformService : GrpcPlatform.GrpcPlatformBase
	{
		private readonly IPlatformRepository _platformRepository;
		private readonly IMapper _mapper;

		public GrpcPlatformService(IPlatformRepository platformRepository, IMapper mapper)
        {
			_platformRepository = platformRepository;
			_mapper = mapper;
		}
		public override async Task<PlatformResponse> GetAllPlatforms(GetAllRequest request, ServerCallContext context)
		{
			var response = new PlatformResponse();
			var platforms = await _platformRepository.GetAllPlatforms();
			response.Platforms.AddRange(_mapper.Map<List<GrpcPlatformModel>>(platforms));
			return response;
		}
	}
}
