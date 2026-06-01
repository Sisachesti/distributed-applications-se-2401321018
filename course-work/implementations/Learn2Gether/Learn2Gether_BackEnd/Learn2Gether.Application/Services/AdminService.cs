using Learn2Gether.Application.DTOs.Responses.User;
using Learn2Gether.Application.Interfaces;
using Learn2Gether.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Learn2Gether.Application.Services
{
    public class AdminService : IAdminService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;

        public AdminService(UserManager<User> userManager, RoleManager<IdentityRole<Guid>> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IEnumerable<AllUsersDTO>> GetAllUsersAsync()
        {
            var allUsers = await _userManager.Users.ToArrayAsync();

            var allUsersViewModel = new List<AllUsersDTO>();
            foreach (User user in allUsers)
            {
                IEnumerable<string> roles = await _userManager.GetRolesAsync(user);
                allUsersViewModel.Add(new AllUsersDTO()
                {
                    UserId = user.Id.ToString(),
                    Username = user.UserName!,
                    Email = user.Email!,
                    Roles = roles
                });
            }

            return allUsersViewModel;
        }

        public async Task<bool> AssignUserToRoleAsync(Guid userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return false;
            }

            var roleExists = await _roleManager.RoleExistsAsync(role);
            if (!roleExists)
            {
                return false;
            }

            var result = await _userManager.AddToRoleAsync(user, role);
            if (!result.Succeeded)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> RemoveUserFromRoleAsync(Guid userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return false;
            }

            var roleExists = await _roleManager.RoleExistsAsync(role);
            if (!roleExists)
            {
                return false;
            }

            var result = await _userManager.RemoveFromRoleAsync(user, role);
            if (!result.Succeeded)
            {
                return false;
            }

            return true;
        }
    }
}