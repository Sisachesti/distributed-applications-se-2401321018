using Learn2Gether.Application.DTOs.Requests.Course;
using Learn2Gether.Application.DTOs.Requests.CourseDTO;
using Learn2Gether.Application.DTOs.Responses.Course;
using Learn2Gether.Application.DTOs.Responses.Lecture;
using Learn2Gether.Domain.Entities;

namespace Learn2Gether.Application.Interfaces
{
    public interface ICourseService
    {
        Task<IEnumerable<AllCoursesIndexDTO>> GetAllCoursesAsync(AllCoursesSearchFilterDTO requestDto);
        Task<int> GetTotalCoursesCountAsync(string? searchQuery);
        Task<CourseDetailsDTO?> GetCourseByIdAsync(Guid courseId, User user);
        Task<bool> EnrollToCourseAsync(Guid courseId, User user);
        Task<bool> UnenrollFromCourseAsync(Guid courseId, User user);
        Task<IEnumerable<AllCoursesIndexDTO>> GetUserEnrolledCoursesAsync(User user);
        Task<EnrolledCourseDetailsDTO> GetEnrolledCourseDetailsAsync(Guid courseId, User user);
        Task<WatchLectureDetailsDTO>  GetWatchLectureDetailsAsync(Guid courseGuid, Guid lectureGuid, User user);
        Task<bool> AddNewCourseAsync(AddNewCourseDTO newCourseDto, User instructor);
        Task<bool> UpdateCourseAsync(EditCourseDTO course, Guid courseGuid, User instructor);
        Task<bool> DeleteCourseAsync(Guid courseGuid, User instructor);
        Task<IEnumerable<AllCoursesIndexDTO>> GetTopCoursesAsync();
    }
}
