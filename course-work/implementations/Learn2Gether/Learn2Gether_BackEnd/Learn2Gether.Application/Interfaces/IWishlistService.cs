using Learn2Gether.Application.DTOs.Responses.Course;
using Learn2Gether.Domain.Entities;

namespace Learn2Gether.Application.Interfaces
{
    public interface IWishlistService
    {
        Task<bool> AddCourseToWishlist(Guid courseId, User student);
        Task<bool> RemoveCourseFromWishlist(Guid courseId, User student);
        Task<IEnumerable<AllCoursesIndexDTO>> GetWishlistCourses(User student);
    }
}
