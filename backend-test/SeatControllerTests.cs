using System;
using Xunit;
using backend.Controllers.Models;
using backend;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace test
{
	public class SeatControllerTests
	{
		private readonly ILogger<SeatController> _seatLogger;
		private readonly ILogger<StudentController> _studentLogger;
		private readonly ILogger<ClassController> _classLogger;

		[Fact]
		public void AddAndGetSeat()
		{
			var testStudent = GetTestStudents();
			var seatController = new SeatController(_seatLogger);
			var studentController = new StudentController(_studentLogger);
			var classController = new ClassController(_classLogger);

			// attempt to get seat
			var request = seatController.GetSeatAPI(testStudent);
			Assert.False(request[0].response);

			// add student
			studentController.RemoveStudent(testStudent);
			request = studentController.AddStudent(testStudent);
			Assert.True(request[0].response);

			// add class for student
			request = classController.AddClass(testStudent);
			Assert.True(request[0].response);

			// add seat
			request = seatController.AddSeatAPI(testStudent);
			Assert.True(request[0].response);

			// get seat
			request = seatController.GetSeatAPI(testStudent);
			Assert.True(request[0].response);

			// cleanup
			request = studentController.RemoveStudent(testStudent);
			Assert.True(request[0].response);
		}

		[Fact]
		public void DisableSeat()
		{
			var testClass = GetTestClass();
			var seatController = new SeatController(_seatLogger);
			var classController = new ClassController(_classLogger);

			// attempt to disable non existent seat
			var request = seatController.DisableSeatAPI(testClass);
			Assert.False(request[0].response);

			// add class
			request = classController.MakeClassAPI(testClass);
			Assert.True(request[0].response);

			// disable a seat
			request = seatController.DisableSeatAPI(testClass);
			Assert.True(request[0].response);

			// attempt to disable same seat
			request = seatController.DisableSeatAPI(testClass);
			Assert.False(request[0].response);

			// cleanup
			request = classController.RemoveClassAPI(testClass);
		}

		[Fact]
		public void ReserveSeat()
		{
			var testClass = GetTestClass();
			var seatController = new SeatController(_seatLogger);
			var classController = new ClassController(_classLogger);

			// add class
			var request = classController.MakeClassAPI(testClass);
			Assert.True(request[0].response);

			// reserve a seat
			request = seatController.ReserveSeatAPI(testClass);
			Assert.True(request[0].response);

			// attempt to reserve same seat
			request = seatController.ReserveSeatAPI(testClass);
			Assert.False(request[0].response);

			// cleanup
			request = classController.RemoveClassAPI(testClass);
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
