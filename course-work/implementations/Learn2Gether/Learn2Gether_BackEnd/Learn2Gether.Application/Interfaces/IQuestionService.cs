using Learn2Gether.Application.DTOs.Requests.Question;
using Learn2Gether.Domain.Entities;

namespace Learn2Gether.Application.Interfaces
{
    public interface IQuestionService
    {
        Task<bool> SaveQuestionAsync(Guid lectureId, SaveQuestionDTO question, User user);
        Task<bool> EditQuestionAsync(Guid questionId, EditQuestionDTO question, User user);
        Task<bool> DeleteQuestionAsync(Guid questionId, User user);
    }
}
