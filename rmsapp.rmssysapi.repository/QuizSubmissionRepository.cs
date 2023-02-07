using rmsapp.rmssysapi.service.DependentInterfaces;
using rmsapp.rmssysapi.service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace rmsapp.rmssysapi.repository
{
    public class QuizSubmissionRepository: IQuizSubmissionRepository
    {
        private readonly PostgreSqlContext _dbContext;

        public QuizSubmissionRepository(PostgreSqlContext context)
        {
            _dbContext = context;

        }
        #region Save Quizs
        public async Task<bool> Add(QuizSubmission quiz)
        {
            bool result = false;
            if (quiz != null)
            {
                await _dbContext.QuizSubmissions.AddAsync(quiz);
                await _dbContext.SaveChangesAsync();
                result = true;
            }
            return result;
        }
        #endregion

        #region Get Quizs Results
        public async Task<QuizSubmission> GetQuizDetails(int quizId)
        {
            QuizSubmission quiz = new QuizSubmission();
            if (quizId > 0)
            {
                quiz = await _dbContext.QuizSubmissions.Where(x => x.QuizId == quizId).SingleOrDefaultAsync().ConfigureAwait(false);
            }
            return quiz;

        }
        #endregion
    }
}
