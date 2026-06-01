using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learn2Gether.Application.DTOs.Responses.Course
{
    /// <summary>
    /// Represents the data transfer object for course details, including image URL, title, instructor, rating, description, number of students enrolled, duration in minutes, and enrollment status. This DTO is used for displaying detailed information about a specific course in the application.
    /// </summary>
    public class CourseDetailsDTO
    {
        public string ImageUrl { get; set; }
        public string Title { get; set; }
        public string Instructor { get; set; }
        public double Rating { get; set; }
        public string Description { get; set; }
        public int StudentsEnrolled { get; set; }   
        public int DurationInMinutes { get; set; }
        public bool Enrolled { get; set; }
    }
}
