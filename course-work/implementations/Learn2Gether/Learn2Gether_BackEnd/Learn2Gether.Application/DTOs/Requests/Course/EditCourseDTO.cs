using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learn2Gether.Application.DTOs.Requests.Course
{
    /// <summary>
    /// Represents the data transfer object for editing a course, including course ID, title, description, and optional
    /// image URL.
    /// </summary>
    public class EditCourseDTO
    {
        [Required]
        public string CourseId { get; set; }

        [Required]
        [MinLength(5)]
        public string Title { get; set; } = null!;

        [Required]
        [MinLength(10)]
        public string Description { get; set; } = null!;

        public string? ImageUrl { get; set; }
    }
}
