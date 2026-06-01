namespace Learn2Gether.Application.DTOs.Responses.Lecture
{
    /// <summary>
    /// Represents the data of a lecture that is part of an enrolled course, including lecture ID, title, and video URL.
    /// </summary>
    public class EnrolledCourseLectureDTO
    {
        public string LectureId { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string VideoUrl { get; set; } = null!;
    }
}
