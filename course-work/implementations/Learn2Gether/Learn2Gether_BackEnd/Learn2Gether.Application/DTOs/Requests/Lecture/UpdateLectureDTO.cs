using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learn2Gether.Application.DTOs.Requests.Lecture
{
    /// <summary>
    /// Represents the data required to update a lecture, including lecture ID, title, and video URL.
    /// </summary>
    public class UpdateLectureDTO
    {
        [Required]
        public string LectureId { get; set; } = null!;
        [Required]
        [MinLength(3)]
        public string Title { get; set; } = null!;
        [Required]
        [MinLength(15)]
        public string VideoUrl { get; set; } = null!;
    }
}
