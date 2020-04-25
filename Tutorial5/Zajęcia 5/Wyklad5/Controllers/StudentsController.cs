using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Wyklad5.Models;
using Wyklad5.Services;

namespace Wyklad5.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentDbService _dbService;

        private const string ConString = "Data Source=db-mssql;Initial Catalog=s17252;Integrated Security=True";
        
        
        /*
        public StudentsController(IStudentsDal dbService)
        {
            _dbService = dbService;
        }

        private IDbService _dbService;
        */
        public StudentsController(IStudentDbService dbService)
        {
            _dbService = dbService;
        }

        //2. Passing the data by QueryString = limited, friendly urls
        //   data within query string is encoded
        //[HttpGet]
        //public IActionResult GetStudents(string orderBy = "firstName") //action method
        //{
        //  return Ok(_dbService.GetStudents());
        //}

        [HttpGet]
        public IActionResult GetStudents(string orderBy)
        {
            return Ok(_dbService.GetStudents());
        }
        [HttpGet("{indexNumber}")]
        public IActionResult GetStudent(string indexNumber)
        {
            var list = new List<Student>();
            using (SqlConnection con = new SqlConnection(ConString))
            using (SqlCommand com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "select FirstName,LastName,BirthDate,Studies.name,Enrollment.Semester from student inner join Enrollment on Enrollment.IdEnrollment = Student.IdEnrollment  inner join Studies on Enrollment.IdStudy = Studies.IdStudy where indexNumber=@index";

                com.Parameters.AddWithValue("index", indexNumber);

                con.Open();
                var dr = com.ExecuteReader();
                if (dr.Read())
                {
                    var en = new Enrollment();
                    var sem = en.Semester = (int)(dr["Semester"]);
                    con.Dispose();
                    return Ok("Student with index " + indexNumber + " is on semester " + sem);
                }
                else
                    con.Dispose();
                return NotFound();
            }

        }
    }
}