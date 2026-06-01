namespace Learn2Gether.Application.DTOs.Responses.Answer
{
    /// <summary>
    /// Represent the data of an answer for visualisation purposes, including the answer's ID, title, content, the date it was answered, the name and ID of the user who answered, the number of likes and dislikes, and whether the answer is accepted. This DTO is used for displaying an answer in the application.
    /// </summary>
    public class LectureAnswerDTO
    {
        public string Id { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string AnsweredOn { get; set; } = null!;
        public string AnsweredBy { get; set; } = null!;
        public string AnsweredById { get; set; } = null!;
        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public bool IsAccepted { get; set; }
    }
}
