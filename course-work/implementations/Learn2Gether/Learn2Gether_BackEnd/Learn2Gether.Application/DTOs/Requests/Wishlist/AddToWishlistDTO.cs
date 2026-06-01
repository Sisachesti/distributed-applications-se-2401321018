using System.ComponentModel.DataAnnotations;

namespace Learn2Gether.Application.DTOs.Requests.Wishlist
{
    /// <summary>
    /// Represents the data required to add a course to a user's wishlist, including the course ID. This DTO is used for adding a course to the wishlist in the application.
    /// </summary>
    public class AddToWishlistDTO
    {
        [Required]
        public string CourseId { get; set; } = null!;
    }
}
