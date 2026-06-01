using Learn2Gether.Application.DTOs.Responses.Module;

namespace Learn2Gether.Application.DTOs.Responses.Instructor
{
    /// <summary>
    /// Represents the details of a course for an instructor, including a collection of modules and their associated lectures.
    /// </summary>
    public class InstructorCourseDetailsDTO
    {
        public IEnumerable<EnrolledCourseModuleDTO> Modules { get; set; } // This DTO contains HashSets of lectures that belong to each module
            = new HashSet<EnrolledCourseModuleDTO>();
    }
}
