using rmsapp.rmssysapi.service.DependentInterfaces;
using rmsapp.rmssysapi.service.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;

namespace rmsapp.rmssysapi.repository
{
    public class MasterQuizRepository : IMasterQuizRepository
    {
        private readonly PostgreSqlContext _dbContext;

        public MasterQuizRepository(PostgreSqlContext context)
        {
            _dbContext = context;

        }
        #region Lastest Master QuestionId
        public async Task<int> GteLatestQuestionId(int setNumber, string subjectName)
        {
            var total = await _dbContext.AssignmentMaster.Where(r => r.SetNumber == setNumber && r.SubjectName == subjectName.ToUpper()).ToListAsync();
            var result = total.Any() ? total.Select(x => x.QuestionId).Max() : 0;
            return result;
        }
        #endregion

        #region Save  Master Quiz
        public async Task<bool> Add(IEnumerable<MasterQuiz> masterQuiz)
        {
            bool result = false;
            if (masterQuiz?.Count()>0)
            {
                await _dbContext.AssignmentMaster.AddRangeAsync(masterQuiz);
                await _dbContext.SaveChangesAsync();
                result = true;
            }
            return result;
        }
        #endregion

        #region Get Questions
        public async Task<IEnumerable<MasterQuiz>> GetQuestions(int set, string subject)
        {
                var questions = await _dbContext.AssignmentMaster.Where(x => x.SetNumber == set && x.SubjectName == subject).ToListAsync();
                return questions;               
        }

        #endregion
        #region Get Questions By SubjectName
        public async Task<IEnumerable<MasterQuiz>> GetQuizDetails(string subject)
        {
            List<MasterQuiz> questions = new List<MasterQuiz>();
            if (!string.IsNullOrEmpty(subject))
            {
                questions = await _dbContext.AssignmentMaster.Where(x => x.SubjectName == subject).ToListAsync().ConfigureAwait(false);
            }
            else
            {
                questions = await _dbContext.AssignmentMaster.ToListAsync().ConfigureAwait(false);
            }
            return questions;
        }

        #endregion
    }
}
