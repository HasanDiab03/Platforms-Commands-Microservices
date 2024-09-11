using AutoMapper;
using CommandService.Models;
using Grpc.Net.Client;

namespace CommandService.SyncDataServices
{
	public class PlatformDataClient : IPlatformDataClient
	{
		private readonly IMapper _mapper;
		private readonly IConfiguration _config;

		public PlatformDataClient(IConfiguration config, IMapper mapper)
        {
			_mapper = mapper;
			_config = config;
		}
        public List<Platform> GetAllPlatforms()
		{
			var platformAddr = _config["GrpcPlatform"];
			Console.WriteLine($"--> Calling gRPC server: {platformAddr}");
			var channel = GrpcChannel.ForAddress(platformAddr);
			var client = new GrpcPlatform.GrpcPlatformClient(channel);
			
			try
			{
				var request = new GetAllRequest();
				var reply = client.GetAllPlatforms(request);
				return _mapper.Map<List<Platform>>(reply.Platforms);
			} catch (Exception ex)
			{
				Console.WriteLine($"Request to get platforms using gRPC failed: {ex.Message}");
				return null;
			}
		}
	}
}
