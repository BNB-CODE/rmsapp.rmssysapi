using Microsoft.EntityFrameworkCore;
using rmsapp.rmssysapi.service.DependentInterfaces;
using rmsapp.rmssysapi.service.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rmsapp.rmssysapi.repository
{

    public class QuizRepository: IQuizRepository
    {
        private readonly PostgreSqlContext _dbContext;

        public QuizRepository(PostgreSqlContext context)
        {
            _dbContext = context;

        }
        #region Get Latest QuestionId
        public async Task<int> GteLatestQuizId()
        {
            var total = await _dbContext.Quizzes.ToListAsync();
            var result = total.Any() ? total.Select(x => x.QuizId).Max() : 0;
            return result;
        }
        #endregion

        #region Save Quiz info
        public async Task<bool> Add(Quiz quiz)
        {
            bool result = false;
            if (quiz!=null)
            {
                await _dbContext.Quizzes.AddAsync(quiz);
                await _dbContext.SaveChangesAsync();
                result = true;
            }
            return result;
        }
        #endregion
        #region Update Quiz info
        public async Task<bool> UpdateQuizInfo(Quiz quiz)
        {
            bool result = false;
            if (quiz != null)
            {
                Quiz existingQuiz = await _dbContext.Quizzes.Where(x => x.QuizId == quiz.QuizId).SingleOrDefaultAsync();
                if (existingQuiz != null)
                {
                    existingQuiz.CandidateId = quiz.CandidateId;
                    existingQuiz.LoginAttempts = quiz.LoginAttempts;
                    existingQuiz.LastLoggedIn = quiz.LastLoggedIn;
                    existingQuiz.QuizSubmittedAt = quiz.QuizSubmittedAt;
                    _dbContext.Quizzes.Update(existingQuiz);
                    await _dbContext.SaveChangesAsync();
                    result = true;
                }
            }
            return result;
        }
        #endregion
        #region get Quiz info
        public async Task<Quiz> GetQuizDetails(int quizId){
            Quiz quiz = new Quiz();
            if (quizId>0)
            {
                quiz = await _dbContext.Quizzes.Where(x => x.QuizId == quizId).SingleOrDefaultAsync().ConfigureAwait(false);
            }
            return quiz;
        }
        #endregion

        #region get Quizzes info
        public async Task<IEnumerable<Quiz>> GetTotalQuizDetails()
        {
            List<Quiz> quiz = await _dbContext.Quizzes.Where(x=>x.IsActive).ToListAsync().ConfigureAwait(false);
            return quiz;
        }
        #endregion

    }
}
