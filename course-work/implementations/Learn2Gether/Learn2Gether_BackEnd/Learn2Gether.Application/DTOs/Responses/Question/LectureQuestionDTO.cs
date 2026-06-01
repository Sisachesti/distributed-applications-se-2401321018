using Learn2Gether.Application.DTOs.Responses.Answer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learn2Gether.Application.DTOs.Responses.Question
{
    /// <summary>
    /// Represents the data transfer object for a lecture question, including its ID, title, content, the date it was asked, the user who asked it, and a collection of answers associated with the question. This DTO is used for displaying detailed information about a specific lecture question in the application.
    /// </summary>
    public class LectureQuestionDTO
    {
        public string Id { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string AskedOn { get; set; } = null!;
        public string AskedBy { get; set; } = null!;
        public string AskedById { get; set; } = null!;

        public IEnumerable<LectureAnswerDTO> Answers { get; set; }
            = new HashSet<LectureAnswerDTO>();
    }
}
