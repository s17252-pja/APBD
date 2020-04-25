using Wyklad5.Helpers;
using Wyklad5.DTOs.RequestModels;
using Wyklad5.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Wyklad5.Controllers
{
    [ApiController]
    [Route("api/enrollments")]
    public class EnrollmentsController : ControllerBase
    {

        private readonly IStudentDbService _dbService;

        public EnrollmentsController(IStudentDbService dbService)
        {
            _dbService = dbService;
        }

        [HttpPost]
        public IActionResult CreateStudent(StudentWithStudiesRequest request)
        {
            try
            {
                return StatusCode(201,_dbService.CreateStudentWithStudies(request));
            }
            catch (DbServiceException e)
            {
                if (e.Type == DbServiceExceptionTypeEnum.NotFound)
                    return NotFound(e.Message);
                else if (e.Type == DbServiceExceptionTypeEnum.ValueNotUnique)
                    return BadRequest(e.Message);
                else
                    return StatusCode(500);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPost("promotions")]
        public IActionResult PromoteStudents(PromotionRequest request)
        {
            if (!_dbService.CheckIfEnrollmentExists(request.Studies, request.Semester))
                return NotFound("Enrollment not found.");

            try
            {
                return Ok(_dbService.PromoteStudents(request.Studies, request.Semester));
            }
            catch (DbServiceException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.ToString());
            }
        }
    }
}