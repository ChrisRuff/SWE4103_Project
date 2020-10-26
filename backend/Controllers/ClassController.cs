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
    public class ClassController : ControllerBase
    {
        private readonly ILogger<ClassController> _logger;

        public ClassController(ILogger<ClassController> logger)
        {
            _logger = logger;
        }

        [HttpPost, Route("api/class/add")]
        public List<ClassDTO> MakeClassAPI(List<ClassDTO> classDTOs)
        {
            for (int i = 0; i < classDTOs.Count; i++)
            {
                bool res = DatabaseConnector.Connector.MakeClass(classDTOs[i].className,
                            classDTOs[i].width, classDTOs[i].height);
                classDTOs[i].response = res;
            }
            return classDTOs;
        }

        [HttpPost, Route("api/student/class/remove")]
        public List<ClassDTO> RemoveClassAPI(List<ClassDTO> classDTOs)
        {
            for (int i = 0; i < classDTOs.Count; i++)
            {
                bool res = DatabaseConnector.Connector.RemoveClass(classDTOs[i].className);
                classDTOs[i].response = res;
            }
            return classDTOs;
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
