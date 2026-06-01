using Learn2Gether.Application.DTOs.Responses.Course;
using Learn2Gether.Application.DTOs.Responses.Note;
using Learn2Gether.Application.DTOs.Responses.Question;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learn2Gether.Application.DTOs.Responses.Lecture
{
    /// <summary>
    /// Represents the details of a lecture for a student, including course details, questions and answers related to the lecture, and notes taken by the student. This DTO is used to provide comprehensive information about a lecture when a student watches it.
    /// </summary>
    public class WatchLectureDetailsDTO
    {
        public EnrolledCourseDetailsDTO CourseDetails { get; set; } = null!;
        public IEnumerable<LectureQuestionDTO> QuestionsAnswers { get; set; }
            = new HashSet<LectureQuestionDTO>();

        public IEnumerable<LectureNoteDTO> Notes { get; set; } =
            new HashSet<LectureNoteDTO>();
    }
}
