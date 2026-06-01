using Learn2Gether.Application.DTOs.Requests.Lecture;
using Learn2Gether.Application.Interfaces;
using Learn2Gether.Domain.Entities;
using Learn2Gether.Infastructure.Repositories;

namespace Learn2Gether.Application.Services
{
    public class LectureService : ILectureService
    {
        private readonly IBaseRepository<Lecture, Guid> _lectureRepository;
        private readonly IBaseRepository<Module, Guid> _moduleRepository;

        public LectureService(IBaseRepository<Lecture, Guid> lectureRepository, IBaseRepository<Module, Guid> moduleRepository)
        {
            _lectureRepository = lectureRepository;
            _moduleRepository = moduleRepository;
        }

        public async Task<bool> AddLectureToModuleAsync(Guid moduleId, AddNewLectureDTO newLecture, Guid userId)
        {
            var module = await _moduleRepository.GetByIdAsync(moduleId);
            if (module == null || module.IsDeleted == true)
            {
                return false;
            }

            var existingLecture = await _lectureRepository
                .FirstOrDefaultAsync(l => l.ModuleId == moduleId && l.Title.ToLower() == newLecture.Title.ToLower() && l.IsDeleted == false);
            if (existingLecture != null && existingLecture.IsDeleted == false)
            {
                return false;
            }

            var lecture = new Lecture
            {
                Title = newLecture.Title,
                ModuleId = moduleId,
                VideoUrl = newLecture.VideoUrl
            };

            await _lectureRepository.AddAsync(lecture);
            return true;
        }

        public async Task<bool> DeleteLectureAsync(Guid lectureId, Guid userId)
        {
            var lecture = await _lectureRepository.GetByIdAsync(lectureId);
            if (lecture == null || lecture.IsDeleted == true)
            {
                return false;
            }

            lecture.IsDeleted = true;
            await _lectureRepository.UpdateAsync(lecture);
            return true;
        }

        public async Task<bool> UpdateLectureAsync(Guid lectureId, UpdateLectureDTO lectureDTO, Guid userId)
        {
            var lecture = await _lectureRepository.GetByIdAsync(lectureId);
            if (lecture == null || lecture.IsDeleted == true)
            {
                return false;
            }

            lecture.Title = lectureDTO.Title;
            lecture.VideoUrl = lectureDTO.VideoUrl;
            await _lectureRepository.UpdateAsync(lecture);
            return true;
        }
    }
}
