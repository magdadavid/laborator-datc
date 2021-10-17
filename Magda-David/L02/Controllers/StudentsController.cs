using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Respositories;
using Models;

namespace L02.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentsController : ControllerBase
    {
       
        public StudentsController(){}

        [HttpGet]
        public IEnumerable<Student> Get()
        {
            return StudentsRepo.Students;
        }

        [HttpGet("{id}")]
        public Student Get(int id)
        {
            return StudentsRepo.Students.FirstOrDefault(s => s.Id == id);
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var del_student = StudentsRepo.Students.FirstOrDefault(s => s.Id == id);
            StudentsRepo.Students.Remove(del_student);
        }

        [HttpPost]
        public Student Post(Student student_post)
        {
            StudentsRepo.Students.Add(student_post);
            return student_post;
        }

        [HttpPut("{id}")]
        public Student Put(int id, Student student_put)
        {
            var edit_student = StudentsRepo.Students.FirstOrDefault(s => s.Id == id);
            edit_student.Name = student_put.Name;
            edit_student.Faculty = student_put.Faculty;
            edit_student.StudyYear = student_put.StudyYear;
            return edit_student;
        }
    }
}
