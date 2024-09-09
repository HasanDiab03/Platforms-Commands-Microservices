using AutoMapper;
using CommandService.DTOs;
using CommandService.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers
{
	[Route("api/c/[controller]")]
	[ApiController]
	public class PlatformsController : ControllerBase
	{
		private readonly ICommandRepository _commandRepository;
		private readonly IMapper _mapper;

		public PlatformsController(ICommandRepository commandRepository, IMapper mapper)
        {
			_commandRepository = commandRepository;
			_mapper = mapper;
		}
        [HttpPost]
        public IActionResult TestInboundConnection()
        {
            Console.WriteLine("--> Inboud Post # Command Service");
            return Ok("Inbound test of Command service from Platfrom Controller");
        }
		[HttpGet]
		public async Task<IActionResult> GetAllPlatforms() 
		{
			return Ok(_mapper.Map<List<PlatformDTO>>(await	_commandRepository.GetAllPlatforms()));
		}
    }
}
