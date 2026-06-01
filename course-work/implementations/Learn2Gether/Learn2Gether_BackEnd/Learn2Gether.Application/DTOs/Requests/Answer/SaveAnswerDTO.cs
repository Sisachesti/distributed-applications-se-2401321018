using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learn2Gether.Application.DTOs.Requests.Answer
{
    /// <summary>
    /// Represents the data required to save an answer, including title and content.
    /// </summary>
    public class SaveAnswerDTO
    {
        [Required]
        [MinLength(3)]
        public string Title { get; set; } = null!;

        [Required]
        [MinLength(3)]
        public string Content { get; set; } = null!;
    }
}
