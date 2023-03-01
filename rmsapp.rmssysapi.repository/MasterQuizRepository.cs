using rmsapp.rmssysapi.service.DependentInterfaces;
using rmsapp.rmssysapi.service.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using workforceapp.workforcesysapi.Service.Utils;

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
        public async Task<int> GteLatestQuestionId(string version, string subjectName)
        {
            var total = await _dbContext.AssignmentMaster.Where(r => r.Version == version.ToUpper() && r.SubjectName == subjectName.ToUpper()).ToListAsync();
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
        public async Task<IEnumerable<MasterQuiz>> GetQuestions(string version, string subject)
        {
                var questions = await _dbContext.AssignmentMaster.Where(x => x.Version == version && x.SubjectName == subject).ToListAsync();
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
        #region Get Toatla Questions with Multiple Sets
        public async Task<IEnumerable<MasterQuiz>> GetMultipleSetQuestionsList(List<InterviewerQuizSet> interviewerQuizzes)

        {
            List<MasterQuiz> masterQuizzes = new List<MasterQuiz>();
            if (interviewerQuizzes?.Count>0)
            {
                foreach (var item in interviewerQuizzes)
                {
                    List<MasterQuiz> MasterQuiz = await _dbContext.AssignmentMaster.Where(x =>item.QuestionIds.Contains(x.QuestionId)&& x.Version ==U.Convert(item.Version) && x.SubjectName == U.Convert(item.SubjectName)).ToListAsync().ConfigureAwait(false);
                    masterQuizzes.AddRange(MasterQuiz);
                }
            }
            return masterQuizzes;
        }
        #endregion
    }
}
