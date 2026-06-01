using Learn2Gether.Application.DTOs.Requests.Course;
using Learn2Gether.Application.DTOs.Requests.CourseDTO;
using Learn2Gether.Application.Interfaces;
using Learn2Gether.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Learn2Gether.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : BaseController
    {
        private readonly ICourseService _courseService;

        public CoursesController(ICourseService courseService, UserManager<User> userManager)
            : base(userManager)
        {
            _courseService = courseService;
        }


        /// <summary>
        /// Get all courses.
        /// </summary>
        /// <param name="inputModel">The search filter and pagination parameters.</param>
        /// <returns>A list of courses matching the search criteria. If no search filter is provided, all courses are returned for each page.</returns>
        [HttpGet]
        [Route("courses")]
        public async Task<IActionResult> GetCourses([FromQuery] AllCoursesSearchFilterDTO inputModel)
        {
            var courses = await _courseService.GetAllCoursesAsync(inputModel);

            int coursesCount = courses.Count();
            int totalPages = (int)Math.Ceiling((double)coursesCount / (inputModel.EntitiesPerPage ?? 10));

            var result = new AllCoursesSearchFilterDTO
            {
                Courses = courses,
                SearchQuery = inputModel.SearchQuery,
                CurrentPage = inputModel.CurrentPage,
                EntitiesPerPage = inputModel.EntitiesPerPage,
                TotalPages = totalPages
            };

            return Ok(result);
        }

        /// <summary>
        /// Get details of a specific course by GUID.
        /// </summary>
        /// <param name="courseId">The course GUID.</param>
        /// <returns>The details of the course.</returns>
        [HttpGet]
        [Route("courses/{courseId:guid}")]
        public async Task<IActionResult> Details([FromRoute] string courseId)
        {
            Guid courseIdGuid = Guid.Empty;
            if (!IsGuidValid(courseId.ToString(), ref courseIdGuid))
            {
                return BadRequest("Invalid course ID.");
            }

            var user = await GetCurrentUserAsync();

            var course = await _courseService.GetCourseByIdAsync(courseIdGuid, user);
            if (course == null)
            {
                return NotFound("No course found with the provided ID.");
            }

            return Ok(course);
        }

        /// <summary>
        /// Authorized user with "Student" role can enroll to a course.
        /// </summary>
        /// <param name="courseId">The Course GUID.</param>
        /// <returns>OK message for successful enrollment.</returns>
        [HttpPost]
        [Authorize(Roles = "Student")]
        [Route("courses/{courseId:guid}/enroll")]
        public async Task<IActionResult> Enroll([FromRoute] string courseId)
        {
            Guid courseIdGuid = Guid.Empty;
            if (!IsGuidValid(courseId.ToString(), ref courseIdGuid))
            {
                return BadRequest("Invalid course ID.");
            }

            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return Unauthorized("User not found or email claim missing.");
            }

            bool enrollmentResult = await _courseService.EnrollToCourseAsync(courseIdGuid, user);
            if (!enrollmentResult)
            {
                return BadRequest("Enrollment failed.");
            }

            return Ok("Enrollment successful.");
        }

        /// <summary>
        /// Get list of all enrolled courses.
        /// </summary>
        /// <returns>A list of courses that the authenticated "Student" user enrolled in.</returns>
        [HttpGet]
        [Authorize(Roles = "Student")]
        [Route("my-courses")]
        public async Task<IActionResult> MyCourses()
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return Unauthorized("User not found or email claim missing.");
            }

            var myCourses = await _courseService.GetUserEnrolledCoursesAsync(user);
            return Ok(myCourses);
        }

        /// <summary>
        /// Get details of a specific enrolled course, including modules, lectures, QnA and notes.
        /// </summary>
        /// <param name="courseId">The course GUID.</param>
        /// <returns>The full details of the enrolled course.</returns>
        [HttpGet]
        [Authorize(Roles = "Student")]
        [Route("my-courses/{courseId:guid}")]
        public async Task<IActionResult> MyCourseDetails([FromRoute] string courseId)
        {
            Guid courseIdGuid = Guid.Empty;
            if (!IsGuidValid(courseId.ToString(), ref courseIdGuid))
            {
                return BadRequest("Invalid course ID.");
            }
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return Unauthorized("User not found or email claim missing.");
            }

            var course = await _courseService.GetEnrolledCourseDetailsAsync(courseIdGuid, user);
            if (course == null)
            {
                return NotFound("No course found with the provided ID.");
            }

            return Ok(course);
        }

        /// <summary>
        /// Watch a specific lecture video of an enrolled course.
        /// </summary>
        /// <param name="courseId">The course GUID.</param>
        /// <param name="lectureId">The lecture GUID.</param>
        /// <returns>The details of the lecture video, including its QnA, notes and other related description.</returns>
        [HttpGet]
        [Authorize(Roles = "Student")]
        [Route("my-courses/{courseId:guid}/{lectureId:guid}")]
        public async Task<IActionResult> MyCourseWatchLecture([FromRoute] string courseId, [FromRoute] string lectureId)
        {
            Guid courseIdGuid = Guid.Empty;
            if (!IsGuidValid(courseId.ToString(), ref courseIdGuid))
            {
                return BadRequest("Invalid course ID.");
            }

            Guid lectureIdGuid = Guid.Empty;
            if (!IsGuidValid(lectureId.ToString(), ref lectureIdGuid))
            {
                return BadRequest("Invalid lecture ID.");
            }

            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return Unauthorized("User not found or email claim missing.");
            }

            var courseLecture = await _courseService.GetWatchLectureDetailsAsync(courseIdGuid, lectureIdGuid, user);
            if (courseLecture == null)
            {
                return NotFound("No course found with the provided ID.");
            }

            return Ok(courseLecture);
        }

        /// <summary>
        /// Add a new course. Only "Instructor" can add a new course.
        /// </summary>
        /// <param name="newCourseDto">The course DTO to be mapped to the Course entity.</param>
        /// <returns>OK responce for successful addition.</returns>
        [HttpPost]
        [Authorize(Roles = "Instructor")]
        [Route("add")]
        public async Task<IActionResult> AddCourse([FromBody] AddNewCourseDTO newCourseDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return Unauthorized("User not found or email claim missing.");
            }

            var result = await _courseService.AddNewCourseAsync(newCourseDto, user);
            if (!result)
            {
                return BadRequest("Failed to add new course.");
            }
            return Ok("Course added successfully.");
        }

        /// <summary>
        /// Edit an existing course. Only "Instructor" that owns the course can edit it.
        /// </summary>
        /// <param name="editCourseDto">The DTO for editing existing course and map the changes to the entity.</param>
        /// <returns>OK responce for successful edit.</returns>
        [HttpPost]
        [Authorize(Roles = "Instructor")]
        [Route("edit")]
        public async Task<IActionResult> EditCourse([FromBody] EditCourseDTO editCourseDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Guid courseIdGuid = Guid.Empty;
            if (!IsGuidValid(editCourseDto.CourseId.ToString(), ref courseIdGuid))
            {
                return BadRequest("Invalid course ID.");
            }

            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return Unauthorized("User not found or email claim missing.");
            }

            var result = await _courseService.UpdateCourseAsync(editCourseDto, courseIdGuid, user);
            if (!result)
            {
                return BadRequest("Failed to update course.");
            }

            return Ok("Course updated successfully.");
        }

        /// <summary>
        /// Delete an existing course. Only "Instructor" that owns the course can delete it.
        /// </summary>
        /// <param name="courseId">The ID of the course to be deleted.</param>
        /// <returns>OK response for successful deletion.</returns>
        [HttpPost]
        [Authorize(Roles = "Instructor")]
        [Route("delete/{courseId:guid}")]
        public async Task<IActionResult> DeleteCourse([FromRoute] string courseId)
        {
            Guid courseIdGuid = Guid.Empty;
            if (!IsGuidValid(courseId.ToString(), ref courseIdGuid))
            {
                return BadRequest("Invalid course ID.");
            }

            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return Unauthorized("User not found or email claim missing.");
            }

            var result = await _courseService.DeleteCourseAsync(courseIdGuid, user);
            if (!result)
            {
                return BadRequest("Failed to delete course.");
            }

            return Ok("Course deleted successfully.");
        }

        /// <summary>
        /// Get top 3 courses based on the number of enrolled students.
        /// </summary>
        /// <returns>Top 3 courses.</returns>
        [HttpGet]
        [Route("top-courses")]
        public async Task<IActionResult> GetTopCourses()
        {
            var topCourses = await _courseService.GetTopCoursesAsync();
            return Ok(topCourses);
        }
    }
}