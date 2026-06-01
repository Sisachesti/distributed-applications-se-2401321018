using Learn2Gether.Application.DTOs.Requests.Note;
using Learn2Gether.Application.Interfaces;
using Learn2Gether.Domain.Entities;
using Learn2Gether.Infastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learn2Gether.Application.Services
{
    public class NoteService : INoteService
    {
        private readonly IBaseRepository<Note, Guid> _noteRepository;
        private readonly IBaseRepository<Lecture, Guid> _lectureRepository;

        public NoteService(IBaseRepository<Note, Guid> noteRepository, IBaseRepository<Lecture, Guid> lectureRepository)
        {
            _noteRepository = noteRepository;
            _lectureRepository = lectureRepository;
        }

        public async Task<bool> DeleteNoteAsync(Guid noteId, User student)
        {
            var note = await _noteRepository.GetByIdAsync(noteId);
            if (note == null || note.IsDeleted == true || note.StudentId != student.Id)
            {
                return false;
            }

            note.IsDeleted = true;
            await _noteRepository.UpdateAsync(note);
            return true;
        }

        public async Task<bool> EditNoteAsync(Guid noteId, EditNoteDTO note, User student)
        {
            var noteToEdit = await _noteRepository.GetByIdAsync(noteId);
            if(noteToEdit == null || noteToEdit.IsDeleted == true || noteToEdit.StudentId != student.Id)
            {
                return false;
            }

            noteToEdit.Content = note.Content;
            await _noteRepository.UpdateAsync(noteToEdit);
            return true;
        }

        public async Task<bool> SaveNoteAsync(Guid lectureId, SaveNoteDTO note, User student)
        {
            var lecture = await _lectureRepository.GetByIdAsync(lectureId);
            if(lecture == null)
            {
                return false;
            }

            var newNote = new Note
            {
                Content = note.Content,
                LectureId = lectureId,
                StudentId = student.Id,
                CreatedAt = DateTime.UtcNow
            };

            await _noteRepository.AddAsync(newNote);
            return true;
        }
    }
}
