using Learn2Gether.Application.DTOs.Requests.Lecture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learn2Gether.Application.Interfaces
{
    public interface ILectureService
    {
        Task<bool> AddLectureToModuleAsync(Guid moduleId, AddNewLectureDTO newLecture, Guid userId);
        Task<bool> UpdateLectureAsync(Guid lectureId, UpdateLectureDTO lecture, Guid userId);
        Task<bool> DeleteLectureAsync(Guid lectureId, Guid userId);
    }
}
