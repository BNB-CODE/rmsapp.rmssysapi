using Npgsql;
using rmsapp.rmssysapi.service.DependentInterfaces;
using rmsapp.rmssysapi.service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace rmsapp.rmssysapi.service.Impl
{
    public class QuizService : IQuizService
    {
        private readonly IQuizRepository _quizRepository;

        public QuizService(IQuizRepository quizRepository)
        {
            _quizRepository = quizRepository;

        }
        #region get LatestQuestionId
        public async Task<int> GteLatestQuizId()
        {
            try
            {
                int quizId = await _quizRepository.GteLatestQuizId().ConfigureAwait(false);
                return quizId;
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"QuizService::GteLatestQuizId:: GteLatestQuizId QuizSerice failed {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"QuizService::GteLatestQuizId:: GteLatestQuizId QuizSerice failed {ex.Message}");
            }
        }
        #endregion

        #region Save Quiz info
        public async Task<bool> Add(Quiz quiz)
        {
            try
            {
                var res = await _quizRepository.Add(quiz).ConfigureAwait(false);
                return res;
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"QuizService::Add:: Save QuizSerice failed {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"QuizService::Add:: Save QuizSerice failed {ex.Message}");
            }
        }
        #endregion
        #region Update Quiz info
        public async Task<bool> UpdateQuizInfo(Quiz quiz)
        {
            try
            {
                var res = await _quizRepository.UpdateQuizInfo(quiz).ConfigureAwait(false);
                return res;
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"QuizService::UpdateQuizInfo:: Update QuizSerice failed {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"QuizService::UpdateQuizInfo:: Update QuizSerice failed {ex.Message}");
            }
        }
        #endregion
        #region get Quiz info
        public async Task<Quiz> GetQuizDetails(int quizId)
        {
            var quiz = await _quizRepository.GetQuizDetails(quizId).ConfigureAwait(false);
            return quiz;
        }
        #endregion
    }
}
