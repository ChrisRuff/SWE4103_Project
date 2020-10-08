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

				Assert.Equal(request[0].response, true);
			}

			private List<StudentDTO> GetTestStudents()
			{
				var testStudents = new List<StudentDTO>();
				testStudents.Add(
						new StudentDTO {
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
