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
		public void IsAbsentInvalid()
		{
			var testStudent = GetTestStudents();
			var controller = new StudentController(_logger);

			// add student
			var request = controller.AddStudent(testStudent);
			Assert.True(request[0].response);

			// add class for student
			request = controller.AddClass(testStudent);
			Assert.True(request[0].response);

			// add seat
			request = controller.AddSeatAPI(testStudent);
			Assert.True(request[0].response);

			// cleanup
			request = controller.RemoveStudent(testStudent);
			Assert.True(request[0].response);
		}*/

		[Fact]
		public void GetStudent()
		{
			var testStudent = GetTestStudents();
			var controller = new StudentController(_logger);

			// attempt to get student
			controller.RemoveStudent(testStudent);
			var request = controller.GetStudentAPI(testStudent);
			Assert.False(request[0].response);

			// add student
			request = controller.AddStudent(testStudent);
			Assert.True(request[0].response);

			// get student
			request = controller.GetStudentAPI(testStudent);
			Assert.True(request[0].response);

			// cleanup
			request = controller.RemoveStudent(testStudent);
			Assert.True(request[0].response);
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
						email = "email1@gmail.com",
						pass = "password",
						response = false
					});

			return testStudents;
		}
	}
}
