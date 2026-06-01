namespace Learn2Gether.Application.DTOs.Responses.Note
{
    /// <summary>
    /// Represents the data of a lecture note that is sent as a response to the client. This DTO includes the note's unique identifier, content, and the timestamp of when it was created. It is used to provide the necessary information about a lecture note in a structured format for client applications to consume.
    /// </summary>
    public class LectureNoteDTO
    {
        public string Id { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string CreatedAt { get; set; } = null!;
    }
}
