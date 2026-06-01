using Learn2Gether.Application.DTOs.Requests.Lecture;
using Learn2Gether.Application.Interfaces;
using Learn2Gether.Domain.Entities;
using Learn2Gether.Infastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Learn2Gether.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LectureController : BaseController
    {
        private readonly ILectureService _lectureService;

        public LectureController(UserManager<User> userManager, ILectureService lectureService) 
            : base(userManager)
        {
            _lectureService = lectureService;
        }

        /// <summary>
        /// Add a new Lecture to a Module. Accessible only by current Instructor of the course.
        /// </summary>
        /// <param name="newLecture">Lecture DTO to be added.</param>
        /// <returns>OK result for successful addition.</returns>
        [HttpPost]
        [Authorize(Roles = "Instructor")]
        [Route("add")]
        public async Task<IActionResult> AddLectureToModule([FromBody] AddNewLectureDTO newLecture)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await GetCurrentUserAsync();
            if(user == null)
            {
                return Unauthorized("User is not authorized");
            }

            Guid moduleGuid = Guid.Empty;
            if (!Guid.TryParse(newLecture.ModuleId, out moduleGuid))
            {
                return BadRequest("Invalid ModuleId format");
            }

            bool lectureAdded = await _lectureService.AddLectureToModuleAsync(moduleGuid, newLecture, user.Id);
            if (!lectureAdded)
            {
                return BadRequest("Failed to add lecture to module");
            }

            return Ok("Lecture added successfully");
        }

        /// <summary>
        /// Existing Lecture to be updated by currrent Instructor owner.
        /// </summary>
        /// <param name="updateLecture">Lecture DTO to be edited.</param>
        /// <returns>OK response for successful edit.</returns>
        [HttpPost]
        [Authorize(Roles = "Instructor")]
        [Route("update")]
        public async Task<IActionResult> UpdateLecture([FromBody] UpdateLectureDTO updateLecture)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return Unauthorized("User is not authorized");
            }

            Guid lectureGuid = Guid.Empty;
            if (!Guid.TryParse(updateLecture.LectureId, out lectureGuid))
            {
                return BadRequest("Invalid LectureId format");
            }

            bool lectureUpdated = await _lectureService.UpdateLectureAsync(lectureGuid, updateLecture, user.Id);
            if (!lectureUpdated)
            {
                return BadRequest("Failed to update lecture");
            }
            return Ok("Lecture updated successfully");
        }

        /// <summary>
        /// Existing Lecture to be deleted with its Notes, Questions and Answers by current Instructor owner. 
        /// </summary>
        /// <param name="lectureId">The Lecture GUID.</param>
        /// <returns>OK response for successful deletion.</returns>
        [HttpPost]
        [Authorize(Roles = "Instructor")]
        [Route("delete/{lectureId:guid}")]
        public async Task<IActionResult> DeleteLecture([FromRoute] string lectureId)
        {
            Guid lectureGuid = Guid.Empty;
            if (!Guid.TryParse(lectureId, out lectureGuid))
            {
                return BadRequest("Invalid LectureId format");
            }

            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return Unauthorized("User is not authorized");
            }

            bool isDeleted = await _lectureService.DeleteLectureAsync(lectureGuid, user.Id);

            return Ok("Lecture deleted successfully");
        }
    }
}
