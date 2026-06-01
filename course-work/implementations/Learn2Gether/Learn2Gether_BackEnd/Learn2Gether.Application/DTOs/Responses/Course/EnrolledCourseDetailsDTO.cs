using Learn2Gether.Application.DTOs.Responses.Module;

namespace Learn2Gether.Application.DTOs.Responses.Course
{
    /// <summary>
    /// Represents the data transfer object for detailed information about an enrolled course, including course ID, title, instructor, duration, description, and modules. This DTO is used for displaying detailed information about an enrolled course in the application.
    /// </summary>
    public class EnrolledCourseDetailsDTO
    {
        public string CourseId { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Instructor { get; set; } = null!;
        public int DurationInMinutes { get; set; }
        public string Description { get; set; } = null!;

        public IEnumerable<EnrolledCourseModuleDTO> Modules { get; set; }
            = new HashSet<EnrolledCourseModuleDTO>();
    }
}
