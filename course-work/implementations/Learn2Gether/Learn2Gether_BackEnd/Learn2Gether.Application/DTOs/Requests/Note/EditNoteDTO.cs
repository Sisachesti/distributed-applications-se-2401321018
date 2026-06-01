using System.ComponentModel.DataAnnotations;

namespace Learn2Gether.Application.DTOs.Requests.Note
{
    /// <summary>
    /// Represents the data required to edit an existing note, including note ID and content.
    /// </summary>
    public class EditNoteDTO
    {
        [Required]
        public string NoteId { get; set; } = null!;

        [Required]
        [MinLength(3)]
        public string Content { get; set; } = null!;
    }
}
