using Wyklad5.Services;
using Wyklad5.Models;
using Microsoft.AspNetCore.Mvc;

namespace Wyklad5.Controllers
{
    [ApiController]
    [Route("api/enrollments")]
    public class EnrollmentsController : ControllerBase
    {
        private readonly IDbService _dbService;

        public EnrollmentsController(IDbService dbService)
        {
            _dbService = dbService;
        }

        [HttpPost]
        public IActionResult CreateEnrollment(CreateEnrollmentDto createEnrollmentDto)
        {
            try
            {
                Enrollment enrollment = _dbService.EnrollStudent(createEnrollmentDto);
                return Created($"/api/enrollments/{enrollment.IdEnrollment}", enrollment);
            }
            catch (StudiesNotFoundException e)
            {
                return BadRequest(new BadRequestDto("Provided studies not found!"));
            }
            catch (StudentAlreadyExistsException e)
            {
                return BadRequest(new BadRequestDto("Student with such Index Number already exist!"));
            }
        }
    }
}