using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learn2Gether.Domain.Entities
{
    public class Question : BaseEntity
    {
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public DateTime AskedOn { get; set; }

        //Soft delete flag
        public bool IsDeleted { get; set; }

        // Many-to-One relationship with Lecture and User
        public Guid LectureId { get; set; }
        public Lecture Lecture { get; set; } = null!;
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        // One-to-Many relationship with Answers
        public virtual ICollection<Answer> QuestionAnswers { get; set; } 
            = new HashSet<Answer>();
    }
}
