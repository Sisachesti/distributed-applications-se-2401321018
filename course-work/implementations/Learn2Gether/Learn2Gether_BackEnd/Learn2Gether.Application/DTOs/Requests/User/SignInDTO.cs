using System.ComponentModel.DataAnnotations;

namespace Learn2Gether.Application.DTOs.Requests.UserDTO
{
    /// <summary>
    /// Represents the data required for a user to sign in, including email and password. This DTO is used for authenticating a user in the application.
    /// </summary>
    public class SignInDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        [MinLength(6)]
        public string Password { get; set; } = null!;
    }
}
