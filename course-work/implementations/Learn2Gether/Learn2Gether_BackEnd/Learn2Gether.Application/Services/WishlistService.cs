using Learn2Gether.Application.DTOs.Responses.Course;
using Learn2Gether.Application.Interfaces;
using Learn2Gether.Domain.Entities;
using Learn2Gether.Infastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learn2Gether.Application.Services
{
    public class WishlistService : IWishlistService
    {
        private readonly IBaseRepository<Course, Guid> _courseRepository;
        private readonly IBaseRepository<User, Guid> _userRepository;
        private readonly IBaseRepository<Wishlist, Guid> _wishlistRepository;

        public WishlistService(IBaseRepository<Course, Guid> courseRepository, IBaseRepository<User, Guid> userRepository, IBaseRepository<Wishlist, Guid> wishlistRepository)
        {
            _courseRepository = courseRepository;
            _userRepository = userRepository;
            _wishlistRepository = wishlistRepository;
        }

        public async Task<bool> AddCourseToWishlist(Guid courseId, User student)
        {
            var course = await _courseRepository.GetByIdAsync(courseId);
            if(course == null)
            {
                return false;
            }

            Wishlist? existingWishlist = await _wishlistRepository
                .FirstOrDefaultAsync(sc => sc.CourseId == courseId && sc.StudentId == student.Id);

            if (existingWishlist == null)
            {
                var wishlistToAdd = new Wishlist
                {
                    CourseId = courseId,
                    StudentId = student.Id
                };
                await _wishlistRepository.AddAsync(wishlistToAdd);
            }

            return true;
        }

        public async Task<IEnumerable<AllCoursesIndexDTO>> GetWishlistCourses(User student)
        {
            var wishlistCourses = await _wishlistRepository.GetAllAttached()
                .Where(w => w.StudentId == student.Id)
                .Select(w => new AllCoursesIndexDTO
                {
                    Id = w.Course.Id.ToString(),
                    Title = w.Course.Title,
                    ImageUrl = w.Course.ImageUrl,
                    Instructor = $"{w.Course.Instructor.FirstName} {w.Course.Instructor.LastName}",
                    Students = w.Course.CourseStudents.Count,
                })
                .ToArrayAsync();

            return wishlistCourses;
        }

        public async Task<bool> RemoveCourseFromWishlist(Guid courseId, User student)
        {
            var course = await _courseRepository.GetByIdAsync(courseId);
            if (course == null)
            {
                return false;
            }

            Wishlist existingWishlist = await this._wishlistRepository
                .FirstOrDefaultAsync(w => w.CourseId == courseId && w.StudentId == student.Id);
            if (existingWishlist == null)
            {
                return false;
            }

            await _wishlistRepository.DeleteAsync(existingWishlist);
            return true;
        }

    }
}
