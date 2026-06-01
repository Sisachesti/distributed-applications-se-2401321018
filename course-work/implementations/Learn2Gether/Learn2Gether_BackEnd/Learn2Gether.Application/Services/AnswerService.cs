using Learn2Gether.Application.DTOs.Requests.Answer;
using Learn2Gether.Application.DTOs.Requests.Question;
using Learn2Gether.Application.Interfaces;
using Learn2Gether.Domain.Entities;
using Learn2Gether.Infastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learn2Gether.Application.Services
{
    public class AnswerService : IAnswerService
    {
        private readonly IBaseRepository<Question, Guid> _questionRepository;
        private readonly IBaseRepository<Lecture, Guid> _lectureRepository;
        private readonly IBaseRepository<Answer, Guid> _answerRepository;

        public AnswerService(IBaseRepository<Question, Guid> questionRepository, IBaseRepository<Lecture, Guid> lectureRepository, IBaseRepository<Answer, Guid> answerRepository)
        {
            _questionRepository = questionRepository;
            _lectureRepository = lectureRepository;
            _answerRepository = answerRepository;
        }

        public async Task<bool> DeleteAnswerAsync(Guid answerId, User user)
        {
            var answer = await this._answerRepository.GetByIdAsync(answerId);
            if (answer == null || answer.UserId != user.Id || answer.IsDeleted == true)
            {
                return false;
            }

            answer.IsDeleted = true;
            await this._answerRepository.UpdateAsync(answer);
            return true;

        }

        public async Task<bool> EditAnswerAsync(Guid answerId, EditAnswerDTO answer, User user)
        {
            var answerToEdit = await this._answerRepository.GetByIdAsync(answerId);
            if (answerToEdit == null || answerToEdit.IsDeleted == true || answerToEdit.UserId != user.Id)
            {
                return false;
            }

            answerToEdit.Title = answer.Title;
            answerToEdit.Content = answer.Content;

            await _answerRepository.UpdateAsync(answerToEdit);
            return true;
        }

        public async Task<bool> SaveAnswerAsync(Guid questionId, SaveAnswerDTO answer, User user)
        {
            var question = await _questionRepository.GetByIdAsync(questionId);
            if (question == null)
            {
                return false;
            }

            Guid lectureGuid = _lectureRepository.FirstOrDefault(l => l.LectureQuestions.Any(q => q.Id == questionId)).Id;

            var newAnswer = new Answer
            {
                Title = answer.Title,
                Content = answer.Content,
                QuestionId = questionId,
                UserId = user.Id,
                CreatedAt = DateTime.UtcNow,
                LectureId = lectureGuid,
                LikesCount = 0,
                DislikesCount = 0,
                IsAccepted = false,
            };

            await _answerRepository.AddAsync(newAnswer);
            return true;
        }
    }
}
