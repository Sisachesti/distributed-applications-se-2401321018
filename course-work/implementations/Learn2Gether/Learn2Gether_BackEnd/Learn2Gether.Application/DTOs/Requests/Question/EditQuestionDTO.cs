using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learn2Gether.Application.DTOs.Requests.Question
{
    /// <summary>
    /// Represents the data transfer object for editing a question, including question ID, title, and content. This DTO is used for updating an existing question in the application.
    /// </summary>
    public class EditQuestionDTO
    {
        [Required]
        public string QuestionId { get; set; } = null!;

        [Required]
        [MinLength(3)]
        public string Title { get; set; } = null!;

        [Required]
        [MinLength(3)]
        public string Content { get; set; } = null!;
    }
}
