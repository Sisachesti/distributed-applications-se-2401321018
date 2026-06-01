using Learn2Gether.Application.DTOs.Requests.Question;
using Learn2Gether.Application.Interfaces;
using Learn2Gether.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Learn2Gether.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : BaseController
    {
        private readonly IQuestionService _questionService;

        public QuestionController(UserManager<User> userManager, IQuestionService questionService) : base(userManager)
        {
            _questionService = questionService;
        }

        /// <summary>
        /// Save a Question to a Lecture by current Studnet user.
        /// </summary>
        /// <param name="lectureId">The Lecture GUID.</param>
        /// <param name="question">The Question DTO to be saved.</param>
        /// <returns>OK response for successful addition.</returns>
        [HttpPost]
        [Authorize(Roles = "Student,Instructor")]
        [Route("save/{lectureId:guid}")]
        public async Task<IActionResult> SaveQuestion([FromRoute] string lectureId, [FromBody] SaveQuestionDTO question)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Guid lectureIdGuid = Guid.Empty;
            if (!IsGuidValid(lectureId, ref lectureIdGuid))
            {
                return BadRequest("Invalid lecture ID.");
            }

            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return Unauthorized();
            }

            bool isSaved = await _questionService.SaveQuestionAsync(lectureIdGuid, question, user);
            if (!isSaved)
            {
                return BadRequest("An error occurred while saving the question.");
            }

            return Ok("Question saved successfully.");
        }

        /// <summary>
        /// Edit existing Question owned by current Student user.
        /// </summary>
        /// <param name="question">The Question DTO for edit.</param>
        /// <returns>OK response for successful edit.</returns>
        [HttpPost]
        [Authorize(Roles = "Student,Instructor")]
        [Route("edit")]
        public async Task<IActionResult> EditQuestion([FromBody] EditQuestionDTO question)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Guid questionIdGuid = Guid.Empty;
            if (!IsGuidValid(question.QuestionId, ref questionIdGuid))
            {
                return BadRequest("Invalid question ID.");
            }

            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return Unauthorized();
            }

            bool isEdited = await _questionService.EditQuestionAsync(questionIdGuid, question, user);
            if (!isEdited)
            {
                return BadRequest("An error occurred while editing the question.");
            }

            return Ok("Question edited successfully.");
        }

        /// <summary>
        /// Deletes existing Question with its answers from current Student user.
        /// </summary>
        /// <param name="questionId">The Question GUID.</param>
        /// <returns>OK response for successful deletion.</returns>
        [HttpPost]
        [Authorize(Roles = "Student,Instructor")]
        [Route("delete")]
        public async Task<IActionResult> DeleteQuestion([FromBody] string questionId)
        {
            Guid questionIdGuid = Guid.Empty;
            if (!IsGuidValid(questionId, ref questionIdGuid))
            {
                return BadRequest("Invalid question ID.");
            }

            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return Unauthorized();
            }

            bool isDeleted = await _questionService.DeleteQuestionAsync(questionIdGuid, user);
            if (!isDeleted)
            {
                return BadRequest("An error occurred while deleting the question.");
            }

            return Ok("Question deleted successfully.");
        }
    }
}
