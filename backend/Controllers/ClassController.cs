
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
						try
						{
							classes[i] = DatabaseConnector.Connector.GetClass(classes[i].className);
							classes[i].response = true;
						}
						catch(System.Exception)
						{
							classes[i].response = false;
						}
					}
					return classes;
				}
		
				[HttpGet, Route("api/class/get/all")]
		public List<String> GetAllClassesAPI()
				{
					return DatabaseConnector.Connector.GetAllClasses();
				}

		[HttpPost, Route("api/class/add")]
		public List<ClassDTO> MakeClassAPI(List<ClassDTO> classDTOs)
		{
			for (int i = 0; i < classDTOs.Count; i++)
			{
				bool res = DatabaseConnector.Connector.MakeClass(classDTOs[i].className,
							classDTOs[i].width, classDTOs[i].height);
				classDTOs[i].response = res;
			}
			return classDTOs;
		}

		[HttpPost, Route("api/student/class/remove")]
		public List<ClassDTO> RemoveClassAPI(List<ClassDTO> classDTOs)
		{
			for (int i = 0; i < classDTOs.Count; i++)
			{
				bool res = DatabaseConnector.Connector.RemoveClass(classDTOs[i].className);
				classDTOs[i].response = res;
			}
			return classDTOs;
		}

		/*
		 * Takes a object like [{"className": "CS2043", "notificationFreq": 5}]
		 */
		[HttpPost, Route("api/class/notification/set")]
		public List<ClassDTO> ChangeFreqAPI(List<ClassDTO> classes)
		{
			for(int i = 0; i < classes.Count; ++i)
			{
				classes[i].response = 
					DatabaseConnector.Connector.ChangeFreq(classes[i].className, classes[i].notificationFreq);

			}
			return classes;
		}

		/*
		 * Takes a object like [{"className": "CS2043", "mandatory": true}]
		 */
		[HttpPost, Route("api/class/set/mandatory")]
		public List<ClassDTO> SetMandatoryStatus(List<ClassDTO> classes)
		{
			for(int i = 0; i < classes.Count; ++i)
			{
				classes[i].response = 
					DatabaseConnector.Connector.setMandatory(classes[i].className, classes[i].mandatory);

			}
			return classes;
		}
	}
}
