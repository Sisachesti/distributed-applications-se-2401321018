using Learn2Gether.Application.DTOs.Requests.Note;
using Learn2Gether.Application.Interfaces;
using Learn2Gether.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Learn2Gether.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoteController : BaseController
    {
        private readonly INoteService _noteService;

        public NoteController(UserManager<User> userManager, INoteService noteService)
            : base(userManager)
        {
            _noteService = noteService;
        }

        /// <summary>
        /// Saves a note for the specified Lecture in a Course, enrolled by the current Student user.
        /// </summary>
        /// <param name="lectureId">The Lecture GUID.</param>
        /// <param name="saveNoteDTO">The Note DTO.</param>
        /// <returns>OK repsonse for successful addition.</returns>
        [HttpPost]
        [Authorize(Roles = "Student")]
        [Route("save/{lectureId:guid}")]
        public async Task<IActionResult> SaveNote([FromRoute] string lectureId, [FromBody] SaveNoteDTO saveNoteDTO)
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

            bool isSaved = await _noteService.SaveNoteAsync(lectureIdGuid, saveNoteDTO, user);
            if (!isSaved)
            {
                return BadRequest("An error occurred while saving the note.");
            }

            return Ok("Note saved successfully.");
        }

        /// <summary>
        /// Delete existing Note, owned by the current Student user.
        /// </summary>
        /// <param name="noteId">The Note GUID.</param>
        /// <returns>OK response for successful deletion.</returns>
        [HttpPost]
        [Authorize(Roles = "Student")]
        [Route("delete")]
        public async Task<IActionResult> DeleteNote([FromBody] string noteId)
        {
            Guid noteIdGuid = Guid.Empty;
            if (!IsGuidValid(noteId, ref noteIdGuid))
            {
                return BadRequest("Invalid note ID.");
            }

            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return Unauthorized();
            }

            bool isDeleted = await _noteService.DeleteNoteAsync(noteIdGuid, user);
            if (!isDeleted)
            {
                return BadRequest("An error occurred while deleting the note.");
            }

            return Ok("Note deleted successfully.");
        }

        /// <summary>
        /// Edits an existing Note owned by current Student user.
        /// </summary>
        /// <param name="editNoteDTO">The Note DTO for edit.</param>
        /// <returns>OK response for successful edit.</returns>
        [HttpPost]
        [Authorize(Roles = "Student")]
        [Route("edit")]
        public async Task<IActionResult> EditNote([FromBody] EditNoteDTO editNoteDTO)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Guid noteIdGuid = Guid.Empty;
            if (!IsGuidValid(editNoteDTO.NoteId, ref noteIdGuid))
            {
                return BadRequest("Invalid note ID.");
            }

            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return Unauthorized();
            }

            bool isDeleted = await _noteService.EditNoteAsync(noteIdGuid, editNoteDTO, user);
            if (!isDeleted)
            {
                return BadRequest("An error occurred while editing the note.");
            }
            return Ok("Note edited successfully.");
        }
    }
}
