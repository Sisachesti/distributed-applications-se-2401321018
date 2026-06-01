using Learn2Gether.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Learn2Gether.API.Controllers
{
    public class BaseController : Controller
    {
        private readonly UserManager<User> _userManager;

        public BaseController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        /// <summary>
        /// Check if GUID is valid and can be parsed. If valid, the parsed GUID will be returned through the out parameter.
        /// </summary>
        /// <param name="id">The string representation of the GUID to validate.</param>
        /// <param name="parsedGuid">The parsed GUID if the validation is successful.</param>
        /// <returns>True if the GUID is valid; otherwise, false.</returns>
        protected bool IsGuidValid(string? id, ref Guid parsedGuid)
        {
            if (String.IsNullOrWhiteSpace(id))
            {
                return false;
            }

            bool isGuidValid = Guid.TryParse(id, out parsedGuid);
            if (!isGuidValid)
            {
                return false;
            }

            return true;
        }
        
        /// <summary>
        /// Gets the currently authenticated user.
        /// </summary>
        /// <returns>The currently authenticated user, or null if not authenticated.</returns>
        protected async Task<User?> GetCurrentUserAsync()
        {
            if (_userManager == null)
            {
                return null;
            }

            var email = User.FindFirst("sub")?.Value ?? User.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return null;
            }

            return await _userManager.FindByEmailAsync(email);
        }
    }
}
