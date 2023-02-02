
using Npgsql;
using rmsapp.rmssysapi.service.DependentInterfaces;
using rmsapp.rmssysapi.service.Models;
using rmsapp.rmssysapi.service.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rmsapp.rmssysapi.service.Impl
{
    public class MasterQuizService: IMasterQuizService
    {
        private readonly IMasterQuizRepository _masterQuizRepository;
        public MasterQuizService(IMasterQuizRepository masterQuizRepository)
        {
            _masterQuizRepository = masterQuizRepository;
        }
        public async Task<bool> Add(int setNumber,string subjectName,IEnumerable<QuizDetails> masterQuiz)
        {
            try
            {
                int maxQuestionId = await _masterQuizRepository.GteLatestQuestionId(setNumber, subjectName).ConfigureAwait(false);
                List<MasterQuiz> masters = new List<MasterQuiz>();
                foreach (var item in masterQuiz)
                {
                    MasterQuiz quiz = new MasterQuiz();
                    string[] questionOptions = !string.IsNullOrEmpty(item.QuestionOptions) ? ((item.QuestionOptions).Split(',')).Select(t => t).ToArray() : null;
                    string[] questionAnswers = !string.IsNullOrEmpty(item.QuestionAnswers) ? ((item.QuestionAnswers).Split(',')).Select(t => t).ToArray() : null;
                    
                    quiz.QuestionId = maxQuestionId + 1;
                    quiz.SetNumber = setNumber;
                    quiz.SubjectName = (subjectName.Trim()).ToUpper();
                    quiz.Question = item.Question;
                    quiz.QuestionType = item.QuestionType;
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

       public async Task<List<CandidateQuestions>> GetCandidateAssignment(int set, string subject)
        {

            try
            {
                var masterQuiz = await _masterQuizRepository.GetQuestions(set, subject);
                var result = masterQuiz.Select(masterQuiz => new CandidateQuestions
                {
                    QuestionId = masterQuiz.QuestionId,
                    SetNumber = masterQuiz.SetNumber,
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

        public async  Task<List<SubjectExpertQuestions>> GetSubjectExpertQuestions(int set, string subject)
        {
            try
            {
                var masterQuiz = await _masterQuizRepository.GetQuestions(set, subject);
                var result = masterQuiz.Select(masterQuiz => new SubjectExpertQuestions     {
                    QuestionId = masterQuiz.QuestionId,
                    SetNumber = masterQuiz.SetNumber,
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
    }
}
