using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learn2Gether.Domain.Entities
{
    public class Answer : BaseEntity
    {
        public string? Title { get; set; }
        public string Content { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Soft delete flag
        public bool IsDeleted { get; set; }
        public int LikesCount { get; set; }
        public int DislikesCount { get; set; }
        public bool IsAccepted { get; set; }

        // Many-to-One relationship with Lecture, Question, and User
        public Guid LectureId { get; set; }
        public Lecture Lecture { get; set; } = null!;
        public Guid QuestionId { get; set; }
        public Question Question { get; set; } = null!;
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

    }
}
