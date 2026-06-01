using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learn2Gether.Domain.Entities
{
    public class Note : BaseEntity
    {
        public string Content { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        // Soft delete flag
        public bool IsDeleted { get; set; }

        // Many-to-One relationship with Lecture, Question, and User
        public Guid LectureId { get; set; }
        public Lecture Lecture { get; set; } = null!;
        public Guid StudentId { get; set; }
        public User Student { get; set; } = null!;
    }
}
