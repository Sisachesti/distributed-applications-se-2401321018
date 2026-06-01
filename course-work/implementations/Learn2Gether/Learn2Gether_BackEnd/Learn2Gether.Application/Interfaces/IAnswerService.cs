using Learn2Gether.Application.DTOs.Requests.Answer;
using Learn2Gether.Domain.Entities;

namespace Learn2Gether.Application.Interfaces
{
    public interface IAnswerService
    {
        Task<bool> SaveAnswerAsync(Guid questionId, SaveAnswerDTO answer, User user);
        Task<bool> EditAnswerAsync(Guid answerId, EditAnswerDTO answer, User user);
        Task<bool> DeleteAnswerAsync(Guid answerId, User user);
    }
}
