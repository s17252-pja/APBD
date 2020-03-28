using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lect_3.Models;
using Lect_3.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lect_3.Controllers
{
    [Route("api/students")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private IDbService _dbService;

        public StudentsController(IDbService dbService)
        {
            this._dbService = dbService;
        }

        //2. Passing the data by QueryString = limited, friendly urls
        //   data within query string is encoded
        [HttpGet]
        public IActionResult GetStudents(string orderBy = "firstName") //action method
        {
            return Ok(_dbService.GetStudents());
        }

        //1. How to pass data using URL segment?
        [HttpGet("{id}")]
        public IActionResult GetStudent(int id) //action method
        {
            var student = _dbService.Get(id);
            if (student != null)
            {
                return Ok(student.LastName);
            }
            return NotFound("Student was not found");
        }

        //3. Passing the data using Body (usually POST)

        [HttpPost]
        public IActionResult CreateStudent(Student student)
        {
            student.IndexNumber = $"s{new Random().Next(1, 20000)}";
            //save into db

            return Ok(student);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(int id)
        {
            if (_dbService.Delete(id))
            {
                return Ok("Deleted successfully");
            }
            return NotFound("Student not found");
        }

        [HttpPut("{id}")]
        public IActionResult ReplaceStudent(int id, [FromBody] Student student)
        {

            if (_dbService.Put(id, student))
            {
                return Ok("Replaced student successfully!");
            }
            return NotFound("Student you are trying to replace was not found.");
        }

    }
}