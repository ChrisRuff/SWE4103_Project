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

			classController.RemoveClassAPI(testClass);

			// generate key for nonexistant class
			var request = controller.GenerateClassCodeAPI(testClass);
			Assert.False(request[0].response);

			// make class
			request = classController.MakeClassAPI(testClass);
			Assert.True(request[0].response);

			// generate key for class
			request = controller.GenerateClassCodeAPI(testClass);
			Assert.True(request[0].response);

			// delete class
			request = classController.RemoveClassAPI(testClass);
		}

		[Fact]
		public void GetClassCode()
		{
			var testClass = GetTestClasses();
			var controller = new InviteLinkController(_logger);
			var classController = new ClassController(_classLogger);

			// get invalid class from key
			testClass[0].className = "INVALID_CLASS";
			var request = controller.GetClassCodeAPI(testClass);
			Assert.False(request[0].response);
			testClass[0].className = "TEST1003";

			// get class from invalid key
			testClass[0].classCode = "INVALID_CODE";
			request = controller.GetClassCodeAPI(testClass);
			Assert.False(request[0].response);

			// make class
			classController.RemoveClassAPI(testClass);
			request = classController.MakeClassAPI(testClass);
			Assert.True(request[0].response);

			// generate key for class
			request = controller.GenerateClassCodeAPI(testClass);
			Assert.True(request[0].response);
			var classCode1 = request[0].classCode;

			// get key
			testClass[0].classCode = classCode1;
			request = controller.GetClassCodeAPI(testClass);
			Assert.True(request[0].response);
			Assert.True(classCode1 == request[0].classCode);
			Assert.True(testClass[0].className == request[0].className);

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
						className = "TEST1003",
						width = 100,
						classCode = "",
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
