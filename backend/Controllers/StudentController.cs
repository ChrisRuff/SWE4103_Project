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
     	string studentName = students[0].studentName;
			string[] classNames = students[0].classNames;
			string email = students[0].email;
			
			// Return bool
			bool res = DatabaseConnector.Connector.AddStudent(studentName, classNames, email);
			
			students[0].response = true;
	
      return students;
    }
  }
}
