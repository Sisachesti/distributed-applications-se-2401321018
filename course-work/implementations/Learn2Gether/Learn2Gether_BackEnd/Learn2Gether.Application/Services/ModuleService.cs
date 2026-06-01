using Learn2Gether.Application.DTOs.Requests.Module;
using Learn2Gether.Application.Interfaces;
using Learn2Gether.Domain.Entities;
using Learn2Gether.Infastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Learn2Gether.Application.Services
{
    public class ModuleService : IModuleService
    {
        private readonly IBaseRepository<Module, Guid> _moduleRepository;
        private readonly IBaseRepository<Course, Guid> _courseRepository;
        private readonly IBaseRepository<Lecture, Guid> _lectureRepository;

        public ModuleService(IBaseRepository<Module, Guid> moduleRepository, IBaseRepository<Course, Guid> courseRepository, IBaseRepository<Lecture, Guid> lectureRepository)
        {
            _moduleRepository = moduleRepository;
            _courseRepository = courseRepository;
            _lectureRepository = lectureRepository;
        }

        public async Task<bool> AddModuleToCourseAsync(Guid courseId, AddNewModuleDTO newModule, Guid userId)
        {
            var course = await _courseRepository.GetByIdAsync(courseId);
            if (course == null || course.IsDeleted == true || course.InstructorId != userId)
            {
                return false;
            }

            var existingModule = await _moduleRepository
                .FirstOrDefaultAsync(m => m.CourseId == course.Id && m.Title.ToLower() == newModule.Title.ToLower() && m.IsDeleted == false);
            if(existingModule != null)
            {
                return false;
            }

            var module = new Module
            {
                Title = newModule.Title,
                CourseId = course.Id,
                Course = course,
            };

            await _moduleRepository.AddAsync(module);
            return true;
        }

        public async Task<bool> DeleteModuleAsync(Guid moduleId, Guid userId)
        {
            var module = await _moduleRepository.GetByIdAsync(moduleId);
            if (module == null || module.IsDeleted == true)
            {
                return false;
            }

            var lectures = await _lectureRepository.GetAllAttached()
                .Where(l => l.ModuleId == moduleId && l.IsDeleted == false)
                .ToArrayAsync();

            foreach (var lecture in lectures)
            {
                lecture.IsDeleted = true;
                await _lectureRepository.UpdateAsync(lecture);
            }

            module.IsDeleted = true;
            await _moduleRepository.UpdateAsync(module);
            return true;
        }

        public async Task<bool> UpdateModuleAsync(Guid moduleId, EditModuleDTO module, Guid userId)
        {
            var moduleToEdit = await _moduleRepository.GetByIdAsync(moduleId);
            if (moduleToEdit == null || moduleToEdit.IsDeleted == true)
            {
                return false;
            }

            var course = await _courseRepository.GetByIdAsync(moduleToEdit.CourseId);
            if(course.InstructorId != userId)
            {
                return false;
            }

            Module? existingModule = await _moduleRepository.GetAllAttached()
                .Where(m => m.CourseId == moduleToEdit.CourseId && m.Title.ToLower() == module.Title.ToLower() && m.IsDeleted == false)
                .FirstOrDefaultAsync();

            if (existingModule != null && existingModule.Id != moduleId)
            {
                return false;
            }

            moduleToEdit.Title = module.Title;
            await _moduleRepository.UpdateAsync(moduleToEdit);
            return true;
        }
    }
}
