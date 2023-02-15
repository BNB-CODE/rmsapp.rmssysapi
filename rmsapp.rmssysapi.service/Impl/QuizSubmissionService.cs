using Npgsql;
using rmsapp.rmssysapi.service.DependentInterfaces;
using rmsapp.rmssysapi.service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace rmsapp.rmssysapi.service.Impl
{
    public class QuizSubmissionService:IQuizSubmissionService
    {
        private readonly IQuizSubmissionRepository _quizSubmissionRepository;

        public QuizSubmissionService(IQuizSubmissionRepository quizSubmissionRepository)
        {
            _quizSubmissionRepository = quizSubmissionRepository;

        }
        #region Save Quiz
        public async Task<bool> Add(QuizSubmission quiz)
        {
            try
            {
                var res = await _quizSubmissionRepository.Add(quiz).ConfigureAwait(false);
                return res;
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"QuizSubmissionService::Add:: Save  failed {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"QuizSubmissionService::Add:: Save  failed {ex.Message}");
            }
        }

        #endregion
        #region Get Quiz Answers
        public async Task<QuizSubmission> GetQuizDetails(int quizId)
        {
            try
            {
                var res = await _quizSubmissionRepository.GetQuizDetails(quizId).ConfigureAwait(false);
                return res;
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"QuizSubmissionService::GetQuizDetails:: Fetch Submitted Quiz  failed {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"QuizSubmissionService::GetQuizDetails:: Fetch Submitted Quiz  failed {ex.Message}");
            }
        }
        #endregion
        #region Get Total Quiz Detail
        public async Task<IEnumerable<QuizSubmission>> GetTotalQuizDetails()
        {
            try
            {
                var res = await _quizSubmissionRepository.GetTotalQuizDetails().ConfigureAwait(false);
                return res;
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"QuizSubmissionService::GetTotalQuizDetails:: Fetch Submitted Quiz  failed {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"QuizSubmissionService::GetTotalQuizDetails:: Fetch Submitted Quiz  failed {ex.Message}");
            }
        }
        #endregion
    }
}
