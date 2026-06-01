using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learn2Gether.Domain.Entities
{
    public class Course : BaseEntity
    {
        public string Title { get; set; } = null!;
        public double Rating { get; set; }
        public int Duration { get; set; }
        public string? ImageUrl { get; set; }
        public string Description { get; set; } = null!;
        public bool IsDeleted { get; set; }
        public bool IsCompleted { get; set; }

        // Many-to-One relationship with User (Instructor)
        public Guid InstructorId { get; set; }
        public User Instructor { get; set; } = null!;

        // One-to-Many relationship with Module
        public virtual ICollection<Module> CourseModules { get; set; }
            = new HashSet<Module>();

        // Many-to-Many relationship with Students(Users)
        public virtual ICollection<StudentCourse> CourseStudents { get; set; }
            = new HashSet<StudentCourse>();

        public virtual ICollection<Wishlist> Wishlist { get; set; }
            = new HashSet<Wishlist>();
    }
}
