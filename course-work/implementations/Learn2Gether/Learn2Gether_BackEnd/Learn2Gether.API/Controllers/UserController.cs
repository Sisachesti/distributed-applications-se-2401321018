using Learn2Gether.Application.DTOs.Requests.UserDTO;
using Learn2Gether.Application.DTOs.Responses.Auth;
using Learn2Gether.Domain.Entities;
using Learn2Gether.Infastructure.Identity.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Learn2Gether.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IJWTService _jwtService;

        public UserController(UserManager<User> userManager, SignInManager<User> signInManager, IJWTService jwtService)
            : base(userManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
        }

        /// <summary>
        /// Register and new User.
        /// </summary>
        /// <param name="registerDto">Register DTO with choice of username, Student/Instructor role.</param>
        /// <returns>OK response with Auth DTO with current New user.</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] SignUpDTO registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(registerDto.Password != registerDto.VerifyPassword)
            {
                return BadRequest(ModelState);
            }

            if (await _userManager.FindByNameAsync(registerDto.Username) != null)
            {
                return BadRequest("User with this username already exists.");
            }

            var user = new User
            {
                UserName = registerDto.Username,
                Email = registerDto.Email,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                EmailConfirmed = false
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                return BadRequest("Could not create user.");
            }

            await _userManager.AddToRoleAsync(user, registerDto.Role);

            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtService.GenerateToken(user.UserName!, user.Email!, roles);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddHours(72)
            };
            Response.Cookies.Append("jwt", token, cookieOptions);

            return Ok(new AuthResponseDTO
            {
                Email = user.Email!,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Roles = roles.ToArray()
            });
        }

        /// <summary>
        /// Login in the system with existing User.
        /// </summary>
        /// <param name="loginDto">User DTO with Email and Password.</param>
        /// <returns>OK response with logget user's Auth DTO</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] SignInDTO loginDto) 
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                return Unauthorized(new { message = "Invalid email." });
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded)
            {
                return Unauthorized(new { message = "Invalid password." });
            }

            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtService.GenerateToken(user.UserName!, user.Email!, roles);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddHours(72)
            };
            Response.Cookies.Append("jwt", token, cookieOptions);

            return Ok(new AuthResponseDTO
            {
                Email = user.Email!,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Roles = roles.ToArray()
            });
        }

        /// <summary>
        /// Logout from the system.
        /// </summary>
        /// <returns>OK response for successful logout.</returns>
        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(-1)
            };
            Response.Cookies.Append("jwt", string.Empty, cookieOptions);

            return Ok("Successful logout.");
        }

        /// <summary>
        /// Retrieves the authenticated user's username, email and roles for authorization for allowed actions in the system.
        /// </summary>
        /// <returns>OK response with new object containing Email, Username and Roles.</returns>
        [Authorize]
        [HttpGet("me")]
        public IActionResult Me()
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var roles = User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToArray();

            return Ok(new { username, email, roles });
        }
    }
}
