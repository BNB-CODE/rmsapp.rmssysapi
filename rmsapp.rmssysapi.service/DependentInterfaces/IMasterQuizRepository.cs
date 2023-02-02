
using rmsapp.rmssysapi.service.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace rmsapp.rmssysapi.service.DependentInterfaces
{
    public interface IMasterQuizRepository
    {
        Task<int> GteLatestQuestionId(int setNumber, string subjectName);
        Task<bool> Add(IEnumerable<MasterQuiz> masterQuiz);

        Task<List<MasterQuiz>> GetQuestions(int set, string subject);
        //Task<IEnumerable<MasterQuiz>> GetAllQuizDetails();
        //Task<bool> Update(MasterQuiz masterQuiz);
        //Task<bool> Delete(int questionId, int setNumber, string subjectName);
    }
}
