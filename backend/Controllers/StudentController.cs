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
                bool res = DatabaseConnector.Connector.AddStudent(students[i].studentName, students[i].email, students[i].pass);
                students[i].response = res;
            }
            return students;
        }

        [HttpPost, Route("api/student/seat/add")]
        public List<StudentDTO> AddSeatAPI(List<StudentDTO> students)
        {
            for (int i = 0; i < students.Count; i++)
            {
                if (!DatabaseConnector.Connector.CheckPass(students[i].email, students[i].pass))
                {
                    students[i].response = false;
                    continue;
                }
                else
                {
                    students[i].response = true;
                    for (int j = 0; j < students[i].classes.Length; j++)
                    {
                        bool res = DatabaseConnector.Connector.AddSeat(students[i].email,
                            students[i].classes[j].className, students[i].classes[j].seat.x, students[i].classes[j].seat.y);
                        students[i].response &= res;
                    }
                }
            }
            return students;
        }

        [HttpPost, Route("api/student/seat/get")]
        public StudentDTO GetSeatAPI(StudentDTO student)
        {
            if (!DatabaseConnector.Connector.CheckPass(student.email, student.pass))
                student.response = false;
            else
            {
                student.response = true;
                try
                {
                    for (int i = 0; i < student.classes.Length; i++)
                    {
                        int[] res = DatabaseConnector.Connector.GetSeat(student.email,
                            student.classes[i].className);
                        student.classes[i].seat.x = res[0];
                        student.classes[i].seat.y = res[1];
                    }
                }
                catch (Exception)
                {
                    student.response = false;
                }
            }
            return student;
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
        

        [HttpPost, Route("api/student/get")]
        public StudentDTO GetStudentAPI(StudentDTO student)
        {
            try
            {
                if (!DatabaseConnector.Connector.CheckPass(student.email, student.pass))
                    student.response = false;
                else
                {
                    student = DatabaseConnector.Connector.GetStudent(student.email);
                    student.response = true;
                }
            }
            catch (Exception)
            {
                student.response = false;   
            }
            return student;
        }

        [HttpPost, Route("api/student/class/add")]
        public ClassDTO MakeClassAPI(ClassDTO classDTO)
        {
            bool res = DatabaseConnector.Connector.MakeClass(classDTO.className,
                classDTO.width, classDTO.height);
            classDTO.response = res;

            return classDTO;
        }

        [HttpPost, Route("api/student/seat/disable")]
        public ClassDTO DisableSeatAPI(ClassDTO classDTO)
        {
            bool res = DatabaseConnector.Connector.DisableSeat(classDTO.className,
                classDTO.seat.x, classDTO.seat.y);
            classDTO.response = res;

            return classDTO;
        }

        [HttpPost, Route("api/student/seat/reserve")]
        public ClassDTO ReserveSeatAPI(ClassDTO classDTO)
        {
            bool res = DatabaseConnector.Connector.ReserveSeat(classDTO.className,
                classDTO.seat.x, classDTO.seat.y);
            classDTO.response = res;

            return classDTO;
        }

        [HttpPost, Route("api/student/class/remove")]
        public ClassDTO RemoveClassAPI(ClassDTO classDTO)
        {
            bool res = DatabaseConnector.Connector.RemoveClass(classDTO.className);
            classDTO.response = res;

            return classDTO;
        }
    }
}