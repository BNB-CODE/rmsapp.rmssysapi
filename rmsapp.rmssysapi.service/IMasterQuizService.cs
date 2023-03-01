

using rmsapp.rmssysapi.service.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace rmsapp.rmssysapi.service
{
    public interface IMasterQuizService
    {
        Task<bool> Add(string version, string subjectName,string tag, IEnumerable<QuizDetails> masterQuiz);
        Task<List<CandidateQuestions>> GetCandidateAssignment(string version, string subject);

        Task<IEnumerable<SubjectExpertQuestions>> GetMasterQuestions(string version, string subject);

        Task<IEnumerable<SubjectDetails>> GetQuizDetails(string subject);

        Task<IEnumerable<CandidateQuestions>> GetCandidateAssignmentMultipleSetsList(List<InterviewerQuizSet> interviewerQuizSets);
        Task<IEnumerable<SubjectExpertQuestions>> GetMasterQuestionsMultipleSetsList(List<InterviewerQuizSet> interviewerQuizSets);

        //Task<IEnumerable<MasterQuiz>> GetAllQuizDetails();
        //Task<bool> Update(MasterQuiz masterQuiz);
        //Task<bool> Delete(int questionId,int setNumber,string subjectName);
    }
}
