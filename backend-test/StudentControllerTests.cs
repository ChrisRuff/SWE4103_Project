using System;
using Xunit;
using backend.Controllers.Models;
using backend;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace test
{
	public class StudentControllerTests
	{
		private readonly ILogger<StudentController> _logger;

		[Fact]
		public void AddStudent()
		{
			var testStudents = GetTestStudents();
			var controller = new StudentController(_logger);
			DatabaseConnector.Connector.RemoveStudent(testStudents[0].email);
			var request = controller.AddStudent(testStudents);

			Assert.True(request[0].response);
		}
		//remove student test
		[Fact]
		public void RemoveStudent()
        {
			var testStudents = GetTestStudents();
			var controller = new StudentController(_logger);
			DatabaseConnector.Connector.RemoveStudent(testStudents[0].email);
			var request = controller.AddStudent(testStudents);
			

					
			if (request[0].response)
            {
				var request2 = controller.RemoveStudent(testStudents);
				Assert.True(request2[0].response);
			}
			
		}

		/*
		[Fact]
		public void GetSeat()
		{
			var testStudent = GetTestStudents();
			var controller = new StudentController(_logger);
			var request = controller.GetSeatAPI(testStudent[0]);

			Assert.False(request.response);
		}

		[Fact]
		public void IsAbsent()
		{
			var testStudent = GetTestStudents();
			var controller = new StudentController(_logger);
			var request = controller.GetSeatAPI(testStudent[0]);

			Assert.False(request.response);
		}
		*/

		private List<StudentDTO> GetTestStudents()
		{
			var testStudents = new List<StudentDTO>();
			testStudents.Add(
					new StudentDTO
					{
						studentName = "Test Student",
						classes = null,
						email = "email@email.com",
						pass = "password",
						response = false
					});

			return testStudents;
		}
	}
}
