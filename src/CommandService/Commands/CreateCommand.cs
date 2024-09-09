﻿using System.ComponentModel.DataAnnotations;

namespace CommandService.Commands
{
	public class CreateCommand
	{
		[Required]
		public string HowTo { get; set; }
		[Required]
		public string CommandLine { get; set; }
	}
}
