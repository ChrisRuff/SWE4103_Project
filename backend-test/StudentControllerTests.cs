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
		private readonly ILogger<StudentController> _studentLogger;
		private readonly ILogger<ClassController> _classLogger;

		[Fact]
		public void AddStudent()
		{
			var testStudents = GetTestStudents();
			var controller = new StudentController(_studentLogger);
			DatabaseConnector.Connector.RemoveStudent(testStudents[0].email);
			var request = controller.AddStudent(testStudents);

			Assert.True(request[0].response);
		}
		
		[Fact]
		public void RemoveStudent()
        {
			var testStudents = GetTestStudents();
			var controller = new StudentController(_studentLogger);
			DatabaseConnector.Connector.RemoveStudent(testStudents[0].email);
			var request = controller.AddStudent(testStudents);
			

					
			if (request[0].response)
            {
				var request2 = controller.RemoveStudent(testStudents);
				Assert.True(request2[0].response);
			}
			
		}

		[Fact]
		public void RemoveClassFromStudent()
		{
			var testClass = GetTestClass();
			var testStudent = GetTestStudents();
			var studentController = new StudentController(_studentLogger);
			var classController = new ClassController(_classLogger);

			// add student
			DatabaseConnector.Connector.RemoveStudent(testStudent[0].email);
			var request = studentController.AddStudent(testStudent);
			Assert.True(request[0].response);

			// make class
			DatabaseConnector.Connector.RemoveClass(testClass[0].className);
			var request2 = classController.MakeClassAPI(testClass);
			Assert.True(request2[0].response);

			//add class
			request = studentController.AddClass(testStudent);
			Assert.True(request[0].response);

			// get student, assert class attached to student
			request = studentController.GetStudentAPI(testStudent);
			Assert.True(request[0].classes.Length == 1);

			// remove class from student
			request = studentController.RemoveClassFromStudent(testStudent);
			Assert.True(request[0].response);

			// get student
			request = studentController.GetStudentAPI(testStudent);
			//Assert.True(request[0].classes.Length == 0);

			// cleanup
			classController.RemoveClassAPI(testClass);
			request = studentController.RemoveStudent(testStudent);
			Assert.True(request[0].response);

		}
		/*
		[Fact]
		public void IsAbsentInvalid()
		{
			var testStudent = GetTestStudents();
			var controller = new StudentController(_studentLogger);

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
			var controller = new StudentController(_studentLogger);

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
		[Fact]
		public void AddAndLoginStudent()
		{
			var testStudents = GetTestStudents();
			var studentController = new StudentController(_studentLogger);

			// add prof
			var request = studentController.AddStudent(testStudents);
			Assert.True(request[0].response);

			// add prof again and fail
			request = studentController.AddStudent(testStudents);
			Assert.False(request[0].response);

			// login prof
			request = studentController.LoginStudent(testStudents);
			Assert.True(request[0].response);

			// cleanup
			request = studentController.RemoveStudent(testStudents);
			Assert.True(request[0].response);
		}

		private List<StudentDTO> GetTestStudents()
		{
			var testStudents = new List<StudentDTO>();
			testStudents.Add(
					new StudentDTO
					{
						name = "Tes Student",
						classes = new ClassDTO[]
						{
							new ClassDTO
							{
								className = "TEST1301",
								width = 100,
								height = 32,
								seat = new SeatDTO
								{
									x = 5,
									y = 10
								}
							}
						},
						email = "email1@gmil.com",
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
						className = "TEST1301",
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

		[Fact]
		public void AddClass()
		{
			var testStudents = GetTestStudents();
			var studentController = new StudentController(_studentLogger);

			DatabaseConnector.Connector.RemoveStudent(testStudents[0].email);
			var request = studentController.AddStudent(testStudents);
			if (request[0].response)
			{
				var request2 = studentController.AddClass(testStudents);
				Assert.True(request2[0].response);
			}

			// cleanup
			studentController.RemoveStudent(testStudents);
		}
	}
}
