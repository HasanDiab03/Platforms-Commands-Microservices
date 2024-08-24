using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers
{
	[Route("api/c/[controller]")]
	[ApiController]
	public class PlatfromsController : ControllerBase
	{
        public PlatfromsController()
        {
            
        }
        [HttpPost]
        public IActionResult TestInboundConnection()
        {
            Console.WriteLine("--> Inboud Post # Command Service");
            return Ok("Inbound test of Command service from Platfrom Controller");
        }
    }
}
