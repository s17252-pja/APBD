using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Wyklad5.DTOs.Requests;
using Wyklad5.DTOs.Responses;
using Wyklad5.Models;
using Wyklad5.Services;

namespace Wyklad5.Controllers
{
    [Route("api/enrollments")]
    [ApiController] //-> implicit model validation
    public class EnrollmentsController : ControllerBase
    {

        private const string ConString = "Data Source=db-mssql;Initial Catalog=s17252;Integrated Security=True";

        private IStudentDbService _service;

        public EnrollmentsController(IStudentDbService service)
        {
            _service = service;
        }


        [HttpPost]
        public IActionResult EnrollStudent(EnrollStudentRequest request)
        {
            _service.EnrollStudent(request);
            var response = new EnrollStudentResponse();
/*
            using (SqlConnection con = new SqlConnection(ConString))
            using (SqlCommand com = new SqlCommand())
            {
                //response.LastName = st.LastName;
                //...
                com.Connection = con;
                com.CommandText = request.ToString();

                con.Open();
                SqlDataReader dr = com.ExecuteReader();
                while (dr.Read())
                {
                    var enroll = new Enrollment();
                    enroll.
                    enroll.LastName = dr["LastName"].ToString();
                    enroll.BirthDate = dr["BirthDate"].ToString();
                    enroll.Enrollment = new Enrollment
                    {
                        Semester = (int)(dr["Semester"]),
                        Study = new Studies { Name = dr["Name"].ToString() }
                    };
                    students.Add(st);
                }

                con.Dispose();
            }
    */        

            return Ok(response);
        }

        //..

        //..


    }
}