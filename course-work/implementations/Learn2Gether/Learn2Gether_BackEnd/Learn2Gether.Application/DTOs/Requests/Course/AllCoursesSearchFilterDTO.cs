using Learn2Gether.Application.DTOs.Responses.Course;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learn2Gether.Application.DTOs.Requests.CourseDTO
{
    /// <summary>
    /// Represents a filter and pagination model for searching and displaying a list of courses.
    /// </summary>
    public class AllCoursesSearchFilterDTO
    {
        public IEnumerable<AllCoursesIndexDTO>? Courses { get; set; }
        public string? SearchQuery { get; set; }
        public int? CurrentPage { get; set; } = 1;
        public int? EntitiesPerPage { get; set; } = 15;
        public int? TotalPages { get; set; }
    }
}
