using System;
using backend.Controllers.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
namespace backend
{
	[ApiController]
	public class ProfController : ControllerBase
	{
		private readonly ILogger<ProfController> _logger;

		public ProfController(ILogger<ProfController> logger)
		{
			_logger = logger;
		}

		[HttpPost, Route("api/prof/add")]
		public List<ProfDTO> AddProfAPI(List<ProfDTO> profs)
		{
			for (int i = 0; i < profs.Count; i++)
			{
				bool res = DatabaseConnector.Connector.AddProf(profs[i].name, profs[i].email, profs[i].pass);
				profs[i].response = res;
			}
			return profs;
		}

		[HttpPost, Route("api/prof/remove")]
		public List<ProfDTO> RemoveProfAPI(List<ProfDTO> profs)
		{
			for (int i = 0; i < profs.Count; i++)
			{
				bool res = DatabaseConnector.Connector.RemoveProf(profs[i].email);
				profs[i].response = res;
			}
			return profs;
		}

		[HttpPost, Route("api/prof/class/add")]
		public List<ProfDTO> AddClassAPI(List<ProfDTO> profs)
		{
			for (int i = 0; i < profs.Count; i++)
			{
				for (int j = 0; j < profs[i].classes.Length; j++)
				{
					bool res = DatabaseConnector.Connector.AddClassProf(profs[i].email, profs[i].classes[j].className);
					profs[i].response = res;
				}
			}
			return profs;
		}

		[HttpPost, Route("api/prof/class/get")]
		public List<String> GetClassesAPI(List<ProfDTO> profs)
		{
			try
			{
				return DatabaseConnector.Connector.GetProfClassNames(profs[0].email);
			}
			catch(System.Exception)
			{
				return new List<String>();
			}
		}

		[HttpPost, Route("api/prof/login")]
        public List<ProfDTO> LoginProf(List<ProfDTO> profs)
        {
            for (int i = 0; i < profs.Count; i++)
            {
                bool res = DatabaseConnector.Connector.CheckPassProf(profs[i].email, profs[i].pass);
                profs[i].response = res;
            }
            return profs;
        }
	}
}
