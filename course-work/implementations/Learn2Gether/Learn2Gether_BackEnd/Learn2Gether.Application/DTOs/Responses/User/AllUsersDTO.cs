using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learn2Gether.Application.DTOs.Responses.User
{
    /// <summary>
    /// Represents a data transfer object for displaying information about all users, including their ID, username, email, and roles. This DTO is used for listing users in the application.
    /// </summary>
    public class AllUsersDTO
    {
        public string UserId { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public IEnumerable<string> Roles { get; set; }
            = new HashSet<string>();
    }
}
