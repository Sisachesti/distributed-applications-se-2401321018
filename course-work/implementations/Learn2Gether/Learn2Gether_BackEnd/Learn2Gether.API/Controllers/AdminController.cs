using Learn2Gether.Application.DTOs.Requests.User;
using Learn2Gether.Application.Interfaces;
using Learn2Gether.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Learn2Gether.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : BaseController
    {
        private readonly IAdminService _userService;

        public AdminController(UserManager<User> userManager, IAdminService adminService) 
            : base(userManager)
        {
            _userService = adminService;
        }

        /// <summary>
        /// Get all existing users in the system. Only accessible by Admin.
        /// </summary>
        /// <returns>A list of all users in the system.</returns>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("index")]
        public async Task<IActionResult> GetAllUsers()
        {
            var allUsers = await _userService
                .GetAllUsersAsync();

            return Ok(allUsers);
        }

        /// <summary>
        /// Assign a role to an user. Only accessible by Admin.
        /// </summary>
        /// <param name="user">User DTO for role asignment.</param>
        /// <returns>OK response for successful role assignment.</returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("assign-role")]
        public async Task<IActionResult> AssignRole([FromBody] GetUserForRoleDTO user)
        {
            Guid userGuid = Guid.Empty;
            if (!this.IsGuidValid(user.UserId, ref userGuid))
            {
                return BadRequest("User ID is not valid.");
            }

            bool assignResult = await _userService
                .AssignUserToRoleAsync(userGuid, user.Role);

            if (!assignResult)
            {
                return BadRequest("Failed to assign role to user.");
            }

            return Ok("Role assigned to user successfully.");
        }

        /// <summary>
        /// Remove a role from an user. Only accessible by Admin.
        /// </summary>
        /// <param name="user">User DTO for role removal.</param>
        /// <returns>OK response for successful role removal.</returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("remove-role")]
        public async Task<IActionResult> RemoveRole([FromBody] GetUserForRoleDTO user)
        {
            Guid userGuid = Guid.Empty;
            if (!this.IsGuidValid(user.UserId, ref userGuid))
            {
                return BadRequest("User ID is not valid.");
            }

            bool assignResult = await _userService
                .RemoveUserFromRoleAsync(userGuid, user.Role);

            if (!assignResult)
            {
                return BadRequest("Failed to remove role from user.");
            }

            return Ok("Role removed from user successfully.");
        }
    }
}
