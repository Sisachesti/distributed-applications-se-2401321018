using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learn2Gether.Domain.Entities
{
    public class User : IdentityUser<Guid>
    {
       public User()
       {
            Id = Guid.NewGuid();
       }

        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? ProfilePictureUrl { get; set; }
        public string? Bio { get; set; }

        // One-to-Many relationship: A user can instruct multiple courses
        public virtual ICollection<Course> InstructorCourses { get; set; } 
            = new HashSet<Course>();

        // Many-to-Many relationship: A user can be enrolled in multiple courses and wish for many non-enrolled courses
        public virtual ICollection<StudentCourse> EnrolledCourses { get; set; }
            = new HashSet<StudentCourse>();

        public virtual ICollection<Wishlist> Wishlist { get; set; }
            = new HashSet<Wishlist>();

        // One-to-Many relationship: A user can ask multiple questions, many can give multiple answers and have multiple notes
        public virtual ICollection<Question> UserQuestions { get; set; }
            = new HashSet<Question>();

        public virtual ICollection<Answer> UserAnswers { get; set; }
            = new HashSet<Answer>();

        public virtual ICollection<Note> StudentNotes { get; set; }
            = new HashSet<Note>();
    }
}
