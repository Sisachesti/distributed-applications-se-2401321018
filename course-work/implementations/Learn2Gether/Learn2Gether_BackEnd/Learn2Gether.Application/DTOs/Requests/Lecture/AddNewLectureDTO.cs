using System.ComponentModel.DataAnnotations;

namespace Learn2Gether.Application.DTOs.Requests.Lecture
{
    /// <summary>
    /// Represents the data required to add a new lecture, including module ID, title, and video URL.
    /// </summary>
    public class AddNewLectureDTO
    {
        [Required]
        public string ModuleId { get; set; } = null!;
        [Required]
        [MinLength(3)]
        public string Title { get; set; } = null!;
        [Required]
        [MinLength(15)]
        public string VideoUrl { get; set; } = null!;
    }
}
