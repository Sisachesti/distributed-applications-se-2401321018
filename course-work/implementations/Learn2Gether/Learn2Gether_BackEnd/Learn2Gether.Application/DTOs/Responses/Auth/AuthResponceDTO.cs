using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learn2Gether.Application.DTOs.Responses.Auth
{
    /// <summary>
    /// Represents the data transfer object for authentication response, including user email, first name, last name, and roles. This DTO is used to return user information after successful authentication in the application.
    /// </summary>
    public class AuthResponseDTO
    {
        public string Email { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string[] Roles { get; set; } = null!;
    }
}
