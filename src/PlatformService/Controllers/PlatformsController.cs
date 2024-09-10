using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PlatformService.AsyncDataServices;
using PlatformService.Commands;
using PlatformService.DTOs;
using PlatformService.Models;
using PlatformService.Repositories;
using PlatformService.SyncDataServices;

namespace PlatformService.Controllers
{
	[ApiController]
	[Route("/api/[controller]")]
	public class PlatformsController : ControllerBase
	{
		private readonly IPlatformRepository _platformRepository;
		private readonly IMapper _mapper;
		private readonly ICommandDataClient _dataClient;
		private readonly IMessageBusClient _messageBusClient;

		public PlatformsController(IPlatformRepository platformRepository, IMapper mapper, 
			ICommandDataClient dataClient, IMessageBusClient messageBusClient)
        {
			_platformRepository = platformRepository;
			_mapper = mapper;
			_dataClient = dataClient;
			_messageBusClient = messageBusClient;
		}
		[HttpGet]
		public async Task<IActionResult> GetAllPlatforms()
		{
			var platforms = await _platformRepository.GetAllPlatforms();
			return Ok(_mapper.Map<List<PlatformDTO>>(platforms));
		}
		[HttpGet("{id}", Name = "GetPlatform")]
		public async Task<IActionResult> GetPlatform(int id)
		{
			var platform = await _platformRepository.GetPlatformById(id);
			return platform == null ? NotFound($"Platform with id={id} does not exist") 
				: Ok(_mapper.Map<PlatformDTO>(platform));
		}
		[HttpPost]
		public async Task<IActionResult> CreatePlatform(PlatformCommand command)
		{
			if(!ModelState.IsValid)
			{
				return BadRequest("Invalid Command");
			}
			var mappedPlatform = _mapper.Map<Platform>(command);
			await _platformRepository.CreatePlatform(mappedPlatform);
			var platformDto = _mapper.Map<PlatformDTO>(mappedPlatform);

			// Send sync message
			try
			{
				await _dataClient.SendPlatformToCommandService(platformDto);
				
			}
			catch (Exception ex) 
			{
				Console.WriteLine($"Could not send synchronous data, ex Message: {ex.Message}");
			}

			// Send Async message
			try
			{
				var publishPlatform = _mapper.Map<PlatformPublishDTO>(platformDto);
				publishPlatform.Event = "Platform_Published";
				_messageBusClient.PublishNewPlatform(publishPlatform);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Could not send asynchronous data, ex Message: {ex.Message}");

			}

			return CreatedAtRoute(nameof(GetPlatform), new {id = platformDto.Id}, platformDto);
		}
    }
}