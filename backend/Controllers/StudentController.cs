using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using backend.Controllers.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace backend
{
  [ApiController]
  public class StudentController : ControllerBase
  {
	private readonly ILogger<StudentController> _logger;

	public StudentController(ILogger<StudentController> logger)
	{
	  _logger = logger;
	}

	[HttpPost, Route("api/student/add")]
	public List<StudentDTO> AddStudent(List<StudentDTO> students)
	{
	  for (int i = 0; i < students.Count; i++)
	  {
		bool res = DatabaseConnector.Connector.AddStudent(students[i].name, students[i].email, students[i].pass);
		students[i].response = res;
	  }
	  return students;
	}
	  /*
	  [HttpPost, Route("api/student/absence/get")]
	  public StudentDTO IsAbsentAPI(StudentDTO student)
	  {
		if (!DatabaseConnector.Connector.CheckPass(student.email, student.pass))
		  student.response = false;
		else
		{
		  student.response = true;
		  for (int i = 0; i < student.classes.Length; i++)
		  {
			bool res = DatabaseConnector.Connector.IsAbsent(student.email,
			  student.classes[i].className);
			student.response &= res;
		  }
		}
		return student;
	  }
	  */
	  [HttpPost, Route("api/student/remove")]
	  public List<StudentDTO> RemoveStudent(List<StudentDTO> students)
	  {
		for (int i = 0; i < students.Count; i++)
		{
		  bool res = DatabaseConnector.Connector.RemoveStudent(students[i].email);
		  students[i].response = res;
		}
		return students;
	  }

		[HttpPost, Route("api/student/class/remove")]
		public List<StudentDTO> RemoveClassFromStudent(List<StudentDTO> students)
		{
			for (int i = 0; i < students.Count; i++)
			{
				for (int j = 0; j < students[i].classes.Length; j++)
				{
					bool res = DatabaseConnector.Connector.RemoveClassFromStudent(students[i].email, students[i].classes[j].className);
					students[i].response = res;
                }
			}
			return students;
		}

		[HttpPost, Route("api/student/get")]
	  public List<StudentDTO> GetStudentAPI(List<StudentDTO> students)
	  {
		for (int i = 0; i < students.Count; i++)
		{
		  try
		  {
			if (!DatabaseConnector.Connector.CheckPassStudent(students[i].email, students[i].pass))
			{
			  students[i].response = false;
			  continue;
			}
			else
			{
			  students[i] = DatabaseConnector.Connector.GetStudent(students[i].email);
			  students[i].response = true;
			}
		  }
		  catch (Exception e)
		  {
			students[i].response = false;
		  }
		}
		return students;
	  }

	  [HttpPost, Route("api/student/login")]
		public List<StudentDTO> LoginStudent(List<StudentDTO> students)
		{
			for (int i = 0; i < students.Count; i++)
			{
				bool res = DatabaseConnector.Connector.CheckPassStudent(students[i].email, students[i].pass);
				students[i].response = res;
			}
			return students;
		}
		[HttpPost, Route("api/student/class/add")]
		public List<StudentDTO> AddClass(List<StudentDTO> students)
		{
			for (int i = 0; i < students.Count; i++)
			{

				for (int j = 0; j < students[i].classes.Length; j++)
				{
					bool res = DatabaseConnector.Connector.AddClass(students[i].email, students[i].classes[j].className);
					students[i].response = res;
				}
			}
			return students;
		}
  }
}

