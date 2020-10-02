using System;

namespace backend.Controllers.Models
{
	// A Data Transfer Object(DTO) is used to model the data you wish to send over HTTP
	public class CustomObjectDTO
	{
		// Each component of your DTO must have what opererations can be executed on it
		// You're probably only ever going to need "get; set;"
		public string message { get; set; }
	}
}	
