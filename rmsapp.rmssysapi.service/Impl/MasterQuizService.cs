
using Npgsql;
using rmsapp.rmssysapi.service.DependentInterfaces;
using rmsapp.rmssysapi.service.Models;
using rmsapp.rmssysapi.service.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using workforceapp.workforcesysapi.Service.Utils;

namespace rmsapp.rmssysapi.service.Impl
{
    public class MasterQuizService: IMasterQuizService
    {
        private readonly IMasterQuizRepository _masterQuizRepository;
        public MasterQuizService(IMasterQuizRepository masterQuizRepository)
        {
            _masterQuizRepository = masterQuizRepository;
        }
        public async Task<bool> Add(string version,string subjectName, string tag,IEnumerable<QuizDetails> masterQuiz)
        {
            try
            {
                int maxQuestionId = await _masterQuizRepository.GteLatestQuestionId(version, subjectName).ConfigureAwait(false);
                List<MasterQuiz> masters = new List<MasterQuiz>();
                foreach (var item in masterQuiz)
                {
                    MasterQuiz quiz = new MasterQuiz();
                    string[] questionOptions = !string.IsNullOrEmpty(item.QuestionOptions) ? ((item.QuestionOptions).Split(',')).Select(t => t).ToArray() : null;
                    string[] questionAnswers = !string.IsNullOrEmpty(item.QuestionAnswers) ? ((item.QuestionAnswers).Split(',')).Select(t => t).ToArray() : null;
                    
                    quiz.QuestionId = maxQuestionId + 1;
                    quiz.Version = U.Convert(version);
                    quiz.SubjectName =U.Convert(subjectName);
                    quiz.Tag = tag;
                    quiz.Question = item.Question;
                    quiz.QuestionType = ((item.QuestionType).Trim()).ToUpper();
                    quiz.QuestionOptions = !string.IsNullOrEmpty(item.QuestionOptions) ?((item.QuestionOptions).Split(',')).Select(t => t).ToArray():null; 
                    quiz.QuestionAnswers = !string.IsNullOrEmpty(item.QuestionAnswers) ? ((item.QuestionAnswers).Split(',')).Select(t => t).ToArray() : null;
                    if (questionOptions.Length>0 && questionAnswers.Length>0)
                    {
                        string[] questionAnswerIds = GetAnswerIds(questionOptions, questionAnswers).ToArray();
                        quiz.QuestionAnswersIds = questionAnswerIds;
                    }
                    quiz.CreatedDate = DateTime.Now;
                    //quiz.UpdatedDate = DateTime.Now;
                    quiz.IsActive = true;
                    //quiz.CreatedBy= urrentuser
                    //quiz.UpdatedBy= urrentuser
                    masters.Add(quiz);
                    maxQuestionId++;
                }
                var result = await _masterQuizRepository.Add(masters).ConfigureAwait(false);
                return result;
              
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"MasterQuizSerice::Add:: Save MasterQuizSerice failed {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"MasterQuizSerice::Add:: Save MasterQuizSerice failed {ex.Message}");
            }
        }

        public async Task<bool> Update(string version, string subjectName, string tag, IEnumerable<UpdateQuizDetails> masterQuiz)
        {
            try
            {
                List<MasterQuiz> masters = new List<MasterQuiz>();
                foreach (var item in masterQuiz)
                {
                    MasterQuiz quiz = new MasterQuiz();
                    string[] questionOptions = !string.IsNullOrEmpty(item.QuestionOptions) ? ((item.QuestionOptions).Split(',')).Select(t => t).ToArray() : null;
                    string[] questionAnswers = !string.IsNullOrEmpty(item.QuestionAnswers) ? ((item.QuestionAnswers).Split(',')).Select(t => t).ToArray() : null;

                    quiz.QuestionId = item.QuestionId;
                    quiz.Version = U.Convert(version);
                    quiz.SubjectName = U.Convert(subjectName);
                    quiz.Tag = tag;
                    quiz.Question = item.Question;
                    quiz.QuestionType = ((item.QuestionType).Trim()).ToUpper();
                    quiz.QuestionOptions = !string.IsNullOrEmpty(item.QuestionOptions) ? ((item.QuestionOptions).Split(',')).Select(t => t).ToArray() : null;
                    quiz.QuestionAnswers = !string.IsNullOrEmpty(item.QuestionAnswers) ? ((item.QuestionAnswers).Split(',')).Select(t => t).ToArray() : null;
                    if (questionOptions.Length > 0 && questionAnswers.Length > 0)
                    {
                        string[] questionAnswerIds = GetAnswerIds(questionOptions, questionAnswers).ToArray();
                        quiz.QuestionAnswersIds = questionAnswerIds;
                    }
                    quiz.UpdatedDate = DateTime.Now;
                    quiz.IsActive = true;
                    //quiz.UpdatedBy= urrentuser
                    masters.Add(quiz);
                }
                var result = await _masterQuizRepository.Update(masters).ConfigureAwait(false);
                return result;

            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"MasterQuizSerice::Update:: Update MasterQuizSerice failed {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"MasterQuizSerice::Update:: Update MasterQuizSerice failed {ex.Message}");
            }
        }
        public async Task<bool> DeleteQuizSet(string version, string subjectName)
        {

            try
            {
                var result = await _masterQuizRepository.DeleteQuizSet(version, subjectName).ConfigureAwait(false);
                return result;
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"MasterQuizSerice::DeleteQuizSet:: Delete Quiz set  failed {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"MasterQuizSerice::DeleteQuizSet:: Delete Quiz set failed {ex.Message}");
            }

        }
        public async Task<bool> DeleteQuestion(int questionId, string version, string subjectName)
        {

            try
            {
                var result = await _masterQuizRepository.DeleteQuizSet(version, subjectName).ConfigureAwait(false);
                return result;
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"MasterQuizSerice::DeleteQuestion:: Delete question in Quiz Set failed {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"MasterQuizSerice::DeleteQuestion:: Delete question in Quiz Set failed {ex.Message}");
            }

        }
        public async Task<List<CandidateQuestions>> GetCandidateAssignment(List<QuizSet> requestedQuizSets)
        {

            try
            {
                var masterQuiz = await _masterQuizRepository.GetQuestions(requestedQuizSets).ConfigureAwait(false);
                var result = masterQuiz.Select(masterQuiz => new CandidateQuestions
                {
                    QuestionId = masterQuiz.QuestionId,
                    Version = masterQuiz.Version,
                    SubjectName = masterQuiz.SubjectName,
                    Question = masterQuiz.Question,
                    QuestionType = masterQuiz.QuestionType,
                    QuestionOptions = masterQuiz.QuestionOptions
                }).ToList();

                return result;
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"MasterQuizSerice::getCandidateAssignment:: Get Assignment failed {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"MasterQuizSerice::getCandidateAssignment:: Get Assignment failed {ex.Message}");
            }
        }

        public async  Task<IEnumerable<SubjectExpertQuestions>> GetMasterQuestions(string version, string subject)
        {
            try
            {
                var masterQuiz = await _masterQuizRepository.GetQuestions(version, subject).ConfigureAwait(false);
                var result = masterQuiz.Select(masterQuiz => new SubjectExpertQuestions     {
                    QuestionId = masterQuiz.QuestionId,
                    Version = masterQuiz.Version,
                    SubjectName = masterQuiz.SubjectName,
                    Question = masterQuiz.Question,
                    QuestionType = masterQuiz.QuestionType,
                    QuestionOptions = masterQuiz.QuestionOptions,
                    QuestionAnswers= masterQuiz.QuestionAnswers,
                    QuestionAnswersIds= masterQuiz.QuestionAnswersIds
                }).ToList();

                return result;
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"MasterQuizSerice::getCandidateAssignment:: Get Assignment failed {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"MasterQuizSerice::getCandidateAssignment:: Get Assignment failed {ex.Message}");
            }

        }
        public async Task<IEnumerable<SubjectDetails>> GetQuizDetails(string subject)
        {
            try
            {
                var masterQuiz = await _masterQuizRepository.GetQuizDetails(subject).ConfigureAwait(false);
                var result = (from x in masterQuiz
                               group x by new
                               {
                                 x.Version,
                                 x.SubjectName,
                                 } into g
                                 select new SubjectDetails
                                 {
                                     Version = g.Key.Version,
                                     SubjectName = g.Key.SubjectName,
                                     TotalQuestionsCount = g.Count(),
                                     CreatedBy= masterQuiz.Where(x=>x.Version==g.Key.Version && x.SubjectName==g.Key.SubjectName).Select(x=>x.CreatedBy).FirstOrDefault(),
                                     UpdatedBy = masterQuiz.Where(x => x.Version == g.Key.Version && x.SubjectName == g.Key.SubjectName).Select(x => x.UpdatedBy).LastOrDefault(),
                                     CreatedDate = masterQuiz.Where(x => x.Version == g.Key.Version && x.SubjectName == g.Key.SubjectName).Select(x => x.CreatedDate).FirstOrDefault(),
                                     UpdatedDate = masterQuiz.Where(x => x.Version == g.Key.Version && x.SubjectName == g.Key.SubjectName).Select(x => x.UpdatedDate).LastOrDefault(),
                                 }).ToList();
                return result;
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"MasterQuizSerice::GetQuizDetails:: Get QuizDetails failed {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"MasterQuizSerice::GetQuizDetails:: Get QuizDetails failed {ex.Message}");
            }

        }

        private List<string> GetAnswerIds(string[] totalOptions,string[] answersList)
        {
            List<int> indexes = new List<int>();
            foreach (string searchAnswer in answersList)
            {
                int index = Array.IndexOf(totalOptions, searchAnswer);
                if (index > -1)
                {
                    indexes.Add(index);
                }
            }
            List<string> questionIds = UploadExcelSheetOrder.AlphabetName(indexes);
            return questionIds;
        }
        public async Task<IEnumerable<CandidateQuestions>> GetCandidateAssignmentMultipleSetsList(List<InterviewerQuizSet> interviewerQuizSets)
        {
            try
            {

                var masterQuiz = await _masterQuizRepository.GetMultipleSetQuestionsList(interviewerQuizSets).ConfigureAwait(false);
                var result = masterQuiz.Select(masterQuiz => new CandidateQuestions
                {
                    QuestionId = masterQuiz.QuestionId,
                    Version = masterQuiz.Version,
                    SubjectName = masterQuiz.SubjectName,
                    Question = masterQuiz.Question,
                    QuestionType = masterQuiz.QuestionType,
                    QuestionOptions = masterQuiz.QuestionOptions
                }).ToList();

                return result;
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"MasterQuizSerice::GetCandidateAssignmentList:: Get Assignment failed {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"MasterQuizSerice::GetCandidateAssignmentList:: Get Assignment failed {ex.Message}");
            }
        }
        public async Task<IEnumerable<SubjectExpertQuestions>> GetMasterQuestionsMultipleSetsList(List<InterviewerQuizSet> interviewerQuizSets)
        {
            try
            {
                var masterQuiz = await _masterQuizRepository.GetMultipleSetQuestionsList(interviewerQuizSets).ConfigureAwait(false);
                var result = masterQuiz.Select(masterQuiz => new SubjectExpertQuestions
                {
                    QuestionId = masterQuiz.QuestionId,
                    Version = masterQuiz.Version,
                    SubjectName = masterQuiz.SubjectName,
                    Question = masterQuiz.Question,
                    QuestionType = masterQuiz.QuestionType,
                    QuestionOptions = masterQuiz.QuestionOptions,
                    QuestionAnswers = masterQuiz.QuestionAnswers,
                    QuestionAnswersIds = masterQuiz.QuestionAnswersIds
                }).ToList();

                return result;
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"MasterQuizSerice::GetMasterQuestionsMultipleSetsList:: GetMasterQuestionsMultipleSetsList failed {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"MasterQuizSerice::GetMasterQuestionsMultipleSetsList:: GetMasterQuestionsMultipleSetsList failed {ex.Message}");
            }

        }
    }
}
