using PlatformService.DTOs;
using System.Text;
using System.Text.Json;

namespace PlatformService.SyncDataServices.Http
{
	public class HttpCommandDataClient : ICommandDataClient
	{
		private readonly HttpClient _client;
		private readonly IConfiguration _config;

		public HttpCommandDataClient(HttpClient client, IConfiguration config)
        {
			_client = client;
			_config = config;
		}
        public async Task SendPlatformToCommandService(PlatformDTO platform)
		{
			var payload = new StringContent(
				JsonSerializer.Serialize(platform),
				Encoding.UTF8, 
				"application/json");
			var response = await _client.PostAsync($"{_config["CommandServiceIP"]}/api/c/platforms/", payload);
			if (response.IsSuccessStatusCode)
			{
				Console.WriteLine("--> Sync POST to Command Service from Platfrom service is successfull");
			}
			else
			{
				Console.WriteLine("XXX Sync POST to Command Service from Platfrom service has failed");
			}

		}
	}
}
