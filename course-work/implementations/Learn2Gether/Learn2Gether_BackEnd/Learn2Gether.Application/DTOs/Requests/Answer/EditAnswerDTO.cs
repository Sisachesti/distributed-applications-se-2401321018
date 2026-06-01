using System.ComponentModel.DataAnnotations;

namespace Learn2Gether.Application.DTOs.Requests.Answer
{
    /// <summary>
    /// DTO for editing an existing answer. Contains the answer's ID, the new title and new content.
    /// </summary>
    public class EditAnswerDTO
    {
        [Required]
        public string AnswerId { get; set; } = null!;

        [Required]
        [MinLength(3)]
        public string Title { get; set; } = null!;

        [Required]
        [MinLength(3)]
        public string Content { get; set; } = null!;
    }
}
