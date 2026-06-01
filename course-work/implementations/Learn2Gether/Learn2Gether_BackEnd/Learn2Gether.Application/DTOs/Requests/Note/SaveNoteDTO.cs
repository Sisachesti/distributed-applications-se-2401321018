using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learn2Gether.Application.DTOs.Requests.Note
{
    /// <summary>
    /// Represents the data required to save a note, including the content of the note. This DTO is used for creating a new note in the application.
    /// </summary>
    public class SaveNoteDTO
    {
        [Required]
        [MinLength(3)]
        public string Content { get; set; }
    }
}
