using Learn2Gether.Application.DTOs.Responses.Course;
using Learn2Gether.Application.DTOs.Responses.Instructor;
using Learn2Gether.Application.DTOs.Responses.Lecture;
using Learn2Gether.Application.DTOs.Responses.Module;
using Learn2Gether.Application.Interfaces;
using Learn2Gether.Domain.Entities;
using Learn2Gether.Infastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Learn2Gether.Application.Services
{
    public class InstructorService : IInstructorService
    {
        private readonly IBaseRepository<Course, Guid> _courseRepository;

        public InstructorService(IBaseRepository<Course, Guid> courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public async Task<IEnumerable<AllCoursesIndexDTO>> GetAllOwnedCoursesAsync(Guid instructorId)
        {
            var courses = await _courseRepository.GetAllAttached()
                .Where(c => c.InstructorId == instructorId && c.IsDeleted == false)
                .Select(c => new AllCoursesIndexDTO
                {
                    Id = c.Id.ToString(),
                    Title = c.Title,
                    ImageUrl = c.ImageUrl,
                    Instructor = $"{c.Instructor.FirstName} {c.Instructor.LastName}",
                    Duration = c.Duration,
                    Students = c.CourseStudents.Count,
                    Rating = c.Rating
                })
                .ToListAsync();

            return courses;
        }

        public async Task<InstructorCourseDetailsDTO> GetOwnedCourseByIdAsync(Guid courseId, Guid instructorId)
        {
           var courseDetails = await _courseRepository.GetAllAttached()
                .Where(c => c.Id == courseId && c.InstructorId == instructorId && c.IsDeleted == false)
                .Select(c => new InstructorCourseDetailsDTO
                {
                    Modules = c.CourseModules
                    .Where(m => m.IsDeleted == false)
                    .Select(m => new EnrolledCourseModuleDTO
                    {
                        ModuleId = m.Id.ToString(),
                        Title = m.Title,
                        Lectures = m.ModuleLectures
                        .Where(l => l.IsDeleted == false)
                        .Select(l => new EnrolledCourseLectureDTO
                        {
                            LectureId = l.Id.ToString(),
                            Title = l.Title,
                            VideoUrl = l.VideoUrl
                        })
                    })
                })
                .FirstOrDefaultAsync();

            return courseDetails;
        }
    }
}
