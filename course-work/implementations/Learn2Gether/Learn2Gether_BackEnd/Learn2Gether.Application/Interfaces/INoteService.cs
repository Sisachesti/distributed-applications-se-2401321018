using Learn2Gether.Application.DTOs.Requests.Note;
using Learn2Gether.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learn2Gether.Application.Interfaces
{
    public interface INoteService
    {
        Task<bool> SaveNoteAsync(Guid lectureId, SaveNoteDTO note, User student);
        Task<bool> DeleteNoteAsync(Guid noteId, User student);
        Task<bool> EditNoteAsync(Guid noteId, EditNoteDTO note, User student);
    }
}
