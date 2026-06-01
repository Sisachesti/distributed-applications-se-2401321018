namespace Learn2Gether.Application.DTOs.Requests.User
{
    /// <summary>
    /// Represents the data transfer object for retrieving a user based on their role, including the user's ID and the role they belong to. This DTO is used for fetching user information based on their assigned role in the application.
    /// </summary>
    public class GetUserForRoleDTO
    {
        public string UserId { get; set; } = null!;
        public string Role { get; set; } = null!;
    }
}
