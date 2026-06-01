using Learn2Gether.Application.Interfaces;
using Learn2Gether.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Learn2Gether.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorController : BaseController
    {
        private readonly IInstructorService _instructorService;
        public InstructorController(UserManager<User> userManager, IInstructorService instructorService) 
            : base(userManager)
        {
            _instructorService = instructorService;
        }

        /// <summary>
        /// A list of courses owned by the current Instructor.
        /// </summary>
        /// <returns>A list of Courses.</returns>
        [HttpGet]
        [Authorize(Roles = "Instructor")]
        [Route("management")]
        public async Task<IActionResult> GetOwnedCourses()
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return Unauthorized("User is not authorized");
            }

            var ownedCourses = await _instructorService.GetAllOwnedCoursesAsync(user.Id);
            return Ok(ownedCourses);
        }

        /// <summary>
        /// Get details of a specific course owned by the current Instructor, able to be edited or deleted.
        /// </summary>
        /// <param name="courseId">The Course GUID.</param>
        /// <returns>The details of the specific Course.</returns>
        [HttpGet]
        [Authorize(Roles = "Instructor")]
        [Route("management/{courseId:guid}")]
        public async Task<IActionResult> GetOwnedCourseById(string courseId)
        {
            Guid courseIdGuid = Guid.Empty;
            if(!IsGuidValid(courseId, ref courseIdGuid))
            {
                return BadRequest("Invalid course ID format");
            }

            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return Unauthorized("User is not authorized");
            }

            var course = await _instructorService.GetOwnedCourseByIdAsync(courseIdGuid, user.Id);
            if (course == null)
            {
                return NotFound("Course not found");
            }

            return Ok(course);
        }
    }
}