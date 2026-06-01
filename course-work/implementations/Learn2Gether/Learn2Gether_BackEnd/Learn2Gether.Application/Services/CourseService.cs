using Learn2Gether.Application.DTOs.Requests.Course;
using Learn2Gether.Application.DTOs.Requests.CourseDTO;
using Learn2Gether.Application.DTOs.Responses.Answer;
using Learn2Gether.Application.DTOs.Responses.Course;
using Learn2Gether.Application.DTOs.Responses.Lecture;
using Learn2Gether.Application.DTOs.Responses.Note;
using Learn2Gether.Application.DTOs.Responses.Question;
using Learn2Gether.Application.Interfaces;
using Learn2Gether.Domain.Entities;
using Learn2Gether.Infastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Learn2Gether.Application.Services
{
    public class CourseService : ICourseService
    {
        private readonly IBaseRepository<Course, Guid> _courseRepository;
        private readonly IBaseRepository<User, Guid> _userRepository;
        private readonly IBaseRepository<StudentCourse, object> _studentCourseRepository;
        private readonly IBaseRepository<Lecture, Guid> _lectureRepository;
        private readonly IBaseRepository<Module, Guid> _moduleRepository;

        public CourseService(IBaseRepository<Course, Guid> courseRepository, IBaseRepository<User, Guid> userRepository,
            IBaseRepository<StudentCourse, object> studentCourseRepository, IBaseRepository<Lecture, Guid> lectureRepository, IBaseRepository<Module, Guid> moduleRepository)
        {
            _courseRepository = courseRepository;
            _userRepository = userRepository;
            _studentCourseRepository = studentCourseRepository;
            _lectureRepository = lectureRepository;
            _moduleRepository = moduleRepository;
        }

        public async Task<IEnumerable<AllCoursesIndexDTO>> GetAllCoursesAsync(AllCoursesSearchFilterDTO requestDto)
        {
            IQueryable<Course> allCourses = _courseRepository.GetAllAttached();

            if (!String.IsNullOrWhiteSpace(requestDto.SearchQuery))
            {
                allCourses = allCourses.Where(course => course.Title.ToLower().StartsWith(requestDto.SearchQuery.ToLower()));
            }

            if (requestDto.CurrentPage.HasValue &&
                requestDto.EntitiesPerPage.HasValue)
            {
                allCourses = allCourses
                    .Skip((requestDto.CurrentPage.Value - 1) * requestDto.EntitiesPerPage.Value)
                    .Take(requestDto.EntitiesPerPage.Value);
            }

            var result = allCourses
                .Where(c => c.IsDeleted == false)
                .Select(course => new AllCoursesIndexDTO
            {
                Id = course.Id.ToString(),
                Title = course.Title,
                ImageUrl = course.ImageUrl,
                Instructor = $"{course.Instructor.FirstName} {course.Instructor.LastName}",
                Duration = course.Duration,
                Students = course.CourseStudents.Count,
                Rating = course.Rating
            });

            return await result.ToArrayAsync();
        }

        public async Task<int> GetTotalCoursesCountAsync(string? searchQuery)
        {
            AllCoursesSearchFilterDTO requestDto = new AllCoursesSearchFilterDTO
            {
                CurrentPage = null,
                EntitiesPerPage = null,
                SearchQuery = searchQuery
            };

            int classesCount = (await GetAllCoursesAsync(requestDto)).Count();
            return classesCount;
        }

        public async Task<CourseDetailsDTO?> GetCourseByIdAsync(Guid courseId, User user)
        {
            bool isEnrolled = false;

            if (user != null)
            {
                isEnrolled = _studentCourseRepository.GetAllAttached()
                        .Any(sc => sc.CourseId == courseId && sc.StudentId == user.Id);
            }

            var course = await _courseRepository.GetAllAttached()
                .Where(c => c.Id == courseId && c.IsDeleted == false)
                .Select(c => new CourseDetailsDTO
                {
                    Title = c.Title,
                    Description = c.Description,
                    ImageUrl = c.ImageUrl,
                    Instructor = $"{c.Instructor.FirstName} {c.Instructor.LastName}",
                    DurationInMinutes = c.Duration,
                    StudentsEnrolled = c.CourseStudents.Count,
                    Rating = c.Rating,
                    Enrolled = isEnrolled
                })
                .FirstOrDefaultAsync();

            return course;
        }

        public async Task<bool> EnrollToCourseAsync(Guid courseId, User user)
        {
            var course = await _courseRepository.GetByIdAsync(courseId);
            if (course == null || course.IsDeleted)
            {
                return false;
            }

            if (user == null)
            {
                return false;
            }

            StudentCourse? existingEnrollment = await _studentCourseRepository
                .FirstOrDefaultAsync(sc => sc.CourseId == courseId && sc.StudentId == user.Id);

            if (existingEnrollment == null)
            {
                var studentCourse = new StudentCourse
                {
                    CourseId = courseId,
                    StudentId = user.Id
                };

                await _studentCourseRepository.AddAsync(studentCourse);
            }

            return true;
        }

        public async Task<bool> UnenrollFromCourseAsync(Guid courseId, User user)
        {
            var course = await _courseRepository.GetByIdAsync(courseId);
            if (course == null || course.IsDeleted)
            {
                return false;
            }

            if (user == null)
            {
                return false;
            }

            StudentCourse? existingEnrollment = await _studentCourseRepository
                .FirstOrDefaultAsync(sc => sc.CourseId == courseId && sc.StudentId == user.Id);
            if (existingEnrollment != null)
            {
                await _studentCourseRepository.DeleteAsync(existingEnrollment);
            }
            return true;
        }

        public async Task<IEnumerable<AllCoursesIndexDTO>> GetUserEnrolledCoursesAsync(User user)
        {
            var enrolledCourseIds = _studentCourseRepository.GetAllAttached()
                .Where(sc => sc.StudentId == user.Id)
                .Select(sc => sc.CourseId);

            var enrolledCourses = _courseRepository.GetAllAttached()
                .Where(c => enrolledCourseIds.Contains(c.Id) && c.IsDeleted == false)
                .Select(course => new AllCoursesIndexDTO
                {
                    Id = course.Id.ToString(),
                    Title = course.Title,
                    ImageUrl = course.ImageUrl,
                    Instructor = $"{course.Instructor.FirstName} {course.Instructor.LastName}",
                    Duration = course.Duration,
                    Students = course.CourseStudents.Count,
                    Rating = course.Rating
                });

            return await enrolledCourses.ToArrayAsync();
        }

        public async Task<EnrolledCourseDetailsDTO> GetEnrolledCourseDetailsAsync(Guid courseId, User user)
        {
            var course = await _courseRepository.GetAllAttached()
                .Where(c => c.Id == courseId && c.IsDeleted == false)
                .Select(c => new EnrolledCourseDetailsDTO
                {
                    CourseId = c.Id.ToString(),
                    Title = c.Title,
                    Instructor = $"{c.Instructor.FirstName} {c.Instructor.LastName}",
                    DurationInMinutes = c.Duration,
                    Description = c.Description,
                    Modules = c.CourseModules
                    .Where(m => m.IsDeleted == false)
                    .Select(m => new DTOs.Responses.Module.EnrolledCourseModuleDTO
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
            return course!;
        }

        public async Task<WatchLectureDetailsDTO> GetWatchLectureDetailsAsync(Guid courseGuid, Guid lectureGuid, User user)
        {
            var course = await this.GetEnrolledCourseDetailsAsync(courseGuid, user);

            if(course != null)
            { 
                var lectureQuestions = await _lectureRepository.GetAllAttached()
                    .Where(l => l.Id == lectureGuid && l.IsDeleted == false)
                    .SelectMany(l => l.LectureQuestions)
                    .Where(q => q.IsDeleted == false)
                    .Select(q => new LectureQuestionDTO
                    {
                        Id = q.Id.ToString(),
                        Title = q.Title,
                        Content = q.Content,
                        AskedBy = q.User.UserName!,
                        AskedOn = q.AskedOn.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                        Answers = q.QuestionAnswers
                            .Where(a => a.IsDeleted == false)
                            .Select(a => new LectureAnswerDTO
                            {
                                Id = a.Id.ToString(),
                                Title = a.Title!,
                                Content = a.Content,
                                AnsweredBy = a.User.UserName!,
                                AnsweredOn = a.CreatedAt.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                                Likes = a.LikesCount,
                                Dislikes = a.DislikesCount,
                                IsAccepted = a.IsAccepted
                            })
                    })
                    .ToArrayAsync();

                var lectureNotes = await _lectureRepository.GetAllAttached()
                    .Where(l => l.Id == lectureGuid && l.IsDeleted == false)
                    .SelectMany(l => l.LectureNotes)
                    .Where(n => n.StudentId == user.Id && n.IsDeleted == false)
                    .OrderBy(n => n.CreatedAt)
                    .Select(n => new LectureNoteDTO
                    {
                        Id = n.Id.ToString(),
                        Content = n.Content,
                        CreatedAt = n.CreatedAt.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)
                    })
                    .ToArrayAsync();

                var watchLectureDetails = new WatchLectureDetailsDTO
                {
                    CourseDetails = course,
                    QuestionsAnswers = lectureQuestions,
                    Notes = lectureNotes
                };

                return watchLectureDetails;
            }

            return null;
        }

        public async Task<bool> AddNewCourseAsync(AddNewCourseDTO newCourseDto, User instructor)
        {
            var existingCourse = await _courseRepository
                .FirstOrDefaultAsync(c => c.Title.ToLower() == newCourseDto.Title.ToLower());
            if (existingCourse != null && existingCourse.IsDeleted == false)
            {
                return false;
            }

            var newCourse = new Course
            {
                Title = newCourseDto.Title,
                Description = newCourseDto.Description,
                ImageUrl = newCourseDto.ImageUrl,
                InstructorId = instructor.Id,
                Duration = 0,
                Rating = 0.0,
                IsDeleted = false,
                IsCompleted = false
            };

            await _courseRepository.AddAsync(newCourse);
            return true;
        }

        public async Task<bool> UpdateCourseAsync(EditCourseDTO course, Guid courseGuid, User instructor)
        {
            var courseToEdit = await _courseRepository.GetByIdAsync(courseGuid);
            if (courseToEdit == null || courseToEdit.InstructorId != instructor.Id || courseToEdit.IsDeleted == true)
            {
                return false;
            }

            courseToEdit.Title = course.Title;
            courseToEdit.Description = course.Description;
            courseToEdit.ImageUrl = course.ImageUrl;
            await _courseRepository.UpdateAsync(courseToEdit);
            return true;
        }

        public async Task<bool> DeleteCourseAsync(Guid courseGuid, User instructor)
        {
            var courseToDelete = await _courseRepository.GetByIdAsync(courseGuid);
            if (courseToDelete == null || courseToDelete.InstructorId != instructor.Id || courseToDelete.IsDeleted == true)
            {
                return false;
            }

            var modules = await _moduleRepository.GetAllAttached()
                .Where(m => m.CourseId == courseGuid && m.IsDeleted == false)
                .ToListAsync();

            foreach (var module in modules)
            {
                module.IsDeleted = true;
                await this._moduleRepository.UpdateAsync(module);

                if(module.ModuleLectures.Count() > 0)
                {
                    var lectures = await _lectureRepository.GetAllAttached()
                    .Where(l => l.ModuleId == module.Id && l.IsDeleted == false)
                    .ToListAsync();

                    foreach (var lecture in lectures)
                    {
                        lecture.IsDeleted = true;
                        await this._lectureRepository.UpdateAsync(lecture);
                    }
                }
            }

            courseToDelete.IsDeleted = true;
            await _courseRepository.UpdateAsync(courseToDelete);
            return true;
        }

        public async Task<IEnumerable<AllCoursesIndexDTO>> GetTopCoursesAsync()
        {
            var topCourses = await _courseRepository.GetAllAttached()
                .Where(c => c.IsDeleted == false)
                .OrderByDescending(c => c.CourseStudents.Count)
                .Take(3)
                .Select(course => new AllCoursesIndexDTO
                {
                    Id = course.Id.ToString(),
                    Title = course.Title,
                    ImageUrl = course.ImageUrl,
                    Instructor = $"{course.Instructor.FirstName} {course.Instructor.LastName}",
                    Students = course.CourseStudents.Count,
                })
                .ToListAsync();

            return topCourses;
        }
    }
}
