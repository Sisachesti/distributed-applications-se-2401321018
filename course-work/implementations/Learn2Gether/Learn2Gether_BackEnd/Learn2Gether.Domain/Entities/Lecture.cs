using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learn2Gether.Domain.Entities
{
    public class Lecture : BaseEntity
    {
        public string Title { get; set; } = null!;
        public string VideoUrl { get; set; } = null!;
        public bool IsDeleted { get; set; }
        public bool IsCompleted { get; set; }

        // Many-to-One relationship with Module
        public Guid ModuleId { get; set; }
        public Module Module { get; set; } = null!;

        public virtual ICollection<Question> LectureQuestions { get; set; } =
            new HashSet<Question>();
        public virtual ICollection<Answer> LectureAnswers { get; set; } =
            new HashSet<Answer>();
        public virtual ICollection<Note> LectureNotes { get; set; } =
            new HashSet<Note>();
    }
}
