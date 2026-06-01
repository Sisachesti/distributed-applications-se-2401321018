using System.ComponentModel.DataAnnotations;

namespace Learn2Gether.Application.DTOs.Requests.Wishlist
{
    /// <summary>
    /// Represents the data required to remove a course from a user's wishlist, including the course ID. This DTO is used for removing a course from the wishlist in the application.
    /// </summary>
    public class RemoveFromWishlistDTO
    {
        [Required]
        public string CourseId { get; set; } = null!;
    }
}
