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
		private readonly ILogger<ProfController> _logger;

		[Fact]
		public void AddProf()
		{
			var testProfs = GetTestProfs();
			var controller = new ProfController(_logger);
			DatabaseConnector.Connector.RemoveProf(testProfs[0].email);
			var request = controller.AddProfAPI(testProfs);

			Assert.True(request[0].response);
		}	
		
		[Fact]
		public void RemoveProf()
		{
			var testProfs = GetTestProfs();
			var controller = new ProfController(_logger);
			DatabaseConnector.Connector.RemoveProf(testProfs[0].email);
			controller.AddProfAPI(testProfs);
			
			var request = controller.RemoveProfAPI(testProfs);
			Assert.True(request[0].response);
		}
		[Fact]
		public void AddAndLoginProf()
		{
			var testProfs = GetTestProfs();
			var profController = new ProfController(_logger);

			// add prof
			DatabaseConnector.Connector.RemoveProf(testProfs[0].email);
			var request = profController.AddProfAPI(testProfs);
			Assert.True(request[0].response);

			// add prof again and fail
			request = profController.AddProfAPI(testProfs);
			Assert.False(request[0].response);

			// login prof
			request = profController.LoginProf(testProfs);
			Assert.True(request[0].response);
		}

		[Fact]
		public void GetProfClasses()
		{
			var testProfs = GetTestProfs();
			var controller = new ProfController(_logger);

			// attempt to get student
			controller.RemoveProfAPI(testProfs);
			controller.AddProfAPI(testProfs);
			controller.AddClassAPI(testProfs);
			var request = controller.GetClassesAPI(testProfs);
			Assert.True(request[0] == "TEST1001" && request[1] == "TEST1002");

			// cleanup
			var request2 = controller.RemoveProfAPI(testProfs);
			Assert.True(request2[0].response);
		}

		private List<ProfDTO> GetTestProfs()
		{
			var testProfs = new List<ProfDTO>();
			testProfs.Add(
					new ProfDTO
					{
						name = "Test Prof",
						classes = new ClassDTO[]
						{
							new ClassDTO
							{
								className = "TEST1001",
							},
							new ClassDTO
							{
								className = "TEST1002",
							}
						},
						email = "email@email.com",
						pass = "password",
						response = false
					});

			return testProfs;
		}
	}
}
