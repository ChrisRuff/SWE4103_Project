using System;
using Xunit;
using backend.Controllers.Models;
using backend;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace test
{
	public class InviteLinkControllerTests
	{
		private readonly ILogger<InviteLinkController> _logger;
		private readonly ILogger<ClassController> _classLogger;

		[Fact]
		public void GenerateClassCode()
		{
			var testClass = GetTestClasses();
			var controller = new InviteLinkController(_logger);
			var classController = new ClassController(_classLogger);

			// generate key for nonexistant class
			var request = controller.GenerateClassCodeAPI(testClass);
			Assert.False(request[0].response);

			// make class
			request = classController.MakeClassAPI(testClass);
			Assert.True(request[0].response);

			// generate key for class
			request = controller.GenerateClassCodeAPI(testClass);
			Assert.True(request[0].response);
			Console.WriteLine("\n\n" + request[0].classCode);

			// delete class
			request = classController.RemoveClassAPI(testClass);
		}	

		private List<StudentDTO> GetTestStudents()
		{
			var testStudents = new List<StudentDTO>();
			testStudents.Add(
					new StudentDTO
					{
						studentName = "Test Student",
						classes = new ClassDTO[]
						{
							new ClassDTO
							{
								className = "TEST1001",
								width = 100,
								height = 32,
								seat = new SeatDTO
								{
									x = 5,
									y = 10
								}
							}
						},
						email = "email@email.com",
						pass = "password",
						response = false
					});

			return testStudents;
		}

		private List<ClassDTO> GetTestClasses()
		{
			var testClass = new List<ClassDTO>();
			testClass.Add(
					new ClassDTO
					{
						className = "TEST1001",
						width = 100,
						height = 32,
						seat = new SeatDTO
						{
							x = 5,
							y = 10
						}
					});

			return testClass;
		}
	}
}
