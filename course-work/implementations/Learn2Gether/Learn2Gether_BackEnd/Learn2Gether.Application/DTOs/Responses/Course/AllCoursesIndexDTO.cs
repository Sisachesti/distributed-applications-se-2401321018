using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learn2Gether.Application.DTOs.Responses.Course
{
    /// <summary>
    /// Represents a data transfer object containing summary information for a course in the course index.
    /// </summary>
    public class AllCoursesIndexDTO
    {
        public string Id { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public string Instructor { get; set; } = null!;
        public int Duration { get; set; }
        public int Students { get; set; }
        public double Rating { get; set; }
    }
}
