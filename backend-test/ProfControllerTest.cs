using System;
using Xunit;
using backend.Controllers.Models;
using backend;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace test
{
	public class ProfControllerTests
	{
		private readonly ILogger<ProfController> _profLogger;

		[Fact]
		public void AddAndLoginProf()
		{
			var testProfs = GetTestProfs();
			var profController = new ProfController(_profLogger);

			// add prof
			DatabaseConnector.Connector.RemoveProf(testProfs[0].email);
			var request = profController.AddProf(testProfs);
			Assert.True(request[0].response);

			// add prof again and fail
			request = profController.AddProf(testProfs);
			Assert.False(request[0].response);

			// login prof
			request = profController.LoginProf(testProfs);
			Assert.True(request[0].response);
		}

		private List<ProfDTO> GetTestProfs()
		{
			var testProfs = new List<ProfDTO>();
			testProfs.Add(
					new ProfDTO
					{
						profName = "mrProf",
						email = "proff@email.com",
						pass = "passyword",
						response = false
					});

			return testProfs;
		}
	}
}

