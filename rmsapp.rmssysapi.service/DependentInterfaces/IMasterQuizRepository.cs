
using rmsapp.rmssysapi.service.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace rmsapp.rmssysapi.service.DependentInterfaces
{
    public interface IMasterQuizRepository
    {
        Task<int> GteLatestQuestionId(string version, string subjectName);
        Task<bool> Add(IEnumerable<MasterQuiz> masterQuiz);
        Task<bool> Update(IEnumerable<MasterQuiz> masterQuiz);
        Task<bool> DeleteQuizSet(string version, string subjectName);
        Task<bool> DeleteQuestion(int questionId, string version, string subjectName);
        Task<IEnumerable<MasterQuiz>> GetQuestions(List<QuizSet> requestedQuizSets);
        Task<IEnumerable<MasterQuiz>> GetQuestions(string version, string subjec);
        Task<IEnumerable<MasterQuiz>> GetQuizDetails(string subject);

        Task<IEnumerable<MasterQuiz>> GetMultipleSetQuestionsList(List<InterviewerQuizSet> interviewerQuizzes);

        //Task<IEnumerable<MasterQuiz>> GetAllQuizDetails();
        //Task<bool> Update(MasterQuiz masterQuiz);
        //Task<bool> Delete(int questionId, int setNumber, string subjectName);
    }
}
