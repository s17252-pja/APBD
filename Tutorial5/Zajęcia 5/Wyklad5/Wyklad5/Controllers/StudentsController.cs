using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Wyklad5.Models;
using Wyklad5.Services;

namespace Wyklad5.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        private const string ConString = "Data Source=db-mssql;Initial Catalog=s17252;Integrated Security=True";

        private IStudentsDal _dbService;

        public StudentsController(IStudentsDal dbService)
        {
            _dbService = dbService;
        }

        [HttpGet]
        public IActionResult GetStudents([FromServices] IStudentsDal dbService)
        {
            var students = new List<Student>();

            using (SqlConnection con = new SqlConnection(ConString))
            using (SqlCommand com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "select firstname, lastname, birthdate, name, semester from student, enrollment, studies where student.idenrollment = enrollment.idenrollment and studies.idstudy = enrollment.idstudy";

                con.Open();
                SqlDataReader dr = com.ExecuteReader();
                while (dr.Read())
                {
                    var st = new Student();
                    st.FirstName = dr["FirstName"].ToString();
                    st.LastName = dr["LastName"].ToString();
                    st.BirthDate = dr["BirthDate"].ToString();
                    st.IdEnrollment = new Enrollment
                    {
                        Semester = (int)(dr["Semester"]),
                        Study = new Studies { Name = dr["Name"].ToString() }
                    };
                    students.Add(st);
                }

                con.Dispose();
            }

            return Ok(students);
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