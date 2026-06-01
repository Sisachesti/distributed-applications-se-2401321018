using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learn2Gether.Application.DTOs.Requests.Question
{
    /// <summary>
    /// Represents the data required to save a question, including the title and content of the question. This DTO is used for creating a new question in the application.
    /// </summary>
    public class SaveQuestionDTO
    {
        [Required]
        [MinLength(3)]
        public string Title { get; set; } = null!;

        [Required]
        [MinLength(3)]
        public string Content { get; set; } = null!;
    }
}
