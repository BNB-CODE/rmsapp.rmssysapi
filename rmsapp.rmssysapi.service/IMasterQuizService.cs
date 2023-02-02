

using rmsapp.rmssysapi.service.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace rmsapp.rmssysapi.service
{
    public interface IMasterQuizService
    {
        Task<bool> Add(int setNumber, string subjectName, IEnumerable<QuizDetails> masterQuiz);
        Task<List<CandidateQuestions>> GetCandidateAssignment(int set, string subject);

        Task<List<SubjectExpertQuestions>> GetSubjectExpertQuestions(int set, string subject);
        //Task<IEnumerable<MasterQuiz>> GetAllQuizDetails();
        //Task<bool> Update(MasterQuiz masterQuiz);
        //Task<bool> Delete(int questionId,int setNumber,string subjectName);
    }
}
