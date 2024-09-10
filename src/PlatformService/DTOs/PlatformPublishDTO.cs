namespace PlatformService.DTOs
{
	public record PlatformPublishDTO(int Id, string Name)
	{
		public string Event { get; set; } = string.Empty;
	}
}
