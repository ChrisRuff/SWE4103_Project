
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using backend.Controllers.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace backend
{
	[ApiController]
	public class ClassController : ControllerBase
	{
		private readonly ILogger<ClassController> _logger;

		public ClassController(ILogger<ClassController> logger)
		{
			_logger = logger;
		}

		[HttpPost, Route("api/class/get")]
		public List<ClassDTO> GetClass(List<ClassDTO> classes)
		{
			for(int i = 0; i < classes.Count; ++i)
			{
				classes[i] = DatabaseConnector.Connector.GetClass(classes[i].className);
			}
			return classes;
		}
	}
}
