using System.ComponentModel.DataAnnotations;

namespace Learn2Gether.Application.DTOs.Requests.UserDTO
{
    /// <summary>
    /// Represents the data required for a user to sign up, including first name, last name, username, role, email, password, and password verification. This DTO is used for registering a new user in the application.
    /// </summary>
    public class SignUpDTO
    {
        [Required]
        [MinLength(3)]
        public string FirstName { get; set; } = null!;

        [Required]
        [MinLength(3)]
        public string LastName { get; set; } = null!;

        [Required]
        [MinLength(6)]
        public string Username { get; set; } = null!;

        [Required]
        public string Role { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = null!;

        [Required]
        [MinLength(6)]
        public string VerifyPassword { get; set; } = null!;
    }
}
