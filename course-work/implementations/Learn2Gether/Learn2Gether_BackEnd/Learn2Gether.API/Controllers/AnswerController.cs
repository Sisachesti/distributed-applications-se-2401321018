using Learn2Gether.Application.DTOs.Requests.Answer;
using Learn2Gether.Application.Interfaces;
using Learn2Gether.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Learn2Gether.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnswerController : BaseController
    {
        private readonly IAnswerService _answerService;

        public AnswerController(UserManager<User> userManager, IAnswerService answerService) 
            : base(userManager)
        {
            _answerService = answerService;
        }

        /// <summary>
        /// Saves an answer for a specified question submitted by the current user.
        /// </summary>
        /// <param name="questionId">The GUID of the question.</param>
        /// <param name="answer">The answer data to be saved.</param>
        /// <returns>OK response for successful save.</returns>
        [HttpPost]
        [Authorize(Roles = "Student,Instructor")]
        [Route("save/{questionId:guid}")]
        public async Task<IActionResult> SaveAnswer([FromRoute] string questionId, [FromBody] SaveAnswerDTO answer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

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

            bool isSaved = await _answerService.SaveAnswerAsync(questionIdGuid, answer, user);
            if (!isSaved)
            {
                return BadRequest("An error occurred while saving the answer.");
            }

            return Ok("Answer saved successfully.");
        }

        /// <summary>
        /// Edits an existing answer that belongs to the current user.
        /// </summary>
        /// <param name="answer">Answer DTO for editing</param>
        /// <returns>OK response for successful edit.</returns>
        [HttpPost]
        [Authorize(Roles = "Student,Instructor")]
        [Route("edit")]
        public async Task<IActionResult> EditAnswer([FromBody] EditAnswerDTO answer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Guid answerIdGuid = Guid.Empty;
            if (!IsGuidValid(answer.AnswerId, ref answerIdGuid))
            {
                return BadRequest("Invalid answer ID.");
            }

            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return Unauthorized();
            }

            bool isEdited = await _answerService.EditAnswerAsync(answerIdGuid, answer, user);
            if (!isEdited)
            {
                return BadRequest("An error occurred while editing the answer.");
            }

            return Ok("Answer edited successfully.");
        }
        
        /// <summary>
        /// Deletes an existing answer that belongs to the current user.
        /// </summary>
        /// <param name="answerId">The GUID of the answer to be deleted.</param>
        /// <returns>OK response for successful deletion.</returns>
        [HttpPost]
        [Authorize(Roles = "Student,Instructor")]
        [Route("delete")]
        public async Task<IActionResult> DeleteAnswer([FromBody] string answerId)
        {
            Guid answerIdGuid = Guid.Empty;
            if (!IsGuidValid(answerId, ref answerIdGuid))
            {
                return BadRequest("Invalid answer ID.");
            }

            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return Unauthorized();
            }

            bool isDeleted = await _answerService.DeleteAnswerAsync(answerIdGuid, user);
            if (!isDeleted)
            {
                return BadRequest("An error occurred while deleting the answer.");
            }

            return Ok("Answer deleted successfully.");
        }
    }
}
