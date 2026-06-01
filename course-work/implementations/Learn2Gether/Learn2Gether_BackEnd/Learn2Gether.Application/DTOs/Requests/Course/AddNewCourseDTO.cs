using System.ComponentModel.DataAnnotations;

namespace Learn2Gether.Application.DTOs.Requests.Course
{
    /// <summary>
    /// Represents the data required to add a new course, including title, description, and an optional image URL.
    /// </summary>
    public class AddNewCourseDTO
    {
        [Required]
        [MinLength(5)]
        public string Title { get; set; } = null!;

        [Required]
        [MinLength(10)]
        public string Description { get; set; } = null!;

        public string? ImageUrl { get; set; }
    }
}
