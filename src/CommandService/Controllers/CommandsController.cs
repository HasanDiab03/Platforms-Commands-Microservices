using AutoMapper;
using CommandService.Commands;
using CommandService.DTOs;
using CommandService.Models;
using CommandService.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers
{
	[Route("api/c/platforms/{platformId}/[controller]")]
	[ApiController]
	public class CommandsController : ControllerBase
	{
		private readonly ICommandRepository _commandRepository;
		private readonly IMapper _mapper;

		public CommandsController(ICommandRepository commandRepository, IMapper mapper)
        {
			_commandRepository = commandRepository;
			_mapper = mapper;
		}
        [HttpGet]
		public async Task<IActionResult> GetCommandsForPlatform(int platformId)
		{
			if(!(await _commandRepository.PlatformExists(platformId)))
				return BadRequest("no platform with provided platform id exists");
			return Ok(_mapper.Map<List<CommandDTO>>(await _commandRepository.GetCommandsForPlatform(platformId)));
		}
		[HttpGet("{commandId}")]
		public async Task<IActionResult> GetCommandForPlatform(int platformId, int commandId)
		{
			if (!(await _commandRepository.PlatformExists(platformId)))
				return BadRequest("no platform with provided platform id exists");
			return Ok(_mapper.Map<CommandDTO>(await _commandRepository.GetCommandForPlatform(platformId, commandId)));
		}
		[HttpPost]
		public async Task<IActionResult> CreateCommandForPlatform(int platformId, CreateCommand createCommand)
		{
			if (!(await _commandRepository.PlatformExists(platformId)))
				return BadRequest("no platform with provided platform id exists");
			if(!ModelState.IsValid)
				return BadRequest("Command Validation failed");
			var command = _commandRepository.CreateCommandForPlatform(platformId, _mapper.Map<Command>(createCommand));
			var commandDto = _mapper.Map<CommandDTO>(command);
			return CreatedAtRoute(nameof(GetCommandForPlatform), new { platformId, commandId = commandDto.Id }, commandDto);
		}
	}
}
