using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learn2Gether.Domain.Entities
{
    // Junction table for Many-to-Many relationship between Students and Courses
    public class StudentCourse
    {
        public Guid StudentId { get; set; }
        public User Student { get; set; } = null!;
        public Guid CourseId { get; set; }
        public Course Course { get; set; } = null!;
    }
}
