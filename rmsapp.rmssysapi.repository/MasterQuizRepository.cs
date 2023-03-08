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

        #region Update  Master Quiz
        public async Task<bool> Update(IEnumerable<MasterQuiz> masterQuiz)
        {
            bool result = false;
            if (masterQuiz?.Count() > 0)
            {
                
                string version = U.Normalize(masterQuiz.Select(x => x.Version).FirstOrDefault());
                string subject = U.Normalize(masterQuiz.Select(x => x.SubjectName).FirstOrDefault());
                List<MasterQuiz> totalMasterQuizs = await _dbContext.AssignmentMaster.Where(x => x.Version == version && x.SubjectName == subject && x.IsActive).OrderBy(x => x.QuestionId).ToListAsync().ConfigureAwait(false);
                string tagVal=string.IsNullOrEmpty(masterQuiz.Select(x=>x.Tag).FirstOrDefault()) ? totalMasterQuizs.Select(x => x.Tag).FirstOrDefault() : U.Normalize(masterQuiz.Select(x => x.Tag).FirstOrDefault());
                //List<int> questionIds = masterQuiz.Select(x => x.QuestionId).ToList();
                List<MasterQuiz> updateQuizzes = new List<MasterQuiz>();
                foreach (var updateQuiz in totalMasterQuizs)
                {
                    MasterQuiz modifiedQuiz = masterQuiz.Where(x => x.QuestionId == updateQuiz.QuestionId).SingleOrDefault();
                    if (modifiedQuiz!=null)
                    {
                        updateQuiz.Question = string.IsNullOrEmpty(modifiedQuiz.Question)? updateQuiz.Question: modifiedQuiz.Question;
                        updateQuiz.QuestionType = string.IsNullOrEmpty(modifiedQuiz.QuestionType) ? updateQuiz.QuestionType : modifiedQuiz.QuestionType;
                        updateQuiz.QuestionOptions = modifiedQuiz.QuestionOptions.Length>-1? modifiedQuiz.QuestionOptions: updateQuiz.QuestionOptions;
                        updateQuiz.QuestionAnswers = modifiedQuiz.QuestionAnswers.Length > -1 ? modifiedQuiz.QuestionAnswers : updateQuiz.QuestionAnswers;
                        updateQuiz.QuestionAnswersIds = modifiedQuiz.QuestionAnswersIds.Length > -1 ? modifiedQuiz.QuestionAnswersIds : updateQuiz.QuestionAnswersIds;
                        updateQuiz.IsActive = modifiedQuiz.IsActive;
                        updateQuiz.UpdatedBy = modifiedQuiz.UpdatedBy;
                        updateQuiz.UpdatedDate = modifiedQuiz.UpdatedDate;
                        updateQuiz.Tag = tagVal;
                        updateQuizzes.Add(updateQuiz);
                    }
                }
                if (updateQuizzes.Count>0)
                {
                    totalMasterQuizs.ForEach(x => x.Tag = tagVal);
                    _dbContext.AssignmentMaster.UpdateRange(updateQuizzes);
                    //_dbContext.AssignmentMaster.UpdateRange(_dbContext.AssignmentMaster.Where(x => x.Version == version && x.SubjectName == subject && x.IsActive).ForEachAsync(x=>x.Tag= tagVal).);
                    await _dbContext.SaveChangesAsync();
                    result = true;
                }

            }
            return result;
        }
        #endregion

        #region Delete Quiz Set Wise
        public async Task<bool> DeleteQuizSet(string version, string subjectName)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(version) &&!string.IsNullOrEmpty(subjectName))
            {
                _dbContext.AssignmentMaster.RemoveRange(await _dbContext.AssignmentMaster.Where(x=>x.Version==version && x.SubjectName==subjectName).ToListAsync());
                await _dbContext.SaveChangesAsync().ConfigureAwait(false);
                result = true;
            }
            return result;
        }
        #endregion
        #region Delete Question Id With version and SubjectName
        public async Task<bool> DeleteQuestion(int questionId,string version, string subjectName)
        {
            bool result = false;
            if (questionId>0 && !string.IsNullOrEmpty(version) && !string.IsNullOrEmpty(subjectName))
            {
                _dbContext.AssignmentMaster.Remove(await _dbContext.AssignmentMaster.Where(x =>x.QuestionId==questionId &&  x.Version == version && x.SubjectName == subjectName).SingleOrDefaultAsync());
                await _dbContext.SaveChangesAsync().ConfigureAwait(false);
                result = true;
            }
            return result;
        }
        #endregion

        #region Get Questions
        public async Task<IEnumerable<MasterQuiz>> GetQuestions(string version, string subject)
        {
                var questions = await _dbContext.AssignmentMaster.Where(x => x.Version == version && x.SubjectName == subject &&x.IsActive).ToListAsync();
                return questions;               
        }

        #endregion
        #region Get Multi Set Questions
         public async Task<IEnumerable<MasterQuiz>>  GetQuestions(List<QuizSet> requestedQuizSets)
        {
            List<MasterQuiz> masterQuizzes = new List<MasterQuiz>();
            if (requestedQuizSets?.Count > 0)
            {
                foreach (var item in requestedQuizSets)
                {
                    List<MasterQuiz> MasterQuiz = await _dbContext.AssignmentMaster.Where(x => x.Version == U.Normalize(item.Version) && x.SubjectName == U.Normalize(item.SubjectName) && x.IsActive).ToListAsync().ConfigureAwait(false);
                    if (MasterQuiz.Count > 0)
                    {
                        masterQuizzes.AddRange(MasterQuiz);
                    }
                }
            }
            return masterQuizzes;
        }
        #endregion
        #region Get Questions By SubjectName
        public async Task<IEnumerable<MasterQuiz>> GetQuizDetails(string subject)
        {
            List<MasterQuiz> questions = new List<MasterQuiz>();
            if (!string.IsNullOrEmpty(subject))
            {
                questions = await _dbContext.AssignmentMaster.Where(x => x.SubjectName == subject && x.IsActive).ToListAsync().ConfigureAwait(false);
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
                    List<MasterQuiz> MasterQuiz = await _dbContext.AssignmentMaster.Where(x =>item.QuestionIds.Contains(x.QuestionId)&& x.Version ==U.Normalize(item.Version) && x.SubjectName == U.Normalize(item.SubjectName) && x.IsActive).ToListAsync().ConfigureAwait(false);
                    if (MasterQuiz.Count>0) {
                        masterQuizzes.AddRange(MasterQuiz);
                    } 
                }
            }
            return masterQuizzes;
        }
        #endregion
        #region  Get Total Questions info Based on Search Query
        public async Task<IEnumerable<MasterQuiz>> GetMultipleSetQuestionsList(List<string> searcKeys)
        {
            List<MasterQuiz> masterQuizzes = new List<MasterQuiz>();
            if (searcKeys?.Count > 0)
            {
                foreach (var item in searcKeys)
                {
                    List<MasterQuiz> MasterQuiz = await _dbContext.AssignmentMaster.Where(x => (x.SubjectName.Contains(U.Normalize(item)) || x.Tag.Contains(U.Normalize(item)) )&& x.IsActive).ToListAsync().ConfigureAwait(false);
                    if (MasterQuiz.Count > 0)
                    {
                        masterQuizzes.AddRange(MasterQuiz);
                    }
                }
            }
            return masterQuizzes;
        }
        #endregion
    }
}
