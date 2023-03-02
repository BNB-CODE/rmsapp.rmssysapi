

using rmsapp.rmssysapi.service.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace rmsapp.rmssysapi.service
{
    public interface IMasterQuizService
    {
        Task<bool> Add(string version, string subjectName,string tag, IEnumerable<QuizDetails> masterQuiz);
        Task<bool> Update(string version, string subjectName, string tag, IEnumerable<UpdateQuizDetails> masterQuiz);
        Task<bool> DeleteQuizSet(string version, string subjectName);
        Task<bool> DeleteQuestion(int questionId, string version, string subjectName);
        Task<List<CandidateQuestions>> GetCandidateAssignment(List<QuizSet> requestedQuizSets);

        Task<IEnumerable<SubjectExpertQuestions>> GetMasterQuestions(string version, string subject);

        Task<IEnumerable<SubjectDetails>> GetQuizDetails(string subject);

        Task<IEnumerable<CandidateQuestions>> GetCandidateAssignmentMultipleSetsList(List<InterviewerQuizSet> interviewerQuizSets);
        Task<IEnumerable<SubjectExpertQuestions>> GetMasterQuestionsMultipleSetsList(List<InterviewerQuizSet> interviewerQuizSets);

        //Task<IEnumerable<MasterQuiz>> GetAllQuizDetails();
        //Task<bool> Update(MasterQuiz masterQuiz);
        //Task<bool> Delete(int questionId,int setNumber,string subjectName);
    }
}
