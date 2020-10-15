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

		[Fact]
		public void AddAndGetSeat()
		{
			var testStudent = GetTestStudents();
			var controller = new StudentController(_logger);
			controller.RemoveStudent(testStudent);

			// attempt to get seat
			var request2 = controller.GetSeatAPI(testStudent[0]);
			Assert.False(request2.response);

			// add student
			var request = controller.AddStudent(testStudent);
			Assert.True(request[0].response);

			// add class for student
			request = controller.AddClass(testStudent);
			Assert.True(request[0].response);

			// add seat
			request = controller.AddSeatAPI(testStudent);
			Assert.True(request[0].response);

			// get seat
			request2 = controller.GetSeatAPI(testStudent[0]);
			Assert.True(request2.response);

			// cleanup
			request = controller.RemoveStudent(testStudent);
			Assert.True(request[0].response);
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
			var request = controller.GetStudentAPI(testStudent[0]);
			Assert.False(request.response);

			// add student
			var request2 = controller.AddStudent(testStudent);
			Assert.True(request2[0].response);

			// get student
			request = controller.GetStudentAPI(testStudent[0]);
			Assert.True(request.response);

			// cleanup
			request2 = controller.RemoveStudent(testStudent);
			Assert.True(request2[0].response);
		}

		[Fact]
		public void DisableSeat()
		{
			var testClass = GetTestClass();
			var controller = new StudentController(_logger);

			// add class
			var request = controller.MakeClassAPI(testClass);
			Assert.True(request.response);

			// disable a seat
			request = controller.DisableSeatAPI(testClass);
			Assert.True(request.response);

			// attempt to disable same seat
			request = controller.DisableSeatAPI(testClass);
			Assert.False(request.response);

			// cleanup
			request = controller.RemoveClassAPI(testClass);
			Assert.True(request.response);
		}

		[Fact]
		public void ReserveSeat()
		{
			var testClass = GetTestClass();
			var controller = new StudentController(_logger);

			// add class
			var request = controller.MakeClassAPI(testClass);
			Assert.True(request.response);

			// reserve a seat
			request = controller.ReserveSeatAPI(testClass);
			Assert.True(request.response);

			// attempt to reserve same seat
			request = controller.ReserveSeatAPI(testClass);
			Assert.False(request.response);

			// cleanup
			request = controller.RemoveClassAPI(testClass);
			Assert.True(request.response);
		}

		[Fact]
		public void MakeAndRemoveClass()
		{
			var testClass = GetTestClass();
			var controller = new StudentController(_logger);

			// add class
			var request = controller.MakeClassAPI(testClass);
			Assert.True(request.response);

			// attempt to add class again
			request = controller.MakeClassAPI(testClass);
			Assert.False(request.response);

			// delete class
			request = controller.RemoveClassAPI(testClass);
			Assert.True(request.response);

			// attempt to delete class again
			request = controller.RemoveClassAPI(testClass);
			Assert.False(request.response);
		}

		[Fact]
		public void AddClass()
        {
			var testStudents = GetTestStudents();
			var controller = new StudentController(_logger);
			DatabaseConnector.Connector.RemoveStudent(testStudents[0].email);
			var request = controller.AddStudent(testStudents);
			if (request[0].response)
            {
				var request2 = controller.AddClass(testStudents);
				Assert.True(request2[0].response);
			}
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

		private ClassDTO GetTestClass()
		{
			var testClass =
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
					};

			return testClass;
		}
	}
}
