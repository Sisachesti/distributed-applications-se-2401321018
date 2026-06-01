using Learn2Gether.Application.DTOs.Requests.Wishlist;
using Learn2Gether.Application.Interfaces;
using Learn2Gether.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Learn2Gether.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistController : BaseController
    {
        private readonly IWishlistService _wishlistService;

        public WishlistController(UserManager<User> userManager, IWishlistService wishlistService) 
            : base(userManager)
        {
            _wishlistService = wishlistService;
        }

        /// <summary>
        /// Add a Course to current Student user's Wishlsit.
        /// </summary>
        /// <param name="addToWishlistDTO">DTO of the Course to be validated and added.</param>
        /// <returns>OK repsonse for successfully adding the Course.</returns>
        [HttpPost]
        [Authorize(Roles = "Student")]
        [Route("add")]
        public async Task<IActionResult> AddToWishlist([FromBody] AddToWishlistDTO addToWishlistDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var courseIdGuid = Guid.Empty;
            if(!IsGuidValid(addToWishlistDTO.CourseId, ref courseIdGuid))
            {
                return BadRequest("Invalid CourseId format.");
            }

            var user = await GetCurrentUserAsync();
            if(user == null)
            {
                return Unauthorized();
            }

            var result = await _wishlistService.AddCourseToWishlist(courseIdGuid, user);
            if (!result)
            {
                return BadRequest("Failed to add course to wishlist.");
            }

            return Ok("Course added to wishlist successfully.");
        }

        /// <summary>
        /// Remove existing Course from current Student user's Wishlist
        /// </summary>
        /// <param name="removeFromWishlistDTO">DTO of the Course to be validated and removed.</param>
        /// <returns>OK response for successful removal.</returns>
        [HttpPost]
        [Authorize(Roles = "Student")]
        [Route("remove")]
        public async Task<IActionResult> RemoveFromWishlist([FromBody] RemoveFromWishlistDTO removeFromWishlistDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var courseIdGuid = Guid.Empty;
            if (!IsGuidValid(removeFromWishlistDTO.CourseId, ref courseIdGuid))
            {
                return BadRequest("Invalid CourseId format.");
            }

            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return Unauthorized();
            }

            var result = await _wishlistService.RemoveCourseFromWishlist(courseIdGuid, user);
            if (!result)
            {
                return BadRequest("Failed to remove course from wishlist.");
            }

            return Ok("Course removed from wishlist successfully.");
        }

        /// <summary>
        /// Retrieves the list of Courses in the current Student user's Wishlist.
        /// </summary>
        /// <returns>OK response with a list of all Courses belonging to the Wishlist.</returns>
        [HttpGet]
        [Authorize (Roles = "Student")]
        [Route("courses")]
        public async Task<IActionResult> GetWishlistCourses()
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return Unauthorized();
            }

            var courses = await _wishlistService.GetWishlistCourses(user);
            return Ok(courses);
        }
    }
}
