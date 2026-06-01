using Learn2Gether.Application.DTOs.Responses.Lecture;

namespace Learn2Gether.Application.DTOs.Responses.Module
{
    /// <summary>
    /// Represents the data of a module that a user is enrolled in its course, including module ID, title, and a collection of lectures within the module.
    /// </summary>
    public class EnrolledCourseModuleDTO
    {
        public string ModuleId { get; set; } = null!;
        public string Title { get; set; } = null!;
        public IEnumerable<EnrolledCourseLectureDTO> Lectures { get; set; }
            = new HashSet<EnrolledCourseLectureDTO>();
    }
}
