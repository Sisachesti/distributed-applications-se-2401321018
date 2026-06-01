using System.ComponentModel.DataAnnotations;

namespace Learn2Gether.Application.DTOs.Requests.Module
{
    /// <summary>
    /// Represents the data required to add a new module, including course ID and title.
    /// </summary>
    public class AddNewModuleDTO
    {
        [Required]
        public string CourseId { get; set; } = null!;

        [Required]
        [MinLength(3)]
        public string Title { get; set; } = null!;
    }
}
