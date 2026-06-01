using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learn2Gether.Domain.Entities
{
    public class Module : BaseEntity
    {
        public string Title { get; set; } = null!;
        public bool IsDeleted { get; set; }
        public bool IsCompleted { get; set; }

        // One-to-Many relationship with Course
        public Guid CourseId { get; set; }
        public Course Course { get; set; } = null!;

        // One-to-Many relationship with Lectures
        public virtual ICollection<Lecture> ModuleLectures { get; set; } 
            = new HashSet<Lecture>();
    }
}
