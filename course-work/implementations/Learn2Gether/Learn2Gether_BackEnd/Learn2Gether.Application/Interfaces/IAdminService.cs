using Learn2Gether.Application.DTOs.Responses.User;

namespace Learn2Gether.Application.Interfaces
{
    public interface IAdminService
    {
        Task<IEnumerable<AllUsersDTO>> GetAllUsersAsync();
        Task<bool> AssignUserToRoleAsync(Guid userId, string role);
        Task<bool> RemoveUserFromRoleAsync(Guid userId, string role);
    }
}
