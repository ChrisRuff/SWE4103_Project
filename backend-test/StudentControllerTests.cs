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
		public void GetSeat()
		{
			var testStudent = GetTestStudent();
			var controller = new StudentController(_logger);
			var request = controller.GetSeatAPI(testStudent);

			Assert.True(request.response);
		}

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

		private StudentDTO GetTestStudent()
		{
			var testSeat =
					new SeatDTO
					{
						x = -1,
						y = -1
					};

			ClassDTO[] testClass = {
				new ClassDTO
				{
					className = "TEST1001",
					seat = testSeat
				}
			};

			var testStudent =
					new StudentDTO
					{
						studentName = "Test Student",
						classes = testClass,
						email = "email@email.com",
						pass = "password",
						response = false
					};

			return testStudent;
		}
	}
}
