using Learn2Gether.Application.DTOs.Requests.Question;
using Learn2Gether.Application.Interfaces;
using Learn2Gether.Domain.Entities;
using Learn2Gether.Infastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Learn2Gether.Application.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IBaseRepository<Lecture, Guid> _lectureRepository;
        private readonly IBaseRepository<Question, Guid> _questionRepository;
        private readonly IBaseRepository<Answer, Guid> _answerRepository;

        public QuestionService(IBaseRepository<Lecture, Guid> lectureRepository, IBaseRepository<Question, Guid> questionRepository, IBaseRepository<Answer, Guid> answerRepository)
        {
            _lectureRepository = lectureRepository;
            _questionRepository = questionRepository;
            _answerRepository = answerRepository;
        }

        public async Task<bool> DeleteQuestionAsync(Guid questionId, User user)
        {
            var question = await this._questionRepository.GetByIdAsync(questionId);
            if (question == null || question.User.UserName != user.UserName || question.IsDeleted == true)
            {
                return false;
            }

            var answers = await _answerRepository.GetAllAttached()
                .Where(a => a.QuestionId == questionId && a.IsDeleted == false)
                .ToListAsync();

            foreach (var answer in answers)
            {
                answer.IsDeleted = true;
                await this._answerRepository.UpdateAsync(answer);
            }
            question.IsDeleted = true;

            await this._questionRepository.UpdateAsync(question);
            return true;
        }

        public async Task<bool> EditQuestionAsync(Guid questionId, EditQuestionDTO question, User user)
        {
            var questionToEdit = await this._questionRepository.GetByIdAsync(questionId);
            if (questionToEdit == null || questionToEdit.IsDeleted == true || questionToEdit.UserId != user.Id)
            {
                return false;
            }

            questionToEdit.Title = question.Title;
            questionToEdit.Content = question.Content;

            await _questionRepository.UpdateAsync(questionToEdit);
            return true;
        }

        public async Task<bool> SaveQuestionAsync(Guid lectureId, SaveQuestionDTO question, User user)
        {
            var lecture = await _lectureRepository.GetByIdAsync(lectureId);
            if (lecture == null)
            {
                return false;
            }

            var newQuestion = new Question
            {
                Title = question.Title,
                Content = question.Content,
                LectureId = lectureId,
                UserId = user.Id,
                AskedOn = DateTime.UtcNow
            };

            await _questionRepository.AddAsync(newQuestion);
            return true;
        }
    }
}
