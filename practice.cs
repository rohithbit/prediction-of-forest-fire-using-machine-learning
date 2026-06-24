StudentService.cs
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnetapp.Models;
 
namespace dotnetapp.Services
{
    public class StudentService
    {
        private readonly List<Student> students;
        public StudentService(){
            students = new List<Student>(){
                new Student{
                    StudentId = 1,
                    Name = "Alice",
                    Age = 18,
                    Grade = "A"
                },
                new Student{
                    StudentId = 2,
                    Name = "Bob",
                    Age = 17,
                    Grade = "B"
                },
                new Student{
                    StudentId = 3,
                    Name = "Charlie",
                    Age = 16,
                    Grade = "C"
                }
            };
        }
        public List<Student> GetAllStudents(){
            return students;
        }
        public Student GetStudentById(int studentId){
            return students.FirstOrDefault(s => s.StudentId == studentId);
        }
        public Student CreateStudent(Student newStudent){
            students.Add(newStudent);
            return newStudent;
        }
        public bool UpdateStudent(int studentId, Student updatedStudent){
            var student = students.FirstOrDefault(s => s.StudentId == studentId);
            if(student == null){
                return false;
            }
            student.Name = updatedStudent.Name;
            student.Age = updatedStudent.Age;
            student.Grade = updatedStudent.Grade;
            return true;
        }
        public bool DeleteStudent(int studentId){
            var student = students.FirstOrDefault(s => s.StudentId == studentId);
            if(student == null){
                return false;
            }
            students.Remove(student);
            return true;
        }
    }
}
 
StudentController.cs
 
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
 
using dotnetapp.Services;
using dotnetapp.Models;
 
namespace dotnetapp.Controllers
{   [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly StudentService db;
        public StudentController(StudentService db1){
            db = db1;
        }
        [HttpGet]
        public IActionResult GetAllStudents(){
            var students = db.GetAllStudents();
            if(students == null || students.Count == 0){
                return NoContent();
            }
            return Ok(students);
        }
        [HttpGet("{studentId}")]
        public IActionResult GetStudentById(int studentId){
            var student = db.GetStudentById(studentId);
            if(student == null){
                return NotFound();
            }
            return Ok(student);
        }
        [HttpPost]
        public IActionResult CreateStudent(Student newStudent){
            if(newStudent == null){
                return BadRequest();
            }
            db.CreateStudent(newStudent);
            return Created("", newStudent);
        }
        [HttpPut("{studentId}")]
        public IActionResult UpdateStudent(int studentId, Student updatedStudent){
            var res = db.UpdateStudent(studentId,updatedStudent);
            if(!res){
                return NotFound();
            }
            return NoContent();
        }
        [HttpDelete("{studentId}")]
        public IActionResult DeleteStudent(int studentId){
            var res = db.DeleteStudent(studentId);
            if(!res){
                return NotFound();
            }
            return NoContent();
        }
    }
 
using dotnetapp.Services;
using dotnetapp.Models;
 
var builder = WebApplication.CreateBuilder(args);
 
// Add Event services to the container.
 
builder.Services.AddControllers();
builder.Services.AddSingleton<StudentService>(); // change this line here
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
 
var app = builder.Build();
 
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
 
app.UseHttpsRedirection();
 
app.UseAuthorization();
 
app.MapControllers();
 
app.Run();
 
using dotnetapp.Services;
using dotnetapp.Models;
 
var builder = WebApplication.CreateBuilder(args);
 
// Add Event services to the container.
 
builder.Services.AddControllers();
builder.Services.AddSingleton<StudentService>(); // change this line here
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
 
var app = builder.Build();
 
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
 
app.UseHttpsRedirection();
 
app.UseAuthorization();
 
app.MapControllers();
 
app.Run();
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
 
namespace dotnetapp.Models
{
    public class Student
    {
        public int StudentId{get;set;}
        public string Name {get;set;}
        public int Age{get;set;}
        public string Grade {get;set;}
    }
}
 
