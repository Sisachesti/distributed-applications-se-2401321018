using Learn2Gether.Application.DTOs.Responses.Course;
using Learn2Gether.Application.DTOs.Responses.Instructor;

namespace Learn2Gether.Application.Interfaces
{
    public interface IInstructorService
    {
        Task<IEnumerable<AllCoursesIndexDTO>> GetAllOwnedCoursesAsync(Guid instructorId);
        Task<InstructorCourseDetailsDTO> GetOwnedCourseByIdAsync(Guid courseId, Guid instructorId);
    }
}
