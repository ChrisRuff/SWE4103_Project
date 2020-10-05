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
    public List<StudentDTO> Post(List<StudentDTO> students)
    {
			for (int i=0; i < students.Count; i++)
			{
				bool res = DatabaseConnector.Connector.AddStudent(students[i].studentName, students[i].email);
				students[i].response = res;
			}
      return students;
    }
  }
}
