using System;
using Xunit;
using backend.Controllers.Models;
using backend;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace test
{
	public class ClassControllerTests
	{
		private readonly ILogger<ClassController> _classLogger;
		private readonly ILogger<StudentController> _studentLogger;

		[Fact]
		public void MakeAndRemoveClass()
		{
			var testClass = GetTestClass();
			var classController = new ClassController(_classLogger);

			// add class
			DatabaseConnector.Connector.RemoveClass(testClass[0].className);

			var request = classController.MakeClassAPI(testClass);
			Assert.True(request[0].response);

			// attempt to add class again
			request = classController.MakeClassAPI(testClass);
			Assert.False(request[0].response);

			// delete class
			request = classController.RemoveClassAPI(testClass);
			Assert.True(request[0].response);

			// attempt to delete class again
			request = classController.RemoveClassAPI(testClass);
			Assert.False(request[0].response);
		}

		[Fact]
		public void ChangeNotificationFrequency()
		{
			var testClass = GetTestClass();
			var classController = new ClassController(_classLogger);
			classController.MakeClassAPI(testClass);
			var classObject = DatabaseConnector.Connector.GetClass(testClass[0].className);

			Assert.True(classObject.notificationFreq == 3);
			testClass[0].notificationFreq = 5;
			classController.ChangeFreqAPI(testClass);

			classObject = DatabaseConnector.Connector.GetClass(testClass[0].className);
			Assert.True(classObject.notificationFreq == 5);

			classController.RemoveClassAPI(testClass);

		}


		private List<StudentDTO> GetTestStudents()
		{
			var testStudents = new List<StudentDTO>();
			testStudents.Add(
					new StudentDTO
					{
						name = "Test Student",
						classes = new ClassDTO[]
						{
							new ClassDTO
							{
								className = "TEST101",
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

		private List<ClassDTO> GetTestClass()
		{
			var testClass = new List<ClassDTO>();
			testClass.Add(
					new ClassDTO
					{
						className = "TEST101",
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
