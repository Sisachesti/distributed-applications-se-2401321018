using Learn2Gether.Application.DTOs.Requests.Module;
using Learn2Gether.Application.Interfaces;
using Learn2Gether.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Learn2Gether.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModuleController : BaseController
    {
        private readonly IModuleService _moduleService;

        public ModuleController(UserManager<User> userManager, IModuleService moduleService)
            : base(userManager)
        {
            _moduleService = moduleService;
        }

        /// <summary>
        /// Module to be added to existing course, owned by current Instructor.
        /// </summary>
        /// <param name="newModule">The Module DTO.</param>
        /// <returns>OK repsonse for successful addition.</returns>
        [HttpPost]
        [Authorize(Roles = "Instructor")]
        [Route("add")]
        public async Task<IActionResult> AddModule([FromBody] AddNewModuleDTO newModule)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Guid courseGuid = Guid.Empty;
            if (!IsGuidValid(newModule.CourseId, ref courseGuid))
            {
                return BadRequest(ModelState);
            }

            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return Unauthorized("User not authorized");
            }

            var result = await _moduleService.AddModuleToCourseAsync(courseGuid, newModule, user.Id);
            if (!result)
            {
                return BadRequest("Adding new module failed");
            }

            return Ok("Module added successfully.");
        }

        /// <summary>
        /// Existing Module to be deleted with its Lectures by current Instructor owner.
        /// </summary>
        /// <param name="moduleId">The Module GUID.</param>
        /// <returns>OK repsonse for successful deletion.</returns>
        [HttpPost]
        [Authorize(Roles = "Instructor")]
        [Route("delete/{moduleId:guid}")]
        public async Task<IActionResult> DeleteModule([FromRoute] string moduleId)
        {
            Guid moduleGuid = Guid.Empty;
            if (!IsGuidValid(moduleId, ref moduleGuid))
            {
                return BadRequest(ModelState);
            }

            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return Unauthorized("User not authorized");
            }

            var result = await _moduleService.DeleteModuleAsync(moduleGuid, user.Id);
            if (!result)
            {
                return BadRequest(result);
            }

            return Ok("Model deleted successfully");
        }

        /// <summary>
        /// Existing Module to be edited by current Instructor owner.
        /// </summary>
        /// <param name="editModule">The Module DTO.</param>
        /// <returns>OK repsonse for successful edit</returns>
        [HttpPost]
        [Authorize(Roles = "Instructor")]
        [Route("edit")]
        public async Task<IActionResult> EditModule([FromBody] EditModuleDTO editModule)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Guid moduleGuid = Guid.Empty;
            if (!IsGuidValid(editModule.ModuleId, ref moduleGuid))
            {
                return BadRequest(ModelState);
            }

            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return Unauthorized("User not authorized");
            }

            var result = await _moduleService.UpdateModuleAsync(moduleGuid, editModule, user.Id);
            if (!result)
            {
                return BadRequest(result);
            }

            return Ok("Module updated successfully.");
        }
    }
}
